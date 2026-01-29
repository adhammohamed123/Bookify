using Bookify.Application.Abstractions.Caching;
using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Email;
using Bookify.Domain.Abstractions.Repositories;
using Bookify.Infrastracture.Caching;
using Bookify.Infrastracture.Clock;
using Bookify.Infrastracture.Email;
using Bookify.Infrastracture.Keyclock;
using Bookify.Infrastracture.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Quartz;
using StackExchange.Redis;
using System.Reflection;

namespace Bookify.Infrastracture
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddScoped<IEmailSender, EmailSender>();

            AddDbContextWithRepositoryManager(services, configuration);
            AddAuthenticationAndKeyClockConfig(services, configuration);
            AddCaching(services, configuration);
            AddHealthChecks(services, configuration);
            return services;
        }

        private static void AddDbContextWithRepositoryManager(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString, npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName))// this for remember perpouse
                .UseSnakeCaseNamingConvention();
            });
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }

        private static void AddAuthenticationAndKeyClockConfig(IServiceCollection services, IConfiguration configuration)
        {
            var keyclockOptions = configuration.GetSection("KeyClock").Get<KeyclockOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = keyclockOptions!.Issure;
                    options.Audience = keyclockOptions.Audience;
                    options.MetadataAddress = keyclockOptions?.MetadataAddress!;
                    options.RequireHttpsMetadata = keyclockOptions!.RequireHttpsMetadata;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidAudience = keyclockOptions.Audience,
                        ValidIssuer = keyclockOptions.Issure,
                        NameClaimType = "preferred_username"
                    };
                });
        }

        private static void AddCaching(IServiceCollection services, IConfiguration configuration)
        {
            var CacheConnectionString = configuration.GetConnectionString("Cache") ?? throw new InvalidOperationException("cache ConStr not Specified");
           
            var conMultiplexer= ConnectionMultiplexer.Connect(CacheConnectionString);
            services.AddSingleton<IConnectionMultiplexer>(conMultiplexer); // this for opentelemetry can observe calls from this connection
            services.AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = "MyRedisCache";
                //options.Configuration = CacheConnectionString;
                options.ConnectionMultiplexerFactory = () => Task.FromResult<IConnectionMultiplexer>(conMultiplexer);
            });

            services.AddSingleton<ICacheService, CacheService>();
        }

        private static void AddHealthChecks(IServiceCollection services, IConfiguration configuration)
             => services.AddHealthChecks()
             .AddNpgSql(configuration.GetConnectionString("DefaultConnection")!,name:"Database Server")
             .AddRedis(configuration.GetConnectionString("Cache")!,name:"Redis caching server")
             .AddUrlGroup(new Uri("http://192.168.1.14:8080/realms/Bookify"),httpMethod:HttpMethod.Get,name:"Keyclock",failureStatus:HealthStatus.Unhealthy);
    }
}

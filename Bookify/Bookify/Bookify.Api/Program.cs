using Bookify.Api.Extentions;
using Bookify.Api.Middlewares;
using Bookify.Application;
using Bookify.Infrastracture;
using HealthChecks.UI.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using StackExchange.Redis;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options=>options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApi();

builder.Services
    .AddApplicationServices()
   .AddInfrastructureServices(builder.Configuration);

builder.Services.AddOpenTelemetry()
    .ConfigureResource(c=>c.AddService("Bookfiy"))
    .WithTracing(trace=>trace.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddNpgsql().AddRedisInstrumentation())
    .WithMetrics(metrics=>metrics.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddRuntimeInstrumentation())
    .UseOtlpExporter();

//builder.Services.AddHealthChecks();//.AddCheck<DBCustomHealthCheck>("Db_Check");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(config =>
    {
        config.SwaggerEndpoint("/openapi/v1.json", "Bookify Api v1");
    });
    await app.ApplayMigrationAsync();
    //app.AddDamyData();
}

//app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.Run();


#region Custom Health Check Implementation
//public class DBCustomHealthCheck(ApplicationDbContext dbContext) : IHealthCheck
//{
//    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
//    {
//        bool result = await dbContext.Database.CanConnectAsync();

//        return result is true ? HealthCheckResult.Healthy(): HealthCheckResult.Unhealthy();
//    }
//} 
#endregion
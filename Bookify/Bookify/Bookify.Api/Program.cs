using Bookify.Api.Extentions;
using Bookify.Application;
using Bookify.Infrastracture;
using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services
    .AddApplicationServices()
   .AddInfrastructureServices(builder.Configuration);

builder.Services.AddOpenTelemetry()
    .ConfigureResource(c=>c.AddService("Bookfiy"))
    .WithTracing(trace=>trace.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddNpgsql())
    .WithMetrics(metrics=>metrics.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddRuntimeInstrumentation())
    .UseOtlpExporter();
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
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

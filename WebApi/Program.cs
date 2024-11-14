using Application;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Serilog;
using System.Collections.Generic;
using WebApi.Endpoints;
using WebApi.Extensions;
using WebApi.Infrastructure;
using WebApi.Middleware;
using WebApi.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);

// Logging
builder.Host.UseSerilog(
    (context, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
    });

// Global exception handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// API versioning
builder.Services.AddApiVersioning(
    options =>
    {
        options.DefaultApiVersion = new ApiVersion(1);
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

// Swagger
builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var devOrigin = "angular_front";
builder.Services.AddCors(
    options =>
    {
        options.AddPolicy(name: devOrigin,
            policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod(); ;
            });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// CORS
app.UseCors(devOrigin);

// Middleware
app.UseMiddleware<RequestLogContextMiddleware>();

app.UseExceptionHandler();

app.UseSerilogRequestLogging();

// Add api versions
ApiVersionSet apiVersionSet = app.NewApiVersionSet()
     .HasApiVersion(new ApiVersion(1))
     .HasApiVersion(new ApiVersion(2))
     .ReportApiVersions()
     .Build();

RouteGroupBuilder versionedGroup = app.MapGroup("api/v{apiVersion:apiVersion}")
    .WithApiVersionSet(apiVersionSet);

// Map endpoints
versionedGroup.MapCategoryEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        options =>
        {
            IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();

            foreach (ApiVersionDescription description in descriptions)
            {
                string url = $"/swagger/{description.GroupName}/swagger.json";
                string name = description.GroupName.ToUpperInvariant();

                options.SwaggerEndpoint(url, name);
            }
        });

    app.ApplyMigrations();
}

app.Run();

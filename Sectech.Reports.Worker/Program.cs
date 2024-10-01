using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Sectech.Reports.Worker.HealthChecks;
using Sectech.Reports.Worker.RabbitMq;
using SecTech.Reports.Application.Services;
using SecTech.Reports.DAL.Infrastructure;
using SecTech.Reports.Domain.Interfaces.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddScoped<IWorkerService, WorkerService>();
builder.Services.AddSingleton<RabbitMqListener>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<RabbitMqListener>());
builder.Services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy("Server is working"))
                .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                .AddCheck<RabbitMqCheck>("RabbitMq");
                
    


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.Run();
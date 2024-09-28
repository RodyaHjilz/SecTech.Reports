using Sectech.Reports.Worker.RabbitMq;
using SecTech.Reports.Application.Services;
using SecTech.Reports.DAL.Infrastructure;
using SecTech.Reports.Domain.Interfaces.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddScoped<IWorkerService, WorkerService>();
builder.Services.AddHostedService<RabbitMqListener>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.Run();
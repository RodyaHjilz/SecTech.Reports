using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;
using Sectech.Reports.Worker.RabbitMq;

namespace Sectech.Reports.Worker.HealthChecks
{
    public class RabbitMqCheck : IHealthCheck
    {
        private readonly RabbitMqListener _rabbitMqListener;

        public RabbitMqCheck(RabbitMqListener rabbitMqListener)
        {
            _rabbitMqListener = rabbitMqListener;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if(_rabbitMqListener.IsConnected)
            {
                return Task.FromResult(HealthCheckResult.Healthy($"RabbitMq is connected. Listener ID: {_rabbitMqListener.ListenerId}"));
            }
            else
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("RabbitMq is not connected"));
            }

        }
    }
}

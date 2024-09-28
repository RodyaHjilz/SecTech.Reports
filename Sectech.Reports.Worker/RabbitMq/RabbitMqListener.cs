
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SecTech.Reports.Domain.Interfaces.Services;
using System.Text;

namespace Sectech.Reports.Worker.RabbitMq
{
    public class RabbitMqListener : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;
        private readonly ILogger<RabbitMqListener> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public RabbitMqListener(ILogger<RabbitMqListener> logger, IServiceScopeFactory serviceScopeFactory)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _queueName = "reports_queue";

            // Создаем очередь при инициализации
            _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // Логика сохранения сообщения в базу данных
                await SaveMessageToDatabaseAsync(message);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        private async Task SaveMessageToDatabaseAsync(string message)
        {
            // Логика сохранения в базу данных
            _logger.LogInformation($"Saving message to database: {message}");
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var workerService = scope.ServiceProvider.GetRequiredService<IWorkerService>();
                await workerService.SaveToDatabase(message);
            }
            // await _workerService.SaveToDatabase(message);
            // Здесь можно вызвать сервис для работы с базой данных
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }


    }
}

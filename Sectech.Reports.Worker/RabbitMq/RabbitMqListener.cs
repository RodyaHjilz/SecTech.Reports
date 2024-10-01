using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SecTech.Reports.Domain.Interfaces.Services;
using System.Text;

namespace Sectech.Reports.Worker.RabbitMq
{
    public class RabbitMqListener : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private const string _queueName = "reports_queue";
        private readonly ILogger<RabbitMqListener> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        public bool IsConnected => _connection != null && _connection.IsOpen && _channel != null && _channel.IsOpen;
        public Guid ListenerId = Guid.NewGuid();
        public RabbitMqListener(ILogger<RabbitMqListener> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            Task.Run(() => InitializeConnection(_cancellationTokenSource.Token)).Wait();
        }

        private async Task InitializeConnection(CancellationToken cancellationToken)
        {
            int retryCount = 0;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    _connection = factory.CreateConnection();
                    _channel = _connection.CreateModel();

                    _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    _connection.ConnectionShutdown += OnConnectionShutDown;
                    _logger.LogInformation("RabbitMqService initialized successful. Queuename: {queue}. Listener ID: {id}", _queueName, ListenerId);
                    retryCount = 0;
                    return;
                }
                catch(Exception ex)
                {
                    retryCount++;
                    var delay = TimeSpan.FromSeconds(Math.Min(30, Math.Pow(2, retryCount))); // Экспоненциальная задержка с максимумом 30 секунд

                    _logger.LogError(ex, "Failed to connect to RabbitMQ. Retrying in {delay} seconds...", delay.TotalSeconds);
                    await Task.Delay(delay, cancellationToken); // Задержка между попытками
                }
            }
        }


        private void OnConnectionShutDown(object sender, ShutdownEventArgs args)
        {
            _logger.LogWarning("RabbitMq connection was closed. Trying to reconnect...");
            Task.Run(() => InitializeConnection(_cancellationTokenSource.Token));
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
            _logger.LogInformation($"Saving message to database: {message}");
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var workerService = scope.ServiceProvider.GetRequiredService<IWorkerService>();
                await workerService.SaveToDatabase(message);
            }
        }

        public override void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _channel?.Close();
            _connection?.Close();
            _cancellationTokenSource.Dispose();
        }


    }
}

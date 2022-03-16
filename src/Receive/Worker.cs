using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Receive
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly RabbitMq _rabbitMq;

        public Worker(ILogger<Worker> logger, RabbitMq rabbitMq)
        {
            _logger = logger;
            _rabbitMq = rabbitMq;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Esperando 30 segundo para iniciar...");
            Thread.Sleep(TimeSpan.FromSeconds(30));

            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMq.Hostname,
                Password = _rabbitMq.UserName,
                UserName = _rabbitMq.Password,
                Port = _rabbitMq.Port
            };

            var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.BasicQos(0, 3, false); // False: Por worker (consumer) True: Por channel
            channel.QueueDeclare(queue: "hello",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine(" [x] Received {0}", message);
                Thread.Sleep(TimeSpan.FromSeconds(1));

                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queue: "hello",
                                 autoAck: false,
                                 consumer: consumer);

            await Task.CompletedTask;
        }
    }
}

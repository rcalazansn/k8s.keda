using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Send
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest"
            };

           
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                IBasicProperties props = channel.CreateBasicProperties();
                props.ContentType = "application/json";
                props.DeliveryMode = 2;
                props.Headers = new Dictionary<string, object>()
                {
                    { "Data (UTC)", DateTime.UtcNow.ToString("u") },
                    { "Data (BR)", DateTime.UtcNow.ToLocalTime().ToString("R") },
                    { "Host", Environment.MachineName },
                    { "SO", Environment.OSVersion.ToString() },
                };


                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                while (!stoppingToken.IsCancellationRequested)
                {
                    channel.BasicPublish(exchange: "",
                                         routingKey: "hello",
                                         basicProperties: props,
                                         body: body);
                    Console.WriteLine(" [x] Sent {0} - {1}", message, DateTimeOffset.Now);

                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }
}

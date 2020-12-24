using Microsoft.Extensions.Hosting;
using Net.RabbitMQ.Models.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Client
{
    public class Consumer : BackgroundService
    {
        private readonly IConsumer _consumer;
        private readonly ILogger<Consumer> _logger;
        public Consumer(IConsumer consumer,ILogger<Consumer> logger)
        {
            _consumer = consumer;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe<Test>(MessageProccessor);
            return Task.CompletedTask;
        }
        
        private Task MessageProccessor(Test test)
        {
            Console.WriteLine($"Id : {test.Id} Name : {test.Name}");
            return Task.CompletedTask;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"consuming at {DateTime.Now}");
            return Task.CompletedTask;
        }

    }
}

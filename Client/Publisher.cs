using Microsoft.Extensions.Hosting;
using Net.RabbitMQ.Models.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public class Publisher : BackgroundService
    {
        private readonly IProducer _producer;
        public Publisher(IProducer producer)
        {
            _producer = producer;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var test = new Test
            {
                Id = 1,
                Name = "test"
            };
            for (int i = 0; i < 5; i++)
            {
                await _producer.PublishAsync(test);
            }
        }
    }
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

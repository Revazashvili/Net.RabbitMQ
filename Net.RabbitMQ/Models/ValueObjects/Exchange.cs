using Net.RabbitMQ.Models.Primitives;

namespace Net.RabbitMQ.Models.ValueObjects
{
    public class Exchange : IExchange
    {
        public string Name { get; set; }
        public ExchangeType Type { get; set; } = ExchangeType.Direct;
        public bool Durable { get; set; } = true;
        public bool AutoDelete { get; set; } = false;
    }
}
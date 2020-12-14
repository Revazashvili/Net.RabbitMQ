using Net.RabbitMQ.Models.Primitives;

namespace Net.RabbitMQ.Models.ValueObjects
{
    public class Exchange
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
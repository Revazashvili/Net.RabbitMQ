using Net.RabbitMQ.Models.Primitives;

namespace Net.RabbitMQ.Models.ValueObjects
{
    public class RabbitMQConfiguration : IExchangeProperty<Exchange>,IQueueProperty<Queue>
    {
        public RabbitMqConnectionConfig RabbitMqConnection { get; set; }
        public Exchange Exchange { get; set; }
        public Queue Queue { get; set; }
        public uint PrefetchSize { get; set; }
        public  ushort  PrefetchCount { get; set; }
    }
}

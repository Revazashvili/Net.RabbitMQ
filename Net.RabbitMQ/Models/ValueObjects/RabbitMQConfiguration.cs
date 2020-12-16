using Net.RabbitMQ.Models.Primitives;

namespace Net.RabbitMQ.Models.ValueObjects
{
    public class RabbitMQConfiguration : IExchangeProperty<Exchange>,IQueueProperty<Queue>,IDLQueueProperty<Queue>,IDLExchangeProperty<Exchange>
    {
        public RabbitMqConnectionConfig RabbitMqConnection { get; set; }
        public Exchange Exchange { get; set; }
        public Queue Queue { get; set; }
        public Queue DLQueue { get; set; }
        public Exchange DLExchange { get; set; }
        public string Routing { get; set; }
        public uint PrefetchSize { get; set; }
        public  ushort  PrefetchCount { get; set; }
    }
}

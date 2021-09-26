using Net.RabbitMQ.Models.Primitives;

namespace Net.RabbitMQ.Models.ValueObjects
{
    public class RabbitMQConfiguration : IExchangeProperty,IQueueProperty,IDlQueueProperty,IDlExchangeProperty
    {
        public RabbitMQConfiguration()
        {
        }

        public RabbitMQConfiguration(RabbitMqConnectionConfig rabbitMqConnection, Exchange exchange, Queue queue, 
            Queue dlQueue, Exchange dlExchange, string routing, uint prefetchSize, ushort prefetchCount)
        {
            RabbitMqConnection = rabbitMqConnection;
            Exchange = exchange;
            Queue = queue;
            DlQueue = dlQueue;
            DlExchange = dlExchange;
            Routing = routing;
            PrefetchSize = prefetchSize;
            PrefetchCount = prefetchCount;
        }

        public RabbitMqConnectionConfig RabbitMqConnection { get; set; }
        public Exchange Exchange { get; set; }
        public Queue Queue { get; set; }
        public Queue DlQueue { get; set; }
        public Exchange DlExchange { get; set; }
        public string Routing { get; set; }
        public uint PrefetchSize { get; set; }
        public ushort PrefetchCount { get; set; }
    }
}

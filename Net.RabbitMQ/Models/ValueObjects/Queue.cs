using System.Collections.Generic;
using Net.RabbitMQ.Models.Primitives;

namespace Net.RabbitMQ.Models.ValueObjects
{
    public class Queue : IQueue
    {
        public string Name { get; set; }
        public string Routing { get; set; }
        public bool Durable { get; set; }
        public bool AutoDelete { get; set; }
        public bool Exclusive { get; set; }
        public Dictionary<string, object> Arguments { get; set; }
    }
}

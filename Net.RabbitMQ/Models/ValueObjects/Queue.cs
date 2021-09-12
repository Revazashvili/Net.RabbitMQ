using System.Collections.Generic;
using Net.RabbitMQ.Models.Primitives;

namespace Net.RabbitMQ.Models.ValueObjects
{
    public class Queue : IQueue
    {
        public string Name { get; set; }
        public bool AutoDelete { get; set; } = false;
        public bool Exclusive { get; set; } = true;
        public bool Durable { get; set; } = true;
    }
}

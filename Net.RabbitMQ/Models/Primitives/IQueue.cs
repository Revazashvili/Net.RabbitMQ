using System.Collections.Generic;

namespace Net.RabbitMQ.Models.Primitives
{
    public interface IQueue
    {
        string Name { get; set; }
        bool Durable { get; set; }
        bool AutoDelete { get; set; }
        bool Exclusive { get; set; }
        Dictionary<string,object> Arguments { get; set; }
        
    }
}

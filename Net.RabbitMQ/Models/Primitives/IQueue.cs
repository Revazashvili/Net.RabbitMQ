using System.Collections.Generic;

namespace Net.RabbitMQ.Models.Primitives
{
    public interface IQueue : IDurable,IAutoDelete
    {
        string Name { get; set; }
        bool Exclusive { get; set; }        
    }
}

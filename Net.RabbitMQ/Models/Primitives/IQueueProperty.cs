using Net.RabbitMQ.Models.ValueObjects;

namespace Net.RabbitMQ.Models.Primitives
{
    public interface IQueueProperty
    {
        Queue Queue { get; set; }
    }
}
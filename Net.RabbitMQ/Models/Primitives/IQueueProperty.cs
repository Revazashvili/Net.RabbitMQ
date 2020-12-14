namespace Net.RabbitMQ.Models.Primitives
{
    public interface IQueueProperty<T>
    {
        T Queue { get; set; }
    }
}
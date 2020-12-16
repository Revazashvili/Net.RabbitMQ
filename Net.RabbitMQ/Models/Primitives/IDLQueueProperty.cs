namespace Net.RabbitMQ.Models.Primitives
{
    public interface IDLQueueProperty<T>
    {
        T DLQueue { get; set; }
    }
}

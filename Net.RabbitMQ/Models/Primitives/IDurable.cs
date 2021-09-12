namespace Net.RabbitMQ.Models.Primitives
{
    public interface IDurable
    {
        bool Durable { get; set; }
    }
}
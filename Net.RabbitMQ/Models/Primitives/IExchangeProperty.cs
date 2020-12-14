namespace Net.RabbitMQ.Models.Primitives
{
    public interface IExchangeProperty<T>
    {
        T Exchange { get; set; }
    }
}

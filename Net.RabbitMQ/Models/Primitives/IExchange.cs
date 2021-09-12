namespace Net.RabbitMQ.Models.Primitives
{
    public interface IExchange : IDurable,IAutoDelete
    {
        string Name { get; set; }
        ExchangeType Type { get; set; }
    }
}
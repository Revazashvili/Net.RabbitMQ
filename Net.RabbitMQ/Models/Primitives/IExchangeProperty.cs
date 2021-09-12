using Net.RabbitMQ.Models.ValueObjects;

namespace Net.RabbitMQ.Models.Primitives
{
    public interface IExchangeProperty
    {
        Exchange Exchange { get; set; }
    }
}

using Net.RabbitMQ.Models.ValueObjects;

namespace Net.RabbitMQ.Models.Primitives
{
    public interface IDlExchangeProperty
    {
        Exchange DlExchange { get; set; }
    }
}

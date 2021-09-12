namespace Net.RabbitMQ.Models.Primitives
{
    public enum ExchangeType
    {
        Direct = 0,
        FanOut,
        Headers,
        Topic
    }
}
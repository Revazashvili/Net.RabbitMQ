using System;

namespace Net.RabbitMQ.Models.Exceptions
{
    public class ConsumedMessageNullException : Exception
    {
        public ConsumedMessageNullException() : base("Consumed message is null or empty.") { }
    }
}
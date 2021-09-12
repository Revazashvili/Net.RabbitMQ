using System;

namespace Net.RabbitMQ.Models.Exceptions
{
    public class CallbackFunctionNullException : Exception
    {
        public CallbackFunctionNullException() : base("Callback function is null.") { }
    }
}
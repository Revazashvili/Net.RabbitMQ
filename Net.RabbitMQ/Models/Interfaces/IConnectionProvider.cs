using RabbitMQ.Client;
using System;

namespace Net.RabbitMQ.Models.Interfaces
{
    public interface IConnectionProvider : IDisposable
    {
        IConnection GetConnection();
    }
}

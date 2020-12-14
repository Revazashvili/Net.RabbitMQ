using Net.RabbitMQ.Models.Interfaces;
using RabbitMQ.Client;
using System;
using Net.RabbitMQ.Models.ValueObjects;

namespace Net.RabbitMQ.Models.Entities
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly IConnectionFactory ConnectionFactory;
        private readonly IConnection Connection;
        private bool _disposed;
        public ConnectionProvider(RabbitMQConfiguration configuration)
        {
            ConnectionFactory = new ConnectionFactory
            {
                HostName = configuration.RabbitMqConnection.HostName,
                VirtualHost = configuration.RabbitMqConnection.VirtualHost,
                Port = (int)configuration.RabbitMqConnection.Port,
                UserName = configuration.RabbitMqConnection.UserName,
                Password = configuration.RabbitMqConnection.Password
            };
            Connection = ConnectionFactory.CreateConnection();
        }

        public IConnection GetConnection()
        {
            return Connection;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
                Connection?.Close();
            _disposed = true;
        }
    }
}

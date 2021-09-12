using Net.RabbitMQ.Models.Interfaces;
using RabbitMQ.Client;
using System;
using Net.RabbitMQ.Models.ValueObjects;

namespace Net.RabbitMQ.Models.Entities
{
    /// <inheritdoc />
    public sealed class ConnectionProvider : IConnectionProvider
    {
        private readonly IConnection _connection;
        private bool _disposed;
        /// <summary>
        /// Creates new instance of <see cref="ConnectionProvider"/> and
        /// <seealso cref="IConnection"/> in constructor,
        /// which will be return on <seealso cref="Connection"/> property.
        /// </summary>
        /// <param name="configuration">The <see cref="RabbitMqConfiguration"/> instance.</param>
        public ConnectionProvider(RabbitMqConfiguration configuration)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = configuration.RabbitMqConnection.HostName,
                VirtualHost = configuration.RabbitMqConnection.VirtualHost,
                Port = (int)configuration.RabbitMqConnection.Port,
                UserName = configuration.RabbitMqConnection.UserName,
                Password = configuration.RabbitMqConnection.Password
            };
            _connection = connectionFactory.CreateConnection();
        }

        public IConnection Connection => _connection;
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
                _connection?.Close();
            _disposed = true;
        }
    }
}

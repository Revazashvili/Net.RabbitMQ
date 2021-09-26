using Net.RabbitMQ.Models.Interfaces;
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;
using Net.RabbitMQ.Models.ValueObjects;
using RabbitMQ.Client.Exceptions;

namespace Net.RabbitMQ.Models.Entities
{
    /// <inheritdoc />
    public sealed class ConnectionProvider : IConnectionProvider
    {
        private readonly RabbitMQConfiguration _configuration;

        /// <summary>
        /// Creates new instance of <see cref="ConnectionProvider"/> and
        /// <seealso cref="IConnection"/> in constructor,
        /// which will be return on <seealso cref="Connection"/> property.
        /// </summary>
        /// <param name="configuration">The <see cref="RabbitMQConfiguration"/> instance.</param>
        public ConnectionProvider(RabbitMQConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConnection Connection()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = _configuration.RabbitMqConnection.HostName,
                VirtualHost = _configuration.RabbitMqConnection.VirtualHost,
                Port = (int)_configuration.RabbitMqConnection.Port,
                UserName = _configuration.RabbitMqConnection.UserName,
                Password = _configuration.RabbitMqConnection.Password,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };
            IConnection connection;
            try
            {
                connection = connectionFactory.CreateConnection();
            }
            catch(BrokerUnreachableException)
            {
                Task.Delay(5000);
                connection = connectionFactory.CreateConnection();
            }

            return connection;
        }
    }
}

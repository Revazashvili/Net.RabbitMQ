using RabbitMQ.Client;

namespace Net.RabbitMQ.Models.Interfaces
{
    /// <summary>
    /// Represents a type used to build RabbitMq connection.
    /// </summary>
    public interface IConnectionProvider
    {
        /// <summary>
        /// Returns RabbitMq connection.
        /// </summary>
        /// <returns>An <see cref="IConnection"/> instance.</returns>
        IConnection Connection();
    }
}

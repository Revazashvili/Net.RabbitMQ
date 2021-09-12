using RabbitMQ.Client;
using System;

namespace Net.RabbitMQ.Models.Interfaces
{
    /// <summary>
    /// Represents a type used to build RabbitMq connection.
    /// </summary>
    public interface IConnectionProvider : IDisposable
    {
        /// <summary>
        /// Returns RabbitMq connection.
        /// </summary>
        /// <returns>An <see cref="IConnection"/> instance.</returns>
        IConnection Connection { get; }
    }
}

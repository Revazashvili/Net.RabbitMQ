using System;
using System.Threading.Tasks;

namespace Net.RabbitMQ.Models.Interfaces
{
    /// <summary>
    /// Represents a type used to publish messages to queue.
    /// </summary>
    public interface IProducer : IDisposable
    {
        /// <summary>
        /// Synchronously sends message to queue.
        /// </summary>
        /// <param name="message">The message to publish in queue.</param>
        /// <typeparam name="T">Any object.</typeparam>
        void Publish<T>(T message);
        /// <summary>
        /// Asynchronously sends message to queue.
        /// </summary>
        /// <param name="message">The message to publish in queue.</param>
        /// <typeparam name="T">Any object.</typeparam>
        Task PublishAsync<T>(T message);
    }
}
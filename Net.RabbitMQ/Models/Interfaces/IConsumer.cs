using System;
using System.Threading.Tasks;

namespace Net.RabbitMQ.Models.Interfaces
{
    /// <summary>
    /// Represents a type used to consume messages from queue.
    /// </summary>
    public interface IConsumer : IDisposable
    {
        /// <summary>
        /// Synchronously consumes messages from queue.
        /// </summary>
        /// <param name="callback">The delegate to process consumed message.</param>
        /// <typeparam name="T">Any object.</typeparam>
        void Subscribe<T>(Func<T, Task> callback);
        
        /// <summary>
        /// Asynchronously consumes messages from queue.
        /// </summary>
        /// <param name="callback">The delegate to process consumed message.</param>
        /// <typeparam name="T">Any object.</typeparam>
        Task SubscribeAsync<T>(Func<T, Task> callback);
        
        /// <summary>
        /// Synchronously unSubscribes from queue.
        /// </summary>
        void UnSubscribe();
        
        /// <summary>
        /// Asynchronously unSubscribes from queue.
        /// </summary>
        Task UnSubscribeAsync();
    }
}
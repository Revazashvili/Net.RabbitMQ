using System;
using System.Threading.Tasks;

namespace Net.RabbitMQ.Models.Interfaces
{
    public interface IConsumer : IDisposable
    {
        void Subscribe<T>(Func<T, Task> callback);
        Task SubscribeAsync<T>(Func<T, Task> callback);
        void UnSubscribe();
    }
}
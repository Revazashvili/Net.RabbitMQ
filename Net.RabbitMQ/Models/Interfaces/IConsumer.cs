using System;
using System.Threading.Tasks;

namespace Net.RabbitMQ.Models.Interfaces
{
    public interface IConsumer : IDisposable
    {
        void Subscribe();
        void Subscribe<T>(Func<T, Task> callback);
    }
}
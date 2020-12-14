using System;
using System.Threading.Tasks;

namespace Net.RabbitMQ.Models.Interfaces
{
    public interface IProducer : IDisposable
    {
        void Publish<T>(T Message);
        Task PublishAsync<T>(T message);
    }
}
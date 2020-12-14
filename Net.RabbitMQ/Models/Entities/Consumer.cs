using Net.RabbitMQ.Models.Interfaces;
using Net.RabbitMQ.Models.ValueObjects;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Net.RabbitMQ.Models.Entities
{
    public class Consumer : IConsumer
    {

        private bool _disposed;
        private readonly IConnectionProvider Connection;
        private readonly IModel _model;
        private readonly RabbitMQConfiguration _config;
        public Consumer(IConnectionProvider connection, RabbitMQConfiguration config)
        {
            Connection = connection;
            _config = config;
            _model = Connection.GetConnection().CreateModel();
            _model.ExchangeDeclare(_config.Exchange.Name, _config.Exchange.Type, _config.Queue.Durable, _config.Queue.AutoDelete, _config.Queue.Arguments);
            _model.QueueDeclare(_config.Queue.Name, _config.Queue.Durable, _config.Queue.Exclusive, _config.Queue.AutoDelete, _config.Queue.Arguments);
            _model.QueueBind(_config.Queue.Name, _config.Exchange.Name, _config.Queue.Routing, _config.Queue.Arguments);
            _model.BasicQos(_config.PrefetchSize, _config.PrefetchCount, false);
        }

        public void Subscribe()
        {
            var _consumer = new EventingBasicConsumer(_model);
            _consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(body));
                Console.WriteLine(message);
            };
            _model.BasicConsume(_config.Queue.Name, autoAck: true, _consumer);
        }

        public void Subscribe<T>(Func<T, Task> callback)
        {
            var _consumer = new EventingBasicConsumer(_model);
            _consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(body));
                var success = callback.Invoke(message);
                if (success.IsCompleted)
                {
                    _model.BasicAck(e.DeliveryTag, true);
                }
                else
                {
                    _model.BasicPublish(_config.Exchange.Name, _config.Queue.Routing + "_error", basicProperties: null, body: body);
                }
            };
            _model.BasicConsume(_config.Queue.Name, autoAck: true, _consumer);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _model?.Close();

            _disposed = true;
        }

    }
}

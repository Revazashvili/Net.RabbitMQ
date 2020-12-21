using Net.RabbitMQ.Models.Interfaces;
using Net.RabbitMQ.Models.ValueObjects;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

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

            //Dead Letter Queue and Exchange
            _model.ExchangeDeclare(_config.DLExchange.Name, _config.DLExchange.Type,_config.Queue.Durable,_config.Queue.AutoDelete);
            _model.QueueDeclare(_config.DLQueue.Name,_config.DLQueue.Durable, _config.DLQueue.Exclusive, _config.DLQueue.AutoDelete);

            //arguments for dlexchange
            var arguments = new Dictionary<string, object>()
            {
                {"x-dead-letter-exchange",_config.DLExchange.Name}
            };

            //Working Queue and Exchange
            _model.ExchangeDeclare(_config.Exchange.Name, _config.Exchange.Type,_config.Queue.Durable, _config.Queue.AutoDelete);
            _model.QueueDeclare(_config.Queue.Name, _config.Queue.Durable, _config.Queue.Exclusive, _config.Queue.AutoDelete,arguments);
            _model.QueueBind(_config.Queue.Name, _config.Exchange.Name, _config.Routing,arguments);
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
                try
                {
                    var body = e.Body.ToArray();
                    var message = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(body));
                    var success = callback.Invoke(message);
                    if (success.IsCompleted)
                    {
                        _model.BasicAck(e.DeliveryTag, false);
                    }
                    else
                    {
                        _model.BasicNack(e.DeliveryTag, false, false);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _model.BasicNack(e.DeliveryTag, false, false);
                }
            };
            _model.BasicConsume(_config.Queue.Name, autoAck: false, _consumer);
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

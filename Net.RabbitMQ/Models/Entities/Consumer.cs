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
        private readonly IConnectionProvider _connection;
        private readonly IModel _model;
        private readonly EventingBasicConsumer _consumer;
        private readonly RabbitMQConfiguration _config;
        private string _consumerTag = String.Empty;
        public Consumer(IConnectionProvider connection, RabbitMQConfiguration config)
        {
            _connection = connection;
            _config = config;
            _model = _connection.GetConnection().CreateModel();
            DeclareDlExchangeAndDlQueue();
            DeclareExchangeAndQueue();
            _consumer = new EventingBasicConsumer(_model);
        }
        public void Subscribe<T>(Func<T, Task> callback)
        {
            _consumer.Received += (sender, e) =>
            {
                try
                {
                    var body = e.Body.ToArray();
                    var message = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(body));
                    var result = callback.Invoke(message);
                    if (result.IsCompleted)
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
            _consumerTag  = _model.BasicConsume(_config.Queue.Name, autoAck: false, _consumer);
        }
        public async Task SubscribeAsync<T>(Func<T, Task> callback)
        {
            await Task.Run(() =>
            {
                _consumer.Received += (sender,e)=>
                {
                    try
                    {
                        var body = e.Body.ToArray();
                        var message = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(body));
                        var result = callback.Invoke(message);
                        if (result.IsCompleted)
                        {
                            _model.BasicAck(e.DeliveryTag,false);
                        }
                        else
                        {
                            _model.BasicNack(e.DeliveryTag,false,false);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        _model.BasicNack(e.DeliveryTag,false,false);
                    }
                };
                _consumerTag  = _model.BasicConsume(_config.Queue.Name, autoAck: false, _consumer);
            });
        }
        public void UnSubscribe()
        {
            if (_consumerTag != null)
            {
                _model.BasicCancel(_consumerTag);
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _model?.Close();

            _disposed = true;
        }

        #region Private Methods
        private void FixDlNames()
        {
            if (_config.DLExchange != null && (_config.DLExchange?.Name == String.Empty || _config.DLExchange.Name is null))
                _config.DLExchange.Name = $"dl{_config.Exchange.Name}";
            if (_config.DLQueue != null && (_config.DLQueue?.Name == String.Empty || _config.DLQueue.Name is null))
                _config.DLQueue.Name = $"dl{_config.Queue.Name}";
        }
        private void DeclareDlExchangeAndDlQueue()
        {
            FixDlNames();
            _model.ExchangeDeclare(_config.DLExchange.Name,
                _config.DLExchange.Type == String.Empty ? _config.Exchange.Type : _config.DLExchange.Type,
                _config.Queue.Durable,
                _config.Queue.AutoDelete
            );
            _model.QueueDeclare(_config.DLQueue.Name,
                _config.DLQueue.Durable,
                _config.DLQueue.Exclusive,
                _config.DLQueue.AutoDelete
            );
            _model.QueueBind(_config.DLQueue.Name, _config.DLExchange.Name, _config.Routing,null);
            _model.BasicQos(_config.PrefetchSize, _config.PrefetchCount, false);
        }
        private void DeclareExchangeAndQueue()
        {
            var arguments = new Dictionary<string, object>()
            {
                {"x-dead-letter-exchange",_config.DLExchange.Name},
                {"x-dead-letter-queue",_config.DLQueue.Name}
            };
            
            _model.ExchangeDeclare(_config.Exchange.Name, _config.Exchange.Type,_config.Queue.Durable, _config.Queue.AutoDelete);
            _model.QueueDeclare(_config.Queue.Name, _config.Queue.Durable, _config.Queue.Exclusive, _config.Queue.AutoDelete,arguments);
            _model.QueueBind(_config.Queue.Name, _config.Exchange.Name, _config.Routing,arguments);
            _model.BasicQos(_config.PrefetchSize, _config.PrefetchCount, false);
        }
        #endregion

    }
}

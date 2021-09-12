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
    public class Subscriber : ISubscriber
    {
        private bool _disposed;
        private readonly IConnectionProvider _connection;
        private readonly IModel _model;
        private readonly EventingBasicConsumer _consumer;
        private readonly RabbitMqConfiguration _config;
        private string _consumerTag = String.Empty;
        public Subscriber(IConnectionProvider connection, RabbitMqConfiguration config)
        {
            _connection = connection;
            _config = config;
            _model = _connection.Connection.CreateModel();
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
                            _model.BasicAck(e.DeliveryTag,false);
                        _model.BasicNack(e.DeliveryTag,false,false);
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

        public Task UnSubscribeAsync()
        {
            throw new NotImplementedException();
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
            if (_config.DlExchange != null && (_config.DlExchange?.Name == String.Empty || _config.DlExchange.Name is null))
                _config.DlExchange.Name = $"dl{_config.Exchange.Name}";
            if (_config.DlQueue != null && (_config.DlQueue?.Name == String.Empty || _config.DlQueue.Name is null))
                _config.DlQueue.Name = $"dl{_config.Queue.Name}";
        }
        private void DeclareDlExchangeAndDlQueue()
        {
            FixDlNames();
            _model.ExchangeDeclare(_config.DlExchange.Name,
                (_config.DlExchange.Type.ToString() == String.Empty ? _config.Exchange.Type : _config.DlExchange.Type).ToString(),
                _config.Queue.Durable,
                _config.Queue.AutoDelete
            );
            _model.QueueDeclare(_config.DlQueue.Name,
                _config.DlQueue.Durable,
                _config.DlQueue.Exclusive,
                _config.DlQueue.AutoDelete
            );
            _model.QueueBind(_config.DlQueue.Name, _config.DlExchange.Name, _config.Routing,null);
            _model.BasicQos(_config.PrefetchSize, _config.PrefetchCount, false);
        }
        private void DeclareExchangeAndQueue()
        {
            var arguments = new Dictionary<string, object>()
            {
                {"x-dead-letter-exchange",_config.DlExchange.Name},
                {"x-dead-letter-queue",_config.DlQueue.Name}
            };
            
            _model.ExchangeDeclare(_config.Exchange.Name, _config.Exchange.Type.ToString(),_config.Queue.Durable, _config.Queue.AutoDelete);
            _model.QueueDeclare(_config.Queue.Name, _config.Queue.Durable, _config.Queue.Exclusive, _config.Queue.AutoDelete,arguments);
            _model.QueueBind(_config.Queue.Name, _config.Exchange.Name, _config.Routing,arguments);
            _model.BasicQos(_config.PrefetchSize, _config.PrefetchCount, false);
        }
        #endregion

    }
}

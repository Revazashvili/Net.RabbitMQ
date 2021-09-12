using Net.RabbitMQ.Models.Interfaces;
using Net.RabbitMQ.Models.ValueObjects;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Net.RabbitMQ.Models.Entities
{
    /// <inheritdoc />
    public class Producer : IProducer
    {
        private readonly IModel _model;
        private readonly RabbitMqConfiguration _config;
        private bool _disposed;

        public Producer(IConnectionProvider connectionProvider, RabbitMqConfiguration config)
        {
            _model = connectionProvider.Connection().CreateModel() ?? throw new Exception(nameof(IConnectionProvider));
            _config = config ?? throw new Exception(nameof(RabbitMqConfiguration));
            _model.ExchangeDeclare(_config.Exchange.Name, _config.Exchange.Type.ToString(), _config.Exchange.Durable,
                _config.Exchange.AutoDelete);
        }

        public void Publish<T>(T message)
        {
            if (message is null)
                throw new ArgumentNullException();
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            var basicProperties = _model.CreateBasicProperties();
            basicProperties.Persistent = true;
            basicProperties.DeliveryMode = 2;
            _model.BasicPublish(_config.Exchange.Name, _config.Routing, basicProperties, body);
        }
        public async Task PublishAsync<T>(T message) => await Task.Run(() => Publish(message));
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
    }
}

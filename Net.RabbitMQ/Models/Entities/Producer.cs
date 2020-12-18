using Net.RabbitMQ.Models.Interfaces;
using Net.RabbitMQ.Models.ValueObjects;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Net.RabbitMQ.Models.Entities
{
    public class Producer : IProducer
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly IModel _model;
        private readonly RabbitMQConfiguration _config;
        private bool _disposed;

        public Producer(IConnectionProvider connectionProvider, RabbitMQConfiguration config)
        {
            _connectionProvider = connectionProvider;
            _model = _connectionProvider.GetConnection().CreateModel();
            _config = config;
            _model.ExchangeDeclare(_config.Exchange.Name, _config.Exchange.Type.ToString(),true,false);
        }
        public void Publish<T>(T message)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            var basicProperties = _model.CreateBasicProperties();
            _model.BasicPublish(_config.Exchange.Name, _config.Routing, basicProperties, body: body);
        }

        public async Task PublishAsync<T>(T message)
        {
            await Task.Run(() =>
            {
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                var basicProperties = _model.CreateBasicProperties();
                _model.BasicPublish(_config.Exchange.Name, _config.Routing, basicProperties, body: body);
            });
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
    }
}

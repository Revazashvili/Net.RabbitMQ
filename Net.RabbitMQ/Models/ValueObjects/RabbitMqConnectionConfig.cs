using Net.RabbitMQ.Models.Common;
using Net.RabbitMQ.Models.Primitives;

namespace Net.RabbitMQ.Models.ValueObjects
{
    public class RabbitMqConnectionConfig : IHost,IUserInfo
    {
        public string HostName { get; set; }
        public string VirtualHost { get; set; }
        public uint Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}

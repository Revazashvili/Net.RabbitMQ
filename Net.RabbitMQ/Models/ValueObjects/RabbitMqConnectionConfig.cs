namespace Net.RabbitMQ.Models.ValueObjects
{
    public class RabbitMqConnectionConfig
    {
        public RabbitMqConnectionConfig()
        {
        }

        public RabbitMqConnectionConfig(string hostName, string virtualHost, uint port, string userName, string password)
        {
            HostName = hostName;
            VirtualHost = virtualHost;
            Port = port;
            UserName = userName;
            Password = password;
        }

        public string HostName { get; set; }
        public string VirtualHost { get; set; }
        public uint Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}

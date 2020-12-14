namespace Net.RabbitMQ.Models.Common
{
    public interface IHost
    {
        string HostName { get; set; }
        string VirtualHost { get; set; }
        uint Port { get; set; }
    }
}
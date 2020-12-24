using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Net.RabbitMQ.Extensions;
using Net.RabbitMQ.Models.ValueObjects;

namespace Client
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile(@"C:\Projects\NetCore\Net.RabbitMQ\Client\appsettings.json", true);
                    config.AddEnvironmentVariables();

                    if (args != null)
                        config.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    //var config = hostContext.Configuration.GetSection("RabbitMqConfiguraion").Get<RabbitMQConfiguration>();
                    var config = new RabbitMQConfiguration()
                    {
                        RabbitMqConnection = new RabbitMqConnectionConfig
                        {
                            HostName = "localhost",
                            VirtualHost = "/",
                            Port = 5672,
                            UserName = "guest",
                            Password = "guest"
                        },
                        Exchange = new Exchange
                        {
                            Name = "testexchange",
                            Type = "fanout"
                        },
                        DLExchange = new Exchange
                        {
                            Name = "dltestexchange",
                            Type = "fanout"
                        },
                        Queue = new Queue
                        {
                            Name = "testqueue",
                            Durable = true,
                            AutoDelete = false,
                            Exclusive = false
                        },
                        DLQueue = new Queue
                        {
                            Name = "dltestqueue",
                            Durable = true,
                            AutoDelete = false,
                            Exclusive = false
                        },
                        PrefetchCount = 10,
                        PrefetchSize = 0,
                        Routing = "test"
                    };
                    services.AddProducer(config);
                    services.AddSubscriber(config);
                    services.AddHostedService<Publisher>();
                    services.AddHostedService<Consumer>();
                });
            await builder.RunConsoleAsync();
        }
    }
}

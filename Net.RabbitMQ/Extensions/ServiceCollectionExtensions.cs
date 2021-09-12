using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Net.RabbitMQ.Models.Entities;
using Net.RabbitMQ.Models.Interfaces;
using Net.RabbitMQ.Models.ValueObjects;

namespace Net.RabbitMQ.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddProducer(this IServiceCollection services, RabbitMQConfiguration configuration)
        {
            services.AddSingleton<IConnectionProvider>(new ConnectionProvider(configuration));
            services.AddScoped<IProducer>(x => new Producer(x.GetService<IConnectionProvider>(), configuration));
            return services;
        }
        public static IServiceCollection AddSubscriber(this IServiceCollection services,
            RabbitMQConfiguration configuration)
        {
            services.AddSingleton<IConnectionProvider>(new ConnectionProvider(configuration));
            services.AddScoped<IConsumer>(x => new Consumer(x.GetService<IConnectionProvider>(), configuration));
            return services;
        }
        public static IServiceCollection AddRabbitMq(this IServiceCollection services,
            RabbitMQConfiguration configuration)
        {
            services.AddSingleton<IConnectionProvider>(new ConnectionProvider(configuration));
            services.AddScoped<IProducer>(x => new Producer(x.GetService<IConnectionProvider>(), configuration));
            services.AddScoped<IConsumer>(x => new Consumer(x.GetService<IConnectionProvider>(), configuration));
            return services;
        }

    }
}

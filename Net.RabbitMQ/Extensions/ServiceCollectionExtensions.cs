using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Net.RabbitMQ.Models.Entities;
using Net.RabbitMQ.Models.Interfaces;
using Net.RabbitMQ.Models.ValueObjects;

namespace Net.RabbitMQ.Extensions
{
    /// <summary>
    /// Extension class for Service Collection
    /// Adds Connection, IProducer and IConsumer to the services
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Adds IProducer to publish messages to exchange
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddProducer(this IServiceCollection services, RabbitMQConfiguration configuration)
        {
            services.AddSingleton<IConnectionProvider>(new ConnectionProvider(configuration));
            services.AddScoped<IProducer>(x => new Producer(x.GetService<IConnectionProvider>(), configuration));
            return services;
        }
        /// <summary>
        /// Adds IConsumer to consume messages from queue
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSubscriber(this IServiceCollection services,
            RabbitMQConfiguration configuration)
        {
            services.AddSingleton<IConnectionProvider>(new ConnectionProvider(configuration));
            services.AddScoped<IConsumer>(x => new Consumer(x.GetService<IConnectionProvider>(), configuration));
            return services;
        }

        /// <summary>
        /// Add IProducer and IConsumer for RabbitMq message broker 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
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

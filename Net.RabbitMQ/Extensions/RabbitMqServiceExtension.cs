using System;
using Microsoft.Extensions.DependencyInjection;
using Net.RabbitMQ.Models.Entities;
using Net.RabbitMQ.Models.Interfaces;
using Net.RabbitMQ.Models.ValueObjects;

namespace Net.RabbitMQ.Extensions
{
    /// <summary>
    /// Extension methods for the RabbitMq services.
    /// </summary>
    public static class RabbitMqServiceExtension
    {
        /// <summary>
        /// Adds <see cref="IProducer"/> service to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="configuration"><see cref="RabbitMqConfiguration"/> instance.</param>
        /// <param name="serviceLifetime"><see cref="ServiceLifetime"/> instance.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddProducer(this IServiceCollection services, 
            RabbitMqConfiguration configuration,ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            services.AddSingleton(configuration);
            services.AddConnectionProvider(serviceLifetime);
            services.AddProducer(serviceLifetime);
            return services;
        }
        
        /// <summary>
        /// Adds <see cref="IProducer"/> service to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="configureAction">An <see cref="Action{RabbitMQConfiguration}"/> to configure the provided <see cref="RabbitMqConfiguration"/>.</param>
        /// <param name="serviceLifetime"><see cref="ServiceLifetime"/> instance.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddProducer(this IServiceCollection services, 
            Action<RabbitMqConfiguration> configureAction,ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            var configuration = new RabbitMqConfiguration();
            configureAction?.Invoke(configuration);
            if (configuration is null) 
                throw new ArgumentNullException(nameof(configuration));
            services.AddProducer(configuration, serviceLifetime);
            return services;
        }
        
        
        /// <summary>
        /// Adds <see cref="ISubscriber"/> service to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="configuration"><see cref="RabbitMqConfiguration"/> instance.</param>
        /// <param name="serviceLifetime"><see cref="ServiceLifetime"/> instance.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddSubscriber(this IServiceCollection services, 
            RabbitMqConfiguration configuration,ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            services.AddSingleton(configuration);
            services.AddConnectionProvider(serviceLifetime);
            services.AddSubscriber(serviceLifetime);
            return services;
        }
        
        /// <summary>
        /// Adds <see cref="ISubscriber"/> service to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="configureAction">An <see cref="Action{RabbitMQConfiguration}"/> to configure the provided <see cref="RabbitMqConfiguration"/>.</param>
        /// <param name="serviceLifetime"><see cref="ServiceLifetime"/> instance.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddSubscriber(this IServiceCollection services, 
            Action<RabbitMqConfiguration> configureAction,ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            var configuration = new RabbitMqConfiguration();
            configureAction?.Invoke(configuration);
            if (configuration is null) 
                throw new ArgumentNullException(nameof(configuration));
            services.AddSubscriber(configuration, serviceLifetime);
            return services;
        }
        
        /// <summary>
        /// Adds RabbitMq services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="configuration"><see cref="RabbitMqConfiguration"/> instance.</param>
        /// <param name="serviceLifetime"><see cref="ServiceLifetime"/> instance.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddRabbitMq(this IServiceCollection services,
            RabbitMqConfiguration configuration,ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            services.AddSingleton(configuration);
            services.AddConnectionProvider(serviceLifetime);
            services.AddProducer(serviceLifetime);
            services.AddSubscriber(serviceLifetime);
            return services;
        }
        
        /// <summary>
        /// Adds RabbitMq services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="configureAction">An <see cref="Action{RabbitMQConfiguration}"/> to configure the provided <see cref="RabbitMqConfiguration"/>.</param>
        /// <param name="serviceLifetime"><see cref="ServiceLifetime"/> instance.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddRabbitMq(this IServiceCollection services,
            Action<RabbitMqConfiguration> configureAction,ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            var configuration = new RabbitMqConfiguration();
            configureAction?.Invoke(configuration);
            if (configuration is null) 
                throw new ArgumentNullException(nameof(configuration));
            services.AddRabbitMq(configuration, serviceLifetime);
            return services;
        }

        /// <summary>
        /// Injects <see cref="IConnectionProvider"/> service into DI Container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="serviceLifetime"><see cref="ServiceLifetime"/> instance.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        private static IServiceCollection AddConnectionProvider(this IServiceCollection services,
            ServiceLifetime serviceLifetime)
        {
            var connectionProviderServiceDescriptor =
                new ServiceDescriptor(typeof(IConnectionProvider), typeof(ConnectionProvider), serviceLifetime);
            services.Add(connectionProviderServiceDescriptor);
            return services;
        }

        /// <summary>
        /// Injects <see cref="IProducer"/> service into DI Container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="serviceLifetime"><see cref="ServiceLifetime"/> instance.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        private static IServiceCollection AddProducer(this IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            var producerServiceDescriptor = new ServiceDescriptor(typeof(IProducer), typeof(Producer), serviceLifetime);
            services.Add(producerServiceDescriptor);
            return services;
        }
        
        /// <summary>
        /// Injects <see cref="ISubscriber"/> service into DI Container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="serviceLifetime"><see cref="ServiceLifetime"/> instance.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        private static IServiceCollection AddSubscriber(this IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            var consumerServiceDescriptor = new ServiceDescriptor(typeof(ISubscriber), typeof(Subscriber), serviceLifetime);
            services.Add(consumerServiceDescriptor);
            return services;
        }
        
    }
}

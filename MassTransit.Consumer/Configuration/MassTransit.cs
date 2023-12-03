using MassTransit.Consumer.Consumers;
using MassTransit.Shared;

namespace MassTransit.Consumer.Configuration;

public static class MassTransit
{
    public static IServiceCollection ConfigureMassTransit(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        var massTransitConfiguration = configurationManager.GetSection("MassTransit").Get<MassTransitConfigurationModel>();

        if (massTransitConfiguration is null)
            throw new ApplicationException("Could not load MassTransit configuration");

        services.AddMassTransit(options =>
        {
            options.UsingRabbitMq((context, configuration) =>
            {
                configuration.Host(massTransitConfiguration.Server, massTransitConfiguration.VirtualHost, hostConfiguration =>
                {
                    hostConfiguration.Username(massTransitConfiguration.UserName);
                    hostConfiguration.Password(massTransitConfiguration.Password);
                });

                //NOTE: to have more than one queue, configure one of the below for each. If you don't configure like below, a queue will be created with the consumer class name
                configuration.ReceiveEndpoint(massTransitConfiguration.QueueName, endpointConfiguration =>
                {
                    endpointConfiguration.Consumer<OrderCreatedConsumer>();
                });
                
                configuration.ConfigureEndpoints(context);
            });

            options.AddConsumer<OrderCreatedConsumer>();
        });

        return services;
    }
}
using MassTransit.Consumer.Consumers;
using MassTransit.Shared;

namespace MassTransit.Consumer.Configuration;

public static class MassTransit
{
    public static IServiceCollection ConfigureMassTransit(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        var massTransitConfiguration = configurationManager.GetSection("MassTransitAzure").Get<MassTransitAzureConfigurationModel>();

        if (massTransitConfiguration is null)
            throw new ApplicationException("Could not load MassTransit configuration");

        services.AddMassTransit(options =>
        {
            options.UsingAzureServiceBus((context, configuration) =>
            {
                configuration.Host(massTransitConfiguration.Connection);

                //NOTE: to have more than one queue, configure one of the below for each. If you don't configure like below, a queue will be created with the consumer class name
                configuration.ReceiveEndpoint(massTransitConfiguration.QueueName, endpointConfiguration =>
                {
                    endpointConfiguration.Consumer<OrderCreatedConsumer>();
                });
                
                //configuration.ConfigureEndpoints(context);
            });

            //options.AddConsumer<OrderCreatedConsumer>();
        });

        return services;
    }
}
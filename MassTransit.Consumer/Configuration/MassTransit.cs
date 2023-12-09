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

                configuration.ReceiveEndpoint(massTransitConfiguration.QueueName, endpointConfiguration =>
                {
                    endpointConfiguration.Consumer<OrderCreatedConsumer>();
                    // endpointConfiguration.UseMessageRetry(retryConfiguration => //NOTE: see documentation for more options
                    // {
                    //     retryConfiguration.
                    // });
                });
                
                //NOTE: if you have a 'topic' queue in service bus:
                // configuration.SubscriptionEndpoint("sub-1", "topic", subscriptionEndpointConfiguration =>
                // {
                //     subscriptionEndpointConfiguration.Consumer<OrderCreatedConsumer>();
                // });
            });
        });

        return services;
    }
}
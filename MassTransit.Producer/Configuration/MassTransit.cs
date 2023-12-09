using MassTransit.Shared;

namespace MassTransit.Producer.Configuration;

public static class MassTransit
{
    public static IServiceCollection ConfigureMassTransit(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        var massTransitConfiguration = configurationManager.GetSection("MassTransitAzure").Get<MassTransitAzureConfigurationModel>();

        if (massTransitConfiguration is null)
            throw new ApplicationException("Could not load MassTransit configuration");

        services.AddSingleton(massTransitConfiguration);

        services.AddMassTransit(options =>
        {
            options.UsingAzureServiceBus((context, configuration) =>
            {
                configuration.Host(massTransitConfiguration.Connection);
                
                //NOTE: if you have a 'topic' queue in service bus:
                // configuration.Message<Order>(messageConfiguration =>
                // {
                //     messageConfiguration.SetEntityName("topic");
                // });
            });
        });

        return services;
    }
}
using MassTransit.Shared;

namespace MassTransit.Producer.Configuration;

public static class MassTransit
{
    public static IServiceCollection ConfigureMassTransit(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        var massTransitConfiguration = configurationManager.GetSection("MassTransit").Get<MassTransitConfigurationModel>();

        if (massTransitConfiguration is null)
            throw new ApplicationException("Could not load MassTransit configuration");

        services.AddSingleton(massTransitConfiguration);

        services.AddMassTransit(options =>
        {
            options.UsingRabbitMq((context, configuration) =>
            {
                configuration.Host(massTransitConfiguration.Server, massTransitConfiguration.VirtualHost, hostConfiguration =>
                {
                    hostConfiguration.Username(massTransitConfiguration?.UserName);
                    hostConfiguration.Password(massTransitConfiguration?.Password);
                });

                configuration.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
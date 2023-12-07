namespace MassTransit.Shared;

public record MassTransitAzureConfigurationModel
{
    public string Connection { get; set; } = string.Empty;
    public string QueueName { get; set; } = string.Empty;
}
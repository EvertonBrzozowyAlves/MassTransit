namespace MassTransit.Shared;

public record MassTransitConfigurationModel
{
    public string Server { get; set; } = string.Empty;
    public string VirtualHost { get; set; } = string.Empty;
    public string QueueName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
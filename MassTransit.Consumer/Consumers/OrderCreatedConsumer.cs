using MassTransit.Shared;

namespace MassTransit.Consumer.Consumers;

public class OrderCreatedConsumer : IConsumer<Order>
{
    public Task Consume(ConsumeContext<Order> context)
    {
        Console.WriteLine(context.Message);
        return Task.CompletedTask;
    }
}
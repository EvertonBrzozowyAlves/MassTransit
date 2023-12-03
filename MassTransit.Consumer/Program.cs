using MassTransit.Consumer;
using MassTransit.Consumer.Configuration;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .ConfigureMassTransit(builder.Configuration)
    .AddHostedService<Worker>();

var host = builder.Build();
host.Run();
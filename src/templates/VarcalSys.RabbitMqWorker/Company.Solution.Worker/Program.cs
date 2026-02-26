using Company.Solution.Worker.Configuration;
using Company.Solution.Worker.Consumers;
using Company.Solution.Worker.Handlers;
using Company.Solution.Worker.Infrastructure;
using Company.Solution.Worker.Workers;

var builder = Host.CreateApplicationBuilder(args);

var rabbitMqOptions = builder.Configuration
    .GetSection(RabbitMqOptions.SectionName)
    .Get<RabbitMqOptions>() ?? new RabbitMqOptions();

builder.Services.AddSingleton(rabbitMqOptions);
builder.Services.AddScoped<IMessageHandler, ExampleMessageHandler>();
builder.Services.AddScoped<IMessageConsumer, ExampleConsumer>();
builder.Services.AddSingleton<RabbitMqConnectionManager>();
builder.Services.AddHostedService<RabbitMqWorker>();

var host = builder.Build();
await host.RunAsync();

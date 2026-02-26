using Spectre.Console.Cli;
using VarcalSys.Cli.Commands;

var app = new CommandApp();

app.Configure(config =>
{
    config.SetApplicationName("varcal");
    config.SetApplicationVersion("1.0.0");

    config.AddCommand<NewCommand>("new")
        .WithDescription("Cria um novo projeto a partir de um template VarcalSys (wizard interativo).")
        .WithExample("new")
        .WithExample("new", "-t", "varcal-ddd", "-c", "Acme", "-s", "OrderService");

    config.AddCommand<ListCommand>("list")
        .WithDescription("Lista todos os templates disponíveis.");

    config.AddCommand<InfoCommand>("info")
        .WithDescription("Exibe detalhes de um template específico.")
        .WithExample("info", "varcal-ddd");
});

return await app.RunAsync(args);

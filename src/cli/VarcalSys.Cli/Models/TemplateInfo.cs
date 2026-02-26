namespace VarcalSys.Cli.Models;

public record TemplateInfo(
    string ShortName,
    string DisplayName,
    string Description,
    string[] Tags
);

public static class AvailableTemplates
{
    public static readonly IReadOnlyList<TemplateInfo> All =
    [
        new(
            "varcal-ddd",
            "DDD + Clean Architecture",
            "Solution com DDD, CQRS (MediatR), FluentValidation e EF Core. Ideal para domínios complexos.",
            ["DDD", "Clean Arch", "CQRS", "EF Core"]
        ),
        new(
            "varcal-clean",
            "Clean Architecture",
            "Solution com Clean Architecture, FluentValidation e EF Core. Sem conceitos complexos de DDD.",
            ["Clean Arch", "EF Core"]
        ),
        new(
            "varcal-minimal",
            "Minimal API",
            "Projeto simples de Minimal API com FluentValidation e Endpoints organizados.",
            ["Minimal API", "FluentValidation"]
        ),
        new(
            "varcal-rabbitmq",
            "Worker RabbitMQ",
            "Worker Service com consumidor RabbitMQ nativo, reconexão automática e manual ack.",
            ["Worker", "RabbitMQ", "Messaging"]
        ),
    ];
}

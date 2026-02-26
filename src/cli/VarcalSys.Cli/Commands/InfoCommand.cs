using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;
using VarcalSys.Cli.Models;

namespace VarcalSys.Cli.Commands;

public class InfoCommand : Command<InfoCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<template>")]
        [Description("Short name of the template (e.g. varcal-ddd)")]
        public string Template { get; set; } = string.Empty;
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var template = AvailableTemplates.All.FirstOrDefault(
            t => t.ShortName.Equals(settings.Template, StringComparison.OrdinalIgnoreCase));

        if (template is null)
        {
            AnsiConsole.MarkupLine($"[red]Template '{settings.Template}' not found.[/]");
            AnsiConsole.MarkupLine("[grey]Run 'varcal list' to see available templates.[/]");
            return 1;
        }

        AnsiConsole.WriteLine();
        var panel = new Panel(
            new Markup($"""
                [bold]{template.DisplayName}[/]

                [grey]{template.Description}[/]

                [yellow]Short Name:[/] {template.ShortName}
                [yellow]Tags:[/]       {string.Join(", ", template.Tags)}

                [yellow]Usage:[/]
                  varcal new -t {template.ShortName} -c MyCompany -s MyProject
                  dotnet new {template.ShortName} --Company MyCompany --Solution MyProject
                """))
        {
            Header = new PanelHeader($" [cyan]Template Info[/] "),
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(Color.Grey)
        };

        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
        return 0;
    }
}

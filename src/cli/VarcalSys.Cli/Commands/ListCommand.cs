using Spectre.Console;
using Spectre.Console.Cli;
using VarcalSys.Cli.Models;

namespace VarcalSys.Cli.Commands;

public class ListCommand : Command
{
    public override int Execute(CommandContext context)
    {
        AnsiConsole.MarkupLine("\n[bold]Available VarcalSys Templates[/]\n");

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn(new TableColumn("[yellow]Short Name[/]").LeftAligned())
            .AddColumn(new TableColumn("[cyan]Template[/]").LeftAligned())
            .AddColumn(new TableColumn("[grey]Description[/]").LeftAligned())
            .AddColumn(new TableColumn("[grey]Tags[/]").LeftAligned());

        foreach (var t in AvailableTemplates.All)
        {
            table.AddRow(
                $"[yellow]{t.ShortName}[/]",
                $"[cyan]{t.DisplayName}[/]",
                $"[grey]{t.Description}[/]",
                string.Join(", ", t.Tags.Select(tag => $"[dim]{tag}[/]"))
            );
        }

        AnsiConsole.Write(table);

        AnsiConsole.MarkupLine("\n[grey]Usage:[/]");
        AnsiConsole.MarkupLine("  [yellow]varcal new[/]                    [grey]→ wizard interativo[/]");
        AnsiConsole.MarkupLine("  [yellow]varcal new -t varcal-ddd[/]      [grey]→ template específico[/]");
        AnsiConsole.MarkupLine("  [yellow]varcal info varcal-ddd[/]        [grey]→ detalhes do template[/]");
        AnsiConsole.WriteLine();

        return 0;
    }
}

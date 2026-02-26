using System.ComponentModel;
using System.Diagnostics;
using Spectre.Console;
using Spectre.Console.Cli;
using VarcalSys.Cli.Models;

namespace VarcalSys.Cli.Commands;

public class NewCommand : AsyncCommand<NewCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-t|--template")]
        [Description("Short name of the template (e.g. varcal-ddd)")]
        public string? Template { get; set; }

        [CommandOption("-c|--company")]
        [Description("Company/organization name")]
        public string? Company { get; set; }

        [CommandOption("-s|--solution")]
        [Description("Solution/project name")]
        public string? Solution { get; set; }

        [CommandOption("-o|--output")]
        [Description("Output directory (defaults to current directory/solution-name)")]
        public string? Output { get; set; }

        [CommandOption("--no-git")]
        [Description("Skip git init after project creation")]
        public bool NoGit { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        AnsiConsole.Write(new FigletText("varcal").Color(Color.DeepSkyBlue1));
        AnsiConsole.MarkupLine("[grey]VarcalSys .NET Template CLI[/]\n");

        // Select template
        var template = settings.Template is not null
            ? AvailableTemplates.All.FirstOrDefault(t => t.ShortName == settings.Template)
            : PromptTemplate();

        if (template is null)
        {
            AnsiConsole.MarkupLine("[red]Template not found.[/]");
            return 1;
        }

        // Gather inputs
        var company = settings.Company
            ?? AnsiConsole.Ask<string>("[yellow]Company name[/] ([grey]e.g. Acme[/]):");

        var solution = settings.Solution
            ?? AnsiConsole.Ask<string>("[yellow]Solution/Project name[/] ([grey]e.g. OrderService[/]):");

        var outputDir = settings.Output
            ?? Path.Combine(Directory.GetCurrentDirectory(), solution);

        // Summary
        AnsiConsole.WriteLine();
        var table = new Table().BorderColor(Color.Grey);
        table.AddColumn("Setting").AddColumn("Value");
        table.AddRow("Template", $"[cyan]{template.DisplayName}[/] ([grey]{template.ShortName}[/])");
        table.AddRow("Company", $"[green]{company}[/]");
        table.AddRow("Solution", $"[green]{solution}[/]");
        table.AddRow("Output", $"[blue]{outputDir}[/]");
        table.AddRow("Git init", settings.NoGit ? "[grey]No[/]" : "[green]Yes[/]");
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();

        if (!AnsiConsole.Confirm("Create project?"))
        {
            AnsiConsole.MarkupLine("[grey]Cancelled.[/]");
            return 0;
        }

        // Run dotnet new
        return await AnsiConsole.Status()
            .StartAsync("Creating project...", async ctx =>
            {
                ctx.Spinner(Spinner.Known.Dots);

                var args = BuildDotnetArgs(template.ShortName, company, solution, outputDir);
                var exitCode = await RunCommandAsync("dotnet", args);

                if (exitCode != 0)
                {
                    AnsiConsole.MarkupLine("\n[red]Failed to create project. Make sure VarcalSys.Templates is installed:[/]");
                    AnsiConsole.MarkupLine("[grey]  dotnet new install VarcalSys.Templates[/]");
                    return exitCode;
                }

                ctx.Status("Restoring NuGet packages...");
                await RunCommandAsync("dotnet", $"restore \"{outputDir}\"");

                if (!settings.NoGit)
                {
                    ctx.Status("Initializing git repository...");
                    await RunCommandAsync("git", $"init \"{outputDir}\"");
                    await CreateGitignoreAsync(outputDir);
                }

                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine($"[green]✓[/] Project created at [blue]{outputDir}[/]");
                AnsiConsole.MarkupLine($"\n[grey]Next steps:[/]");
                AnsiConsole.MarkupLine($"  [yellow]cd[/] {solution}");
                AnsiConsole.MarkupLine($"  [yellow]dotnet build[/]");
                AnsiConsole.MarkupLine($"  [yellow]dotnet test[/]");

                return 0;
            });
    }

    private static TemplateInfo PromptTemplate()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<TemplateInfo>()
                .Title("Select a [cyan]template[/]:")
                .UseConverter(t =>
                    $"[cyan]{t.DisplayName}[/] [grey]({t.ShortName})[/]\n  [dim]{t.Description}[/]")
                .AddChoices(AvailableTemplates.All));
    }

    private static string BuildDotnetArgs(string shortName, string company, string solution, string outputDir)
    {
        return $"new {shortName} --Company \"{company}\" --Solution \"{solution}\" -o \"{outputDir}\" --force";
    }

    private static async Task<int> RunCommandAsync(string command, string arguments)
    {
        var psi = new ProcessStartInfo(command, arguments)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        using var process = Process.Start(psi);
        if (process is null) return -1;

        await process.WaitForExitAsync();
        return process.ExitCode;
    }

    private static async Task CreateGitignoreAsync(string outputDir)
    {
        var gitignorePath = Path.Combine(outputDir, ".gitignore");
        if (File.Exists(gitignorePath)) return;

        const string content = """
            bin/
            obj/
            *.user
            .vs/
            *.nupkg
            appsettings.*.local.json
            """;

        await File.WriteAllTextAsync(gitignorePath, content);
    }
}

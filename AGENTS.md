# Repository Guidelines

## Project Structure & Module Organization
This repository publishes reusable .NET templates and a companion CLI tool.
- `src/templates/`: template sources (`VarcalSys.DddCleanArch`, `VarcalSys.CleanArch`, `VarcalSys.MinimalApi`, `VarcalSys.RabbitMqWorker`).
- `src/cli/VarcalSys.Cli/`: `varcal` command-line tool.
- `nuget/VarcalSys.Templates.csproj`: NuGet packaging entry for templates.
- `nuget/VarcalSys.Templates.vstemplate/`: Visual Studio `.vstemplate` packaging files.
- `.github/workflows/publish.yml`: CI for build, pack, and NuGet publish on `v*` tags.

Keep changes scoped: update only the template/CLI package impacted by your task.

## Build, Test, and Development Commands
Use .NET 10 SDK.
- `dotnet build src/cli/VarcalSys.Cli/VarcalSys.Cli.csproj -c Release`: build CLI.
- `dotnet run --project src/cli/VarcalSys.Cli -- list`: quick CLI smoke test.
- `dotnet pack nuget/VarcalSys.Templates.csproj -c Release -o ./artifacts`: pack template NuGet.
- `dotnet pack src/cli/VarcalSys.Cli/VarcalSys.Cli.csproj -c Release -o ./artifacts`: pack CLI NuGet.
- `dotnet new install ./src/templates/VarcalSys.DddCleanArch/` (repeat per template): install local template for validation.
- `dotnet new varcal-ddd --Company Test --Solution Demo -o /tmp/test-demo && cd /tmp/test-demo && dotnet build && dotnet test`: end-to-end template verification.

## Coding Style & Naming Conventions
- Follow existing C# conventions: 4-space indentation, `PascalCase` for types/methods, `camelCase` for locals/parameters, interfaces prefixed with `I`.
- Keep namespaces and folder names aligned (for example `Handlers/`, `Validators/`, `Infrastructure/`).
- Nullable reference types are enabled; avoid introducing nullable warnings.
- Prefer small, cohesive classes and explicit business flow.

## Testing Guidelines
- Frameworks in templates: `xUnit`, `FluentAssertions`, and `NSubstitute` (where needed).
- Test names should describe behavior (examples: `CreateExampleCommandHandlerTests`, `ExampleAggregateTests`).
- Run generated solution tests after template changes; do not rely only on compile checks.

## Commit & Pull Request Guidelines
Current git history is minimal (`first commit`), so enforce a clear convention going forward:
- Commit format: `type(scope): short description` (for example `feat(cli): add template alias validation`).
- PRs must include: purpose, affected paths (template/CLI/NuGet), validation commands run, and sample output when CLI behavior changes.
- Link related issue/ticket when available and keep PR scope focused.

## Security & Configuration Tips
- Never commit secrets (for example `NUGET_API_KEY`); use GitHub Secrets.
- Keep sample `appsettings*.json` values non-sensitive.
- Validate any new template parameter to avoid unsafe generated defaults.

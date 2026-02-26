# VarçalSys .NET Templates

Templates de projeto .NET 10 para acelerar a criação de novos projetos com arquiteturas padronizadas.

## Templates Disponíveis

| Template | Short Name | Descrição |
|----------|-----------|-----------|
| DDD + Clean Architecture | `varcal-ddd` | DDD, CQRS (MediatR), FluentValidation, EF Core |
| Clean Architecture | `varcal-clean` | Clean Arch, FluentValidation, EF Core |
| Minimal API | `varcal-minimal` | Minimal API com Endpoints e FluentValidation |
| Worker RabbitMQ | `varcal-rabbitmq` | Worker com RabbitMQ.Client nativo |

## Instalação

### Via NuGet (recomendado)

```bash
dotnet new install VarcalSys.Templates
```

### Verificar instalação

```bash
dotnet new list --tag VarcalSys
```

### Via CLI `varcal`

```bash
dotnet tool install -g VarcalSys.Cli
```

---

## Uso via `dotnet new`

### DDD + Clean Architecture

```bash
dotnet new varcal-ddd \
  --Company Acme \
  --Solution OrderService \
  -o ./OrderService
```

**Estrutura gerada:**
```
OrderService/
├── src/
│   ├── Acme.OrderService.Domain/       ← Entities, ValueObjects, Events, Repositories
│   ├── Acme.OrderService.Application/  ← Commands, Queries, Handlers, Validators (MediatR)
│   ├── Acme.OrderService.Infrastructure/ ← EF Core, Repositories
│   └── Acme.OrderService.API/          ← Controllers, Middleware, Program.cs
└── tests/
    ├── Acme.OrderService.Domain.Tests/
    └── Acme.OrderService.Application.Tests/
```

### Clean Architecture (sem DDD)

```bash
dotnet new varcal-clean \
  --Company Acme \
  --Solution ProductService \
  -o ./ProductService
```

### Minimal API

```bash
dotnet new varcal-minimal \
  --Company Acme \
  --Solution CatalogApi \
  -o ./CatalogApi
```

### Worker RabbitMQ

```bash
dotnet new varcal-rabbitmq \
  --Company Acme \
  --Solution NotificationWorker \
  --QueueName notifications \
  -o ./NotificationWorker
```

---

## Uso via CLI `varcal`

```bash
# Wizard interativo
varcal new

# Com parâmetros
varcal new -t varcal-ddd -c Acme -s OrderService

# Listar templates
varcal list

# Detalhes de um template
varcal info varcal-ddd
```

---

## Parâmetros dos Templates

### Parâmetros comuns

| Parâmetro | Padrão | Descrição |
|-----------|--------|-----------|
| `--Company` | `MyCompany` | Nome da empresa/organização |
| `--Solution` | `MySolution` | Nome da solution/projeto |
| `--UseDocker` | `false` | Incluir Dockerfile (apenas DDD e Clean) |

### Parâmetros específicos — RabbitMQ Worker

| Parâmetro | Padrão | Descrição |
|-----------|--------|-----------|
| `--QueueName` | `my-queue` | Nome da fila RabbitMQ |

---

## Stack de Tecnologias

| Template | MediatR | FluentValidation | EF Core | RabbitMQ.Client |
|----------|:-------:|:---------------:|:-------:|:---------------:|
| varcal-ddd | ✅ | ✅ | ✅ SQL Server | — |
| varcal-clean | ✅ | ✅ | ✅ SQL Server | — |
| varcal-minimal | — | ✅ | — | — |
| varcal-rabbitmq | — | — | — | ✅ 7.x |

**Versão alvo:** .NET 10 (LTS)

---

## Integração com Visual Studio

Os arquivos `.vstemplate` estão em `nuget/VarcalSys.Templates.vstemplate/`.

Ver [nuget/VarcalSys.Templates.vstemplate/INSTALL.md](nuget/VarcalSys.Templates.vstemplate/INSTALL.md) para instruções de instalação manual no Visual Studio.

---

## Estrutura do Repositório

```
VSTemplates/
├── .github/workflows/publish.yml        ← CI/CD: publica NuGet em tag push
├── src/
│   ├── templates/
│   │   ├── VarcalSys.DddCleanArch/      ← Template 1
│   │   ├── VarcalSys.CleanArch/         ← Template 2
│   │   ├── VarcalSys.MinimalApi/        ← Template 3
│   │   └── VarcalSys.RabbitMqWorker/    ← Template 4
│   └── cli/VarcalSys.Cli/               ← CLI varcal (dotnet tool)
├── nuget/
│   ├── VarcalSys.Templates.csproj       ← Pacote NuGet de templates
│   └── VarcalSys.Templates.vstemplate/  ← Templates para Visual Studio
└── README.md
```

---

## Desenvolvimento

### Testar templates localmente

```bash
# Instalar templates do diretório local
dotnet new install ./src/templates/VarcalSys.DddCleanArch/
dotnet new install ./src/templates/VarcalSys.CleanArch/
dotnet new install ./src/templates/VarcalSys.MinimalApi/
dotnet new install ./src/templates/VarcalSys.RabbitMqWorker/

# Gerar projeto de teste
dotnet new varcal-ddd --Company Test --Solution Demo -o /tmp/test-demo

# Verificar build
cd /tmp/test-demo && dotnet build && dotnet test
```

### Testar CLI localmente

```bash
cd src/cli/VarcalSys.Cli
dotnet run -- new
dotnet run -- list
dotnet run -- info varcal-ddd
```

### Pack NuGet localmente

```bash
dotnet pack nuget/VarcalSys.Templates.csproj -o ./artifacts
dotnet pack src/cli/VarcalSys.Cli/VarcalSys.Cli.csproj -o ./artifacts

# Instalar e testar
dotnet new install ./artifacts/VarcalSys.Templates.1.0.0.nupkg
dotnet tool install -g VarcalSys.Cli --add-source ./artifacts
```

### Publicar nova versão

Criar e fazer push de uma tag no formato `v1.x.x`:

```bash
git tag v1.0.0
git push origin v1.0.0
```

O GitHub Actions irá automaticamente fazer o pack e push para NuGet.org.
Configure o secret `NUGET_API_KEY` nas configurações do repositório.

---

## Licença

MIT

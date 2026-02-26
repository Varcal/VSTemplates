# Instalação dos Templates no Visual Studio

## Via dotnet new (recomendado)
```bash
dotnet new install VarcalSys.Templates
```

## Instalação Manual no Visual Studio

Copie as pastas de template para:
- **Windows**: `%USERPROFILE%\Documents\Visual Studio 2022\Templates\ProjectTemplates\`
- **Mac**: `~/Library/Application Support/Microsoft Visual Studio 2022/`

Cada pasta de template deve ser zipada individualmente antes de copiar.

### Para zipar cada template (PowerShell):
```powershell
Compress-Archive -Path .\VarcalSys.DddCleanArch\* -DestinationPath "VarcalSys.DddCleanArch.zip"
Compress-Archive -Path .\VarcalSys.CleanArch\* -DestinationPath "VarcalSys.CleanArch.zip"
Compress-Archive -Path .\VarcalSys.MinimalApi\* -DestinationPath "VarcalSys.MinimalApi.zip"
Compress-Archive -Path .\VarcalSys.RabbitMqWorker\* -DestinationPath "VarcalSys.RabbitMqWorker.zip"
```

Copie os arquivos .zip para a pasta de templates do VS e reinicie o Visual Studio.
Os templates aparecerão em **Arquivo > Novo > Projeto** sob a categoria **VarcalSys**.

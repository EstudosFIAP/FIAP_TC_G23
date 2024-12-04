# Usar a imagem base do .NET 8 SDK para construir o projeto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar o arquivo de projeto e restaurar as dependências
COPY ["Consumer/Consumer.csproj", "Consumer/"]
RUN dotnet restore "./Consumer/Consumer.csproj"

# Copiar todos os arquivos e construir o projeto
COPY . .
WORKDIR "/src/Consumer"
RUN dotnet build "./Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Usar root para instalar o curl
USER root
RUN apt-get update && apt-get install -y curl

# Publicar a aplicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Usar a imagem base do .NET 8 Runtime para executar o projeto
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Consumer.dll"]

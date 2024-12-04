# Usar a imagem base do .NET 8 SDK para construir o projeto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar o arquivo de projeto e restaurar as dependências
COPY ["FIAP_TC.Contact.Api/FIAP_TC.Contact.Api.csproj", "FIAP_TC.Contact.Api/"]
RUN dotnet restore "./FIAP_TC.Contact.Api/FIAP_TC.Contact.Api.csproj"

# Copiar todos os arquivos e construir o projeto
COPY . .
WORKDIR "/src/FIAP_TC.Contact.Api"
RUN dotnet build "./FIAP_TC.Contact.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicar a aplicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FIAP_TC.Contact.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Usar a imagem base do .NET 8 Runtime para executar o projeto
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "FIAP_TC.Contact.Api.dll"]

# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia os arquivos da solução para o container
COPY . ./

# Restaura as dependências
RUN dotnet restore

# Compila o projeto, incluindo testes
RUN dotnet build --configuration Release --no-restore

# Executa os testes para garantir que o código está funcionando antes de criar a imagem final
RUN dotnet test --no-build --verbosity normal

# Publica a aplicação (gera os binários otimizados para produção)
RUN dotnet publish -c Release -o /app/out --no-restore --no-build

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia os binários da etapa de build para o runtime
COPY --from=build /app/out .

# Expõe a porta usada pela aplicação
EXPOSE 80

# Define o entrypoint para iniciar a aplicação
ENTRYPOINT ["dotnet", "Challenge-Blip.WebApi.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Fiap.Web.Alunos/Fiap.Web.Alunos.csproj", "Fiap.Web.Alunos/"]
RUN dotnet restore "Fiap.Web.Alunos/Fiap.Web.Alunos.csproj"
COPY . .
WORKDIR "/src/Fiap.Web.Alunos"
RUN dotnet build "./Fiap.Web.Alunos.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Fiap.Web.Alunos.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fiap.Web.Alunos.dll"]

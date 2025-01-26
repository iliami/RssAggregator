FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_URLS=http://+:8080
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./RssAggregator.BusModels/RssAggregator.BusModels.csproj", "RssAggregator.BusModels/"]
COPY ["./RssAggregator.Application/RssAggregator.Application.csproj", "RssAggregator.Application/"]
COPY ["./RssAggregator.Application.DependencyInjection/RssAggregator.Application.DependencyInjection.csproj", "RssAggregator.Application.DependencyInjection/"]
COPY ["./RssAggregator.Infrastructure/RssAggregator.Infrastructure.csproj", "RssAggregator.Infrastructure/"]
COPY ["./RssAggregator.Infrastructure.DependencyInjection/RssAggregator.Infrastructure.DependencyInjection.csproj", "RssAggregator.Infrastructure.DependencyInjection/"]
COPY ["./RssAggregator.Persistence/RssAggregator.Persistence.csproj", "RssAggregator.Persistence/"]
COPY ["./RssAggregator.Persistence.DependencyInjection/RssAggregator.Persistence.DependencyInjection.csproj", "RssAggregator.Persistence.DependencyInjection/"]
COPY ["./RssAggregator.Presentation/RssAggregator.Presentation.csproj", "RssAggregator.Presentation/"]
RUN dotnet restore "RssAggregator.Presentation/RssAggregator.Presentation.csproj"
COPY [".", "."]
WORKDIR /src/RssAggregator.Presentation
RUN dotnet build "RssAggregator.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RssAggregator.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RssAggregator.Presentation.dll"]

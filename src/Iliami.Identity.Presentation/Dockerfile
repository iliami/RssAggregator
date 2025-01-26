FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_URLS=http://+:8080
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./RssAggregator.BusModels/RssAggregator.BusModels.csproj", "RssAggregator.BusModels/"]
COPY ["./Iliami.Identity.Domain/Iliami.Identity.Domain.csproj", "Iliami.Identity.Domain/"]
COPY ["./Iliami.Identity.Domain.DependencyInjection/Iliami.Identity.Domain.DependencyInjection.csproj", "Iliami.Identity.Domain.DependencyInjection/"]
COPY ["./Iliami.Identity.Infrastructure/Iliami.Identity.Infrastructure.csproj", "Iliami.Identity.Infrastructure/"]
COPY ["./Iliami.Identity.Infrastructure.DependencyInjection/Iliami.Identity.Infrastructure.DependencyInjection.csproj", "Iliami.Identity.Infrastructure.DependencyInjection/"]
COPY ["./Iliami.Identity.Presentation/Iliami.Identity.Presentation.csproj", "Iliami.Identity.Presentation/"]
RUN dotnet restore "Iliami.Identity.Presentation/Iliami.Identity.Presentation.csproj"
COPY . .
WORKDIR /src/Iliami.Identity.Presentation
RUN dotnet build "Iliami.Identity.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Iliami.Identity.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Iliami.Identity.Presentation.dll"]

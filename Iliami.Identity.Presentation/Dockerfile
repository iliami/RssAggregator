FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Iliami.Identity.Presentation/Iliami.Identity.Presentation.csproj", "Iliami.Identity.Presentation/"]
RUN dotnet restore "Iliami.Identity.Presentation/Iliami.Identity.Presentation.csproj"
COPY . .
WORKDIR "/src/Iliami.Identity.Presentation"
RUN dotnet build "Iliami.Identity.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Iliami.Identity.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Iliami.Identity.Presentation.dll"]

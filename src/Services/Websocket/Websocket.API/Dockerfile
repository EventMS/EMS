#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/Websocket/Websocket.API/Websocket.API.csproj", "Services/Websocket/Websocket.API/"]
COPY ["BuildingBlocks/EventBus/EventLogEF/EventLogEF.csproj", "BuildingBlocks/EventBus/EventLogEF/"]
COPY ["BuildingBlocks/WebHostCustomization/WebHost.Customization/TemplateWebHost.Customization.csproj", "BuildingBlocks/WebHostCustomization/WebHost.Customization/"]
RUN dotnet restore "Services/Websocket/Websocket.API/Websocket.API.csproj"
COPY . .
WORKDIR "/src/Services/Websocket/Websocket.API"
RUN dotnet build "Websocket.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Websocket.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Websocket.API.dll"]
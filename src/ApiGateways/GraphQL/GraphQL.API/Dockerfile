#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ApiGateways/GraphQL/GraphQL.API/GraphQL.API.csproj", "ApiGateways/GraphQL/GraphQL.API/"]
COPY ["BuildingBlocks/EventBus/EventLogEF/EventLogEF.csproj", "BuildingBlocks/EventBus/EventLogEF/"]
COPY ["BuildingBlocks/WebHostCustomization/WebHost.Customization/TemplateWebHost.Customization.csproj", "BuildingBlocks/WebHostCustomization/WebHost.Customization/"]
RUN dotnet restore "ApiGateways/GraphQL/GraphQL.API/GraphQL.API.csproj"
COPY . .
WORKDIR "/src/ApiGateways/GraphQL/GraphQL.API"
RUN dotnet build "GraphQL.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GraphQL.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GraphQL.API.dll"]
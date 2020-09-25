FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

# It's important to keep lines from here down to "COPY . ." identical in all Dockerfiles
# to take advantage of Docker's build cache, to speed up local container builds
COPY "EMS.sln" "EMS.sln"

COPY "BuildingBlocks/EventBus/EventBus/EventBus.csproj" "BuildingBlocks/EventBus/EventBus/EventBus.csproj"
COPY "BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj" "BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj"
COPY "BuildingBlocks/EventBus/EventBusServiceBus/EventBusServiceBus.csproj" "BuildingBlocks/EventBus/EventBusServiceBus/EventBusServiceBus.csproj"
COPY "BuildingBlocks/EventBus/EventLogEF/EventLogEF.csproj" "BuildingBlocks/EventBus/EventLogEF/EventLogEF.csproj"
COPY "BuildingBlocks/WebHostCustomization/WebHost.Customization/TemplateWebHost.Customization.csproj" "BuildingBlocks/WebHostCustomization/WebHost.Customization/TemplateWebHost.Customization.csproj"
COPY "Services/Template1/Template1.API/Template1.API.csproj" "Services/Template1/Template1.API/Template1.API.csproj"
	COPY "Services/Club/Club.API/Club.API.csproj" "Services/Club/Club.API/Club.API.csproj"
	COPY "Services/Permission/Permission.API/Permission.API.csproj" "Services/Permission/Permission.API/Permission.API.csproj"
COPY "ApiGateways/GraphQL/GraphQL.API/GraphQL.API.csproj" "ApiGateways/GraphQL/GraphQL.API/GraphQL.API.csproj"
COPY "Services/Identity/Identity.API/Identity.API.csproj" "Services/Identity/Identity.API/Identity.API.csproj"
COPY "Web/WebStatus/WebStatus.csproj" "Web/WebStatus/WebStatus.csproj"

COPY "docker-compose.dcproj" "docker-compose.dcproj"

COPY "NuGet.config" "NuGet.config"

RUN dotnet restore "EMS.sln"

COPY . .
WORKDIR /src/Services/Identity/Identity.API
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Identity.API.dll"]

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

# It's important to keep lines from here down to "COPY . ." identical in all Dockerfiles
# to take advantage of Docker's build cache, to speed up local container builds
COPY "EMS.sln" "EMS.sln"

COPY "BuildingBlocks/EventBus/EventLogEF/EventLogEF.csproj" "BuildingBlocks/EventBus/EventLogEF/EventLogEF.csproj"
COPY "BuildingBlocks/WebHostCustomization/WebHost.Customization/TemplateWebHost.Customization.csproj" "BuildingBlocks/WebHostCustomization/WebHost.Customization/TemplateWebHost.Customization.csproj"
COPY "BuildingBlocks/SharedTesting/SharedTesting.csproj" "BuildingBlocks/SharedTesting/SharedTesting.csproj"
COPY "Services/Template1/Template1.API/Template1.API.csproj" "Services/Template1/Template1.API/Template1.API.csproj"
	COPY "Services/PaymentWebhook/PaymentWebhook.API/PaymentWebhook.API.csproj" "Services/PaymentWebhook/PaymentWebhook.API/PaymentWebhook.API.csproj"
	COPY "Services/EventParticipant/EventParticipant.API/EventParticipant.API.csproj" "Services/EventParticipant/EventParticipant.API/EventParticipant.API.csproj"
	COPY "Services/Event/Event.API/Event.API.csproj" "Services/Event/Event.API/Event.API.csproj"
	COPY "Services/Room/Room.API/Room.API.csproj" "Services/Room/Room.API/Room.API.csproj"
	COPY "Services/ClubMember/ClubMember.API/ClubMember.API.csproj" "Services/ClubMember/ClubMember.API/ClubMember.API.csproj"
	COPY "Services/Subscription/Subscription.API/Subscription.API.csproj" "Services/Subscription/Subscription.API/Subscription.API.csproj"
	COPY "Services/Club/Club.API/Club.API.csproj" "Services/Club/Club.API/Club.API.csproj"
	COPY "Services/Permission/Permission.API/Permission.API.csproj" "Services/Permission/Permission.API/Permission.API.csproj"
COPY "ApiGateways/GraphQL/GraphQL.API/GraphQL.API.csproj" "ApiGateways/GraphQL/GraphQL.API/GraphQL.API.csproj"
COPY "Services/Identity/Identity.API/Identity.API.csproj" "Services/Identity/Identity.API/Identity.API.csproj"
COPY "Services/Template1/Template1.API.UnitTests/Template1.API.UnitTests.csproj" "Services/Template1/Template1.API.UnitTests/Template1.API.UnitTests.csproj"
	COPY "Services/PaymentWebhook/PaymentWebhook.API.UnitTests/PaymentWebhook.API.UnitTests.csproj" "Services/PaymentWebhook/PaymentWebhook.API.UnitTests/PaymentWebhook.API.UnitTests.csproj"
	COPY "Services/EventParticipant/EventParticipant.API.UnitTests/EventParticipant.API.UnitTests.csproj" "Services/EventParticipant/EventParticipant.API.UnitTests/EventParticipant.API.UnitTests.csproj"
	COPY "Services/Event/Event.API.UnitTests/Event.API.UnitTests.csproj" "Services/Event/Event.API.UnitTests/Event.API.UnitTests.csproj"
COPY "Services/Room/Room.API.UnitTests/Room.API.UnitTests.csproj" "Services/Room/Room.API.UnitTests/Room.API.UnitTests.csproj"
COPY "Services/ClubMember/ClubMember.API.UnitTests/ClubMember.API.UnitTests.csproj" "Services/ClubMember/ClubMember.API.UnitTests/ClubMember.API.UnitTests.csproj"
COPY "Services/Subscription/Subscription.API.UnitTests/Subscription.API.UnitTests.csproj" "Services/Subscription/Subscription.API.UnitTests/Subscription.API.UnitTests.csproj"
COPY "Services/Club/Club.API.UnitTests/Club.API.UnitTests.csproj" "Services/Club/Club.API.UnitTests/Club.API.UnitTests.csproj"
COPY "Services/Permission/Permission.API/Permission.API.csproj" "Services/Permission/Permission.API/Permission.API.csproj"

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

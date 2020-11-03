#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/PaymentWebhook/PaymentWebhook.API/PaymentWebhook.API.csproj", "Services/PaymentWebhook/PaymentWebhook.API/"]
COPY ["BuildingBlocks/EventBus/EventLogEF/EventLogEF.csproj", "BuildingBlocks/EventBus/EventLogEF/"]
COPY ["BuildingBlocks/WebHostCustomization/WebHost.Customization/TemplateWebHost.Customization.csproj", "BuildingBlocks/WebHostCustomization/WebHost.Customization/"]
RUN dotnet restore "Services/PaymentWebhook/PaymentWebhook.API/PaymentWebhook.API.csproj"
COPY . .
WORKDIR "/src/Services/PaymentWebhook/PaymentWebhook.API"
RUN dotnet build "PaymentWebhook.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PaymentWebhook.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentWebhook.API.dll"]
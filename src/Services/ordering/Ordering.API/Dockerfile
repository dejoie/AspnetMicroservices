#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/ordering/Ordering.API/Ordering.API.csproj", "services/ordering/Ordering.API/"]
COPY ["buildingblocks/EventBus.Messages/EventBus.Messages.csproj", "buildingblocks/EventBus.Messages/"]
COPY ["Services/ordering/Ordering.Application/Ordering.Application.csproj", "Services/ordering/Ordering.Application/"]
COPY ["Services/ordering/Ordering.Domain/Ordering.Domain.csproj", "Services/ordering/Ordering.Domain/"]
COPY ["Services/ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj", "Services/ordering/Ordering.Infrastructure/"]
RUN dotnet restore "services/ordering/Ordering.API/Ordering.API.csproj"
COPY . .
WORKDIR "/src/services/ordering/Ordering.API"
RUN dotnet build "Ordering.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ordering.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]
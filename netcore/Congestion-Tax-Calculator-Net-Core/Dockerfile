# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app



# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . .
COPY ["congestion-tax-calculator.Web/congestion-tax-calculator.Web.csproj", "congestion-tax-calculator.Web/"]
COPY ["Congestion-tax-calculator.AppDomain/Congestion-tax-calculator.AppDomain.csproj", "Congestion-tax-calculator.AppDomain/"]
COPY ["Congestion-tax-calculator.Infrastructure/Congestion-tax-calculator.Infrastructure.csproj", "Congestion-tax-calculator.Infrastructure/"]
RUN dotnet restore "./congestion-tax-calculator.Web/congestion-tax-calculator.Web.csproj"
#COPY . .
WORKDIR "/src/congestion-tax-calculator.Web"
RUN dotnet build "./congestion-tax-calculator.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./congestion-tax-calculator.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
ENV ASPNETCORE_URLS=http://*:5050
EXPOSE 5050
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "congestion-tax-calculator.Web.dll"]
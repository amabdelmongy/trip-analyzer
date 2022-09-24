FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/TripAnalyzer.Api/TripAnalyzer.Api.csproj", "TripAnalyzer.Api/"]
COPY ["src/Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "./TripAnalyzer.Api/TripAnalyzer.Api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "src/TripAnalyzer.Api/TripAnalyzer.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/TripAnalyzer.Api/TripAnalyzer.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TripAnalyzer.Api.dll"]
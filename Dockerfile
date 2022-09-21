FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /
COPY ["TripAnalyzer.Api/TripAnalyzer.Api.csproj", "TripAnalyzer.Api/"]
RUN dotnet restore "./TripAnalyzer.Api/TripAnalyzer.Api.csproj"
COPY . .
WORKDIR "/."
RUN dotnet build "TripAnalyzer.Api/TripAnalyzer.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TripAnalyzer.Api/TripAnalyzer.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TripAnalyzer.Api.dll"]
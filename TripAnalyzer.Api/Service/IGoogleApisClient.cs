namespace TripAnalyzer.Api.Service;

public interface IGoogleApisClient
{
    string? GetAddress(float positionLat, float positionLong);
}

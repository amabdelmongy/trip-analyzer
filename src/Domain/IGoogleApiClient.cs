namespace Domain;

public interface IGoogleApiClient
{
    string? GetAddress(float positionLat, float positionLong);
}

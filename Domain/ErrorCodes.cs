namespace Domain;

public class ErrorCodes
{
    public const string VehiclepushVinDoesNotHaveValue = "'Vin' must not be empty.";
    public const string VehiclepushDataIsLessThan = "'Data Count' must be greater than or equal to '2'.";
    public const string VehiclepushDataPositionlatDoesNotHaveValue = "'Position Lat' must not be empty.";
    public const string VehiclepushDataPositionlongDoesNotHaveValue = "'Position Long' must not be empty.";
    public const string VehiclepushDataTimestampDoesNotHaveValue = "'Timestamp' must not be empty.";
}
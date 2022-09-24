using System.ComponentModel.DataAnnotations;

namespace Domain.Aggregate;

public class DataPoint
{
    public DataPoint(
        int? odometer,
        float? positionLat,
        float? positionLong,
        long? timestamp,
        int? fuelLevel)
    {
        Odometer = odometer;
        PositionLat = positionLat;
        PositionLong = positionLong;
        Timestamp = timestamp;
        FuelLevel = fuelLevel;
    }

    /// <summary>
    /// unix timestamp
    /// </summary>
    /// <value>unix timestamp</value>
    public long? Timestamp { get; }

    /// <summary>
    /// odometer value for a given time
    /// </summary>
    /// <value>odometer value for a given time</value>
    public int? Odometer { get; }

    /// <summary>
    /// fuel level for a given time in percent
    /// </summary>
    /// <value>fuel level for a given time in percent</value>
    [Range(0, 100)]
    public int? FuelLevel { get; }

    /// <summary>
    /// latitude position for a given time
    /// </summary>
    /// <value>latitude position for a given time</value>
    public float? PositionLat { get; }

    /// <summary>
    /// longitude position for a given time
    /// </summary>
    /// <value>longitude position for a given time</value>
    public float? PositionLong { get; }
}

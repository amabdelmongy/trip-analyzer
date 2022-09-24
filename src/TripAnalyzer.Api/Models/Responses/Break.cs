namespace TripAnalyzer.Api.Models.Responses;

public class Break
{
    public Break()
    {
    }

    public Break(float? positionLat, float? positionLong, long? endTimestamp, long? startTimestamp)
    {
        PositionLat = positionLat;
        PositionLong = positionLong;
        EndTimestamp = endTimestamp;
        StartTimestamp = startTimestamp;
    }

    /// <summary>
    /// Gets or Sets StartTimestamp
    /// </summary>
    public long? StartTimestamp { get; set; }

    /// <summary>
    /// Gets or Sets EndTimestamp
    /// </summary>
    public long? EndTimestamp { get; set; }

    /// <summary>
    /// Gets or Sets PositionLat
    /// </summary>
    public float? PositionLat { get; set; }

    /// <summary>
    /// Gets or Sets PositionLong
    /// </summary>
    public float? PositionLong { get; set; }
}

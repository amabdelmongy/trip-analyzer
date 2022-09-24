namespace Domain.Aggregate;

public class BreakAggregate
{

    public BreakAggregate(
        float? positionLat,
        float? positionLong,
        long? startTimestamp,
        long? endTimestamp)
    {
        PositionLat = positionLat;
        PositionLong = positionLong;
        StartTimestamp = startTimestamp;
        EndTimestamp = endTimestamp;
    }

    /// <summary>
    /// Gets or Sets StartTimestamp
    /// </summary>
    public long? StartTimestamp { get; }

    /// <summary>
    /// Gets or Sets EndTimestamp
    /// </summary>
    public long? EndTimestamp { get; }

    /// <summary>
    /// Gets or Sets PositionLat
    /// </summary>
    public float? PositionLat { get; }

    /// <summary>
    /// Gets or Sets PositionLong
    /// </summary>
    public float? PositionLong { get; }
}

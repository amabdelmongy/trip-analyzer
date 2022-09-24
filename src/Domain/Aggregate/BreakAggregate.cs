namespace Domain.Aggregate;

public class BreakAggregate
{
    public BreakAggregate(
        float? positionLat,
        float? positionLong,
        long? startTimestamp,
        long? endTimestamp,
        int? startFuelLevel,
        int? endFuelLevel)
    {
        PositionLat = positionLat;
        PositionLong = positionLong;
        StartTimestamp = startTimestamp;
        EndTimestamp = endTimestamp;
        StartFuelLevel = startFuelLevel;
        EndFuelLevel = endFuelLevel;
    }

    public long? StartTimestamp { get; }

    public long? EndTimestamp { get; }

    public float? PositionLat { get; }

    public float? PositionLong { get; }

    public int? StartFuelLevel { get; }

    public int? EndFuelLevel { get; }

    public int? FuelLevel => EndFuelLevel - StartFuelLevel;

    public long? Period => EndTimestamp - StartTimestamp;
}

namespace Domain.Aggregate;

public class VehiclePushAnalysisAggregate
{
    public VehiclePushAnalysisAggregate(
        string vin,
        string departure,
        string destination,
        List<BreakAggregate> refuelStops,
        float? consumption,
        List<BreakAggregate> breaks)
    {
        Vin = vin;
        Departure = departure;
        Destination = destination;
        RefuelStops = refuelStops;
        Consumption = consumption;
        Breaks = breaks;
    }

    /// <summary>
    /// vehicle identification number
    /// </summary>
    /// <value>vehicle identification number</value>
    public string Vin { get; }

    /// <summary>
    /// city/location where the trip started
    /// </summary>
    /// <value>city/location where the trip started</value>
    public string Departure { get; }

    /// <summary>
    /// city/location where the trip ended
    /// </summary>
    /// <value>city/location where the trip ended</value>
    public string Destination { get; }

    /// <summary>
    /// a list of all refuel stops during the trip
    /// </summary>
    /// <value>a list of all refuel stops during the trip</value>
    public List<BreakAggregate> RefuelStops { get; }

    /// <summary>
    /// the average consumption during the trip (l/100km)
    /// https://www.transpoco.com/knowledge/how-is-fuel-consumption-calculated
    /// </summary>
    /// <value>the average consumption during the trip (l/100km)</value>
    public float? Consumption { get; }

    /// <summary>
    /// a list of all breaks during the trip including the refuel stops
    /// </summary>
    /// <value>a list of all breaks during the trip including the refuel stops</value>
    public List<BreakAggregate> Breaks { get; }
}

namespace TripAnalyzer.Api.Models.Responses;

public class VehiclePushAnalysis
{
    /// <summary>
    /// vehicle identification number
    /// </summary>
    /// <value>vehicle identification number</value>
    public string Vin { get; set; }

    /// <summary>
    /// city/location where the trip started
    /// </summary>
    /// <value>city/location where the trip started</value>
    public string Departure { get; set; }

    /// <summary>
    /// city/location where the trip ended
    /// </summary>
    /// <value>city/location where the trip ended</value>
    public string Destination { get; set; }

    /// <summary>
    /// a list of all refuel stops during the trip
    /// </summary>
    /// <value>a list of all refuel stops during the trip</value>
    public List<Break> RefuelStops { get; set; }

    /// <summary>
    /// the average consumption during the trip (l/100km)
    /// https://www.transpoco.com/knowledge/how-is-fuel-consumption-calculated
    /// </summary>
    /// <value>the average consumption during the trip (l/100km)</value>
    public float? Consumption { get; set; }

    /// <summary>
    /// a list of all breaks during the trip including the refuel stops
    /// </summary>
    /// <value>a list of all breaks during the trip including the refuel stops</value>
    public List<Break> Breaks { get; set; }
}

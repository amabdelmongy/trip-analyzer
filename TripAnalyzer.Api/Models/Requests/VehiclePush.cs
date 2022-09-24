namespace TripAnalyzer.Api.Models.Requests;

public class VehiclePush
{
    /// <summary>
    /// vehicle identification number
    /// </summary>
    /// <value>vehicle identification number</value>
    public string Vin { get; set; }

    /// <summary>
    /// threshold to determine if a car just stopped or did a break during the trip
    /// </summary>
    /// <value>threshold to determine if a car just stopped or did a break during the trip</value>
    public int? BreakThreshold { get; set; }

    /// <summary>
    /// the size of the gas tank in liter
    /// </summary>
    /// <value>the size of the gas tank in liter</value>
    public int? GasTankSize { get; set; }

    /// <summary>
    /// Gets or Sets Data
    /// </summary>
    public List<VehiclePushDataPoint> Data { get; set; }

}

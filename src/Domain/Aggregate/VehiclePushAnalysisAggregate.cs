namespace Domain.Aggregate;

public class VehiclePushAnalysisAggregate
{
    public VehiclePushAnalysisAggregate(
        string vin,
        int? breakThreshold,
        int? gasTankSize,
        List<DataPoint> data)
    {
        Vin = vin;
        BreakThreshold = breakThreshold;
        GasTankSize = gasTankSize;
        Data = data.OrderBy(t => t.Timestamp).ToList();
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
    public string Departure { get; set; }

    /// <summary>
    /// city/location where the trip ended
    /// </summary>
    /// <value>city/location where the trip ended</value>
    public string Destination { get; set; }

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

    private List<BreakAggregate> _breaks = new();
    /// <summary>
    /// a list of all breaks during the trip including the refuel stops
    /// </summary>
    /// <value>a list of all breaks during the trip including the refuel stops</value>
    public List<BreakAggregate> Breaks
    {
        get
        {
            if (_breaks.Any()) return _breaks;

            _breaks =
                Data
                    .GroupBy(x =>
                        new
                        {
                            x.PositionLat,
                            x.PositionLong,
                            x.Odometer
                        }, (key, group) =>
                        new
                        {
                            key,
                            group
                        })
                    .Where(t =>
                        t.group.Count() > 1
                    )
                    .Select(t =>
                        new BreakAggregate(
                            t.key.PositionLat,
                            t.key.PositionLong,
                            t.group.First().Timestamp,
                            t.group.Last().Timestamp,
                            t.group.First().FuelLevel,
                            t.group.Last().FuelLevel
                        ))
                    .Where(t =>
                        t.Period >= BreakThreshold
                    ).ToList();
            return _breaks;
        }
    }

    /// <summary>
    /// a list of all refuel stops during the trip
    /// </summary>
    /// <value>a list of all refuel stops during the trip</value>
    public List<BreakAggregate> RefuelStops
    {
        get
        {
            return Breaks
                .Where(t => t.FuelLevel > 0)
                .ToList();
        }
    }

    /// <summary>
    /// the average consumption during the trip (l/100km)
    /// https://www.transpoco.com/knowledge/how-is-fuel-consumption-calculated
    /// </summary>
    /// <value>the average consumption during the trip (l/100km)</value>
    public float? Consumption
    {
        get
        {
            var travelOdometer = (DestinationData.Odometer - DepartureData.Odometer) / 1000;
            if(travelOdometer <= 0) return null;

            var refuelStopsFuelLevel = RefuelStops.Sum(t => t.FuelLevel);
            var fuelConsumed = DepartureData.FuelLevel + refuelStopsFuelLevel - DestinationData.FuelLevel;
            var fuelLiters = ConvertFuelToLitersFromPercentage(fuelConsumed!.Value, GasTankSize!.Value);
            var consumption = fuelLiters * 100 / travelOdometer;
            if (consumption != null)
                return (float)Math.Round((decimal)consumption, 2);

            return null;

            float ConvertFuelToLitersFromPercentage(int fuelLevel, int gasTankSize) => fuelLevel * gasTankSize / 100;
        }
    }

    /// <summary>
    /// Gets or Sets Data
    /// </summary>
    public List<DataPoint> Data { get; set; }

    /// <summary>
    /// Departure Data
    /// </summary>
    public DataPoint DepartureData => Data.First();

    /// <summary>
    /// Destination Data
    /// </summary>
    public DataPoint DestinationData => Data.Last();

}

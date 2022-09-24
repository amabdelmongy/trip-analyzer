namespace Domain.UnitTest.Aggregate;

public class VehiclePushAnalysisAggregateTest
{
    private Fixture _fixture;
    private string _vin;
    private int? _breakThreshold;
    private int? _gasTankSize;
    private List<DataPoint> _data;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _vin = _fixture.Create<string>();
        _breakThreshold = _fixture.Create<int>();
        _gasTankSize = _fixture.Create<int>();
        _data = _fixture.Create<List<DataPoint>>();
    }

    [Test]
    public void WHEN_analysis_vehiclePush_THEN_return_VehiclePushAnalysis()
    {
        var actual = new VehiclePushAnalysisAggregate(
                _vin,
                _breakThreshold,
                _gasTankSize,
                _data
                );

        Assert.That(actual.Vin, Is.EqualTo(_vin));
        Assert.That(actual.RefuelStops.Count, Is.EqualTo(0));
        Assert.That(actual.Breaks.Count, Is.EqualTo(0));
        Assert.That(actual.GasTankSize, Is.EqualTo(_gasTankSize));
        Assert.That(actual.Consumption, Is.EqualTo(null));
        Assert.That(actual.BreakThreshold, Is.EqualTo(_breakThreshold));
    }

    [Test]
    public void WHEN_analysis_vehiclePush_with_break_THEN_return_VehiclePushAnalysis()
    {
        _breakThreshold = 1000;
        _data = new List<DataPoint>
        {
            new(odometer: 50, positionLat: 123, positionLong: 22, timestamp: 100122, fuelLevel: 50),
            new(odometer: 50, positionLat: 123, positionLong: 22, timestamp: 200123, fuelLevel: 50),
            new(odometer: 50, positionLat: 123, positionLong: 22, timestamp: 300123, fuelLevel: 50),
            new(odometer: 50, positionLat: 123, positionLong: 22, timestamp: 400123, fuelLevel: 50),
            new(odometer: 50, positionLat: 123, positionLong: 22, timestamp: 500123, fuelLevel: 50)
        };

        var actual = new VehiclePushAnalysisAggregate(
            _vin,
            _breakThreshold,
            _gasTankSize,
            _data
        );

        Assert.That(actual!.Breaks.Count, Is.EqualTo(1));

        var breakFirst = actual.Breaks.First();
        Assert.That(breakFirst.PositionLat, Is.EqualTo(_data.First().PositionLat));
        Assert.That(breakFirst.PositionLong, Is.EqualTo(_data.First().PositionLong));
        Assert.That(breakFirst.StartTimestamp, Is.EqualTo(_data.First().Timestamp));
        Assert.That(breakFirst.EndTimestamp, Is.EqualTo(_data.Last().Timestamp));

        Assert.That(actual.RefuelStops.Count, Is.EqualTo(0));
    }
    [Test]
    public void WHEN_analysis_vehiclePush_with_break_less_than_breakThreshold_THEN_return_VehiclePushAnalysis()
    {
        _breakThreshold = 10000;
        _data = new List<DataPoint>
        {
            new(odometer: 50, positionLat: 123, positionLong: 22, timestamp: 100122, fuelLevel: 50),
            new(odometer: 50, positionLat: 123, positionLong: 22, timestamp: 100123, fuelLevel: 50),
            new(odometer: 50, positionLat: 123, positionLong: 22, timestamp: 100123, fuelLevel: 50),
            new(odometer: 50, positionLat: 123, positionLong: 22, timestamp: 100123, fuelLevel: 50),
            new(odometer: 50, positionLat: 123, positionLong: 22, timestamp: 100123, fuelLevel: 50)
        };

        var actual = new VehiclePushAnalysisAggregate(
            _vin,
            _breakThreshold,
            _gasTankSize,
            _data
        );

        Assert.That(actual!.Breaks.Count, Is.EqualTo(0));
    }

    [Test]
    public void WHEN_analysis_vehiclePush_with_break_different_Odometer_THEN_return_VehiclePushAnalysis()
    {
        _data = new List<DataPoint>
        {
            new(odometer: 10050, positionLat: 123, positionLong: 22, timestamp: 100122, fuelLevel: 50),
            new(odometer: 10050, positionLat: 123, positionLong: 22, timestamp: 100123, fuelLevel: 50),
            new(odometer: 20050, positionLat: 123, positionLong: 22, timestamp: 200122, fuelLevel: 80),
            new(odometer: 20050, positionLat: 123, positionLong: 22, timestamp: 200123, fuelLevel: 80),
        };

        var actual = new VehiclePushAnalysisAggregate(
            _vin,
            _breakThreshold,
            _gasTankSize,
            _data
        );

        Assert.That(actual!.Breaks.Count, Is.EqualTo(2));

        var breakFirst = actual.Breaks.First();
        Assert.That(breakFirst.PositionLat, Is.EqualTo(_data.First().PositionLat));
        Assert.That(breakFirst.PositionLong, Is.EqualTo(_data.First().PositionLong));
        Assert.That(breakFirst.StartTimestamp, Is.EqualTo(_data.First().Timestamp));
        Assert.That(breakFirst.EndTimestamp, Is.EqualTo(_data[1].Timestamp));

        Assert.That(actual.RefuelStops.Count, Is.EqualTo(0));
    }

    [Test]
    public void WHEN_analysis_vehiclePush_with_refuelStops_THEN_return_VehiclePushAnalysis()
    {
        _data = new List<DataPoint>
        {
            new(odometer: 10000, positionLat: 123, positionLong: 22, timestamp: 100122, fuelLevel: 20),
            new(odometer: 10000, positionLat: 123, positionLong: 22, timestamp: 200123, fuelLevel: 90),
            new(odometer: 10000, positionLat: 123, positionLong: 22, timestamp: 300123, fuelLevel: 90),
            new(odometer: 10000, positionLat: 123, positionLong: 22, timestamp: 500123, fuelLevel: 90)
        };

        var actual = new VehiclePushAnalysisAggregate(
            _vin,
            _breakThreshold,
            _gasTankSize,
            _data
        );

        Assert.That(actual!.RefuelStops.Count, Is.EqualTo(1));

        var refuelStops = actual.RefuelStops.First();
        Assert.That(refuelStops.PositionLat, Is.EqualTo(_data.First().PositionLat));
        Assert.That(refuelStops.PositionLong, Is.EqualTo(_data.First().PositionLong));
        Assert.That(refuelStops.StartTimestamp, Is.EqualTo(_data.First().Timestamp));
        Assert.That(refuelStops.EndTimestamp, Is.EqualTo(_data.Last().Timestamp));

        //a list of all breaks during the trip including the refuel stops
        Assert.That(actual.Breaks.Count, Is.EqualTo(1));
        var breakFirst = actual.Breaks.First();
        Assert.That(breakFirst.PositionLat, Is.EqualTo(_data.First().PositionLat));
        Assert.That(breakFirst.PositionLong, Is.EqualTo(_data.First().PositionLong));
        Assert.That(breakFirst.StartTimestamp, Is.EqualTo(_data.First().Timestamp));
        Assert.That(breakFirst.EndTimestamp, Is.EqualTo(_data.Last().Timestamp));
    }

    [Test]
    public void WHEN_analysis_vehiclePush_with_2_data_point_THEN_return_consumption()
    {
        //    Odometer at 1st fuel fill eg 0km, Odometer ar 2nd fuel fill eg 500km
        //    Litres add to fill tank to max at 2nd fill(Litres used) eg 65 litre
        //    (65 X 100) ÷ 500 = 13 Litres / 100km
        _gasTankSize = 100;
        _data = new List<DataPoint>
        {
            new(odometer: 0, positionLat: 123, positionLong: 22, timestamp: 100122, fuelLevel: 100),
            new(odometer: 500000, positionLat: 223, positionLong: 32, timestamp: 500123, fuelLevel: 35)
        };

        var actual = new VehiclePushAnalysisAggregate(
            _vin,
            _breakThreshold,
            _gasTankSize,
            _data
        );
        Assert.That(actual!.Consumption, Is.EqualTo(13));
    }

    [Test]
    public void WHEN_analysis_vehiclePush_with_2_data_point_and_refuelStop_THEN_return_consumption()
    {
        _gasTankSize = 100;
        _data = new List<DataPoint>
        {
            new(odometer: 0, positionLat: 123, positionLong: 22, timestamp: 100122, fuelLevel: 100),
            new(odometer: 10000, positionLat: 123, positionLong: 22, timestamp: 100122, fuelLevel: 50),
            new(odometer: 10000, positionLat: 123, positionLong: 22, timestamp: 200123, fuelLevel: 100),
            new(odometer: 500000, positionLat: 223, positionLong: 32, timestamp: 500123, fuelLevel: 85)
        };

        var actual = new VehiclePushAnalysisAggregate(
            _vin,
            _breakThreshold,
            _gasTankSize,
            _data
        );
        Assert.That(actual!.Consumption, Is.EqualTo(13));
    }

    //todo add more unit tests
}

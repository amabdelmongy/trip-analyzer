using TripAnalyzer.Api.Service;

namespace TripAnalyzer.Api.UnitTest.Service_tests;

public class Tests
{
    private Fixture _fixture;
    private VehiclePushAnalysisService _vehiclePushAnalysisService;
    private string _address = "Stuttgart";

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        var googleApisClientMock = new Mock<IGoogleApisClient>();
        googleApisClientMock
            .Setup(t =>
                t.GetAddress(It.IsAny<float>(), It.IsAny<float>())
            )
            .Returns(_address);
        _vehiclePushAnalysisService = new VehiclePushAnalysisService(googleApisClientMock.Object);
    }

    [Test]
    public void WHEN_analysis_vehiclePush_THEN_return_VehiclePushAnalysis()
    {
        var input = _fixture.Create<VehiclePush>();

        var actual = _vehiclePushAnalysisService.Analysis(input);

        Assert.True(actual.IsOk);
        Assert.That(actual.Value.Vin, Is.EqualTo(input.Vin));
        Assert.That(actual.Value.Destination, Is.EqualTo(_address));
        Assert.That(actual.Value.Departure, Is.EqualTo(_address));
    }

    [Test]
    public void WHEN_analysis_vehiclePush_without_vin_THEN_return_Error()
    {
        var input = _fixture.Build<VehiclePush>().With(t=> t.Vin , "").Create();

        var actual = _vehiclePushAnalysisService.Analysis(input);

        Assert.False(actual.IsOk);
        Assert.That(actual.Errors.Count(), Is.EqualTo(1));
        Assert.That(actual.Errors.First().Subject, Is.EqualTo(ErrorCodes.VehiclepushVinDoesNotHaveValue));
    }

    [Test]
    public void WHEN_analysis_vehiclePush_without_Data_THEN_return_Error()
    {
        var input = _fixture.Build<VehiclePush>().With(t => t.Data, new List<VehiclePushDataPoint>()).Create();

        var actual = _vehiclePushAnalysisService.Analysis(input);

        Assert.False(actual.IsOk);
        Assert.That(actual.Errors.Count(), Is.EqualTo(1));
        Assert.That(actual.Errors.First().Subject, Is.EqualTo(ErrorCodes.VehiclepushDataIsLessThan));
    }

    [Test]
    public void WHEN_analysis_VehiclePush_data_without_timestamp_positionlong_positionlat_THEN_return_Errors()
    {
        var input = _fixture.Build<VehiclePush>().With(t => t.Data, new List<VehiclePushDataPoint>() { new VehiclePushDataPoint{ Timestamp = null}}).Create();

        var actual = _vehiclePushAnalysisService.Analysis(input);

        Assert.False(actual.IsOk);
        Assert.That(actual.Errors.Count(), Is.EqualTo(3));
        Assert.True(actual.Errors.ToList().Any(t => t.Subject == ErrorCodes.VehiclepushDataTimestampDoesNotHaveValue));
        Assert.True(actual.Errors.ToList().Any(t => t.Subject == ErrorCodes.VehiclepushDataPositionlongDoesNotHaveValue));
        Assert.True(actual.Errors.ToList().Any(t => t.Subject == ErrorCodes.VehiclepushDataPositionlatDoesNotHaveValue));
    }

    [Test]
    public void WHEN_analysis_vehiclePush_with_break_THEN_return_VehiclePushAnalysis()
    {
        var input = _fixture.Create<VehiclePush>();

        input.Data = new List<VehiclePushDataPoint>
        {
            new ()
            {
                Odometer = 50,
                PositionLat = 123,
                PositionLong = 22,
                Timestamp = 100122,
                FuelLevel = 50
            },
            new ()
            {
                Odometer = 50,
                PositionLat = 123,
                PositionLong = 22,
                Timestamp = 100123,
                FuelLevel = 50
            },
        };

        var actual = _vehiclePushAnalysisService.Analysis(input);

        Assert.True(actual.IsOk);
        Assert.That(actual.Value.Breaks.Count, Is.EqualTo(1));

        var breakFirst = actual.Value.Breaks.First();
        Assert.That(breakFirst.PositionLat, Is.EqualTo(input.Data.First().PositionLat));
        Assert.That(breakFirst.PositionLong, Is.EqualTo(input.Data.First().PositionLong));
        Assert.That(breakFirst.StartTimestamp, Is.EqualTo(input.Data.First().Timestamp));
        Assert.That(breakFirst.EndTimestamp, Is.EqualTo(input.Data.Last().Timestamp));

        //Assert.That(actual.Value.RefuelStops, Is.EqualTo(null));
    }

    [Test]
    public void WHEN_analysis_vehiclePush_with_refuelStops_THEN_return_VehiclePushAnalysis()
    {
        var input = _fixture.Create<VehiclePush>();

        input.Data = new List<VehiclePushDataPoint>
        {
            new ()
            {
                Odometer = 10000,
                PositionLat = 123,
                PositionLong = 22,
                Timestamp = 100122,
                FuelLevel = 20
            },
            new ()
            {
                Odometer = 10000,
                PositionLat = 123,
                PositionLong = 22,
                Timestamp = 100123,
                FuelLevel = 90
            },
        };

        var actual = _vehiclePushAnalysisService.Analysis(input);

        Assert.True(actual.IsOk);
        Assert.That(actual.Value.RefuelStops.Count, Is.EqualTo(1));

        var refuelStops = actual.Value.RefuelStops.First();
        Assert.That(refuelStops.PositionLat, Is.EqualTo(input.Data.First().PositionLat));
        Assert.That(refuelStops.PositionLong, Is.EqualTo(input.Data.First().PositionLong));
        Assert.That(refuelStops.StartTimestamp, Is.EqualTo(input.Data.First().Timestamp));
        Assert.That(refuelStops.EndTimestamp, Is.EqualTo(input.Data.Last().Timestamp));


        //Assert.That(actual.Value.Breaks, Is.EqualTo(null));
    }
    //todo add more unit tests
}

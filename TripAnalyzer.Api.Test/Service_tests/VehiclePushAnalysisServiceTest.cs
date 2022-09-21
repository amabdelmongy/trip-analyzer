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

    //todo add more unit tests
}

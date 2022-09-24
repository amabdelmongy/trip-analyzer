namespace Domain.UnitTest.Service_tests;

public class VehiclePushAnalysisServiceTest
{
    private Fixture _fixture;
    private VehiclePushService _vehiclePushService;
    private string _address = "Stuttgart";
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

        var googleApisClientMock = new Mock<IGoogleApiClient>();
        googleApisClientMock
            .Setup(t =>
                t.GetAddress(It.IsAny<float>(), It.IsAny<float>())
            )
            .Returns(_address);
        _vehiclePushService = new VehiclePushService(googleApisClientMock.Object);
    }

    [Test]
    public void WHEN_analysis_vehiclePush_THEN_return_VehiclePushAnalysis()
    {

        var actual =
            _vehiclePushService.Analysis(
                _vin,
                _breakThreshold,
                _gasTankSize,
                _data
                );

        Assert.True(actual.IsOk);
        Assert.That(actual.Value!.Vin, Is.EqualTo(_vin));
        Assert.That(actual.Value.Destination, Is.EqualTo(_address));
        Assert.That(actual.Value.Departure, Is.EqualTo(_address));
        Assert.That(actual.Value.RefuelStops.Count, Is.EqualTo(0));
        Assert.That(actual.Value.Breaks.Count, Is.EqualTo(0));
    }

    //[Test]
    //public void WHEN_analysis_vehiclePush_without_vin_THEN_return_Error()
    //{
    //    var input = _fixture.Build<VehiclePush>().With(t=> t.Vin , "").Create();

    //    var actual = _vehiclePushService.Analysis(input);

    //    Assert.False(actual.IsOk);
    //    Assert.That(actual.Errors.Count(), Is.EqualTo(1));
    //    Assert.That(actual.Errors.First().Message, Is.EqualTo(ErrorCodes.VehiclepushVinDoesNotHaveValue));
    //}

    //[Test]
    //public void WHEN_analysis_vehiclePush_without_Data_THEN_return_Error()
    //{
    //    var input = _fixture.Build<VehiclePush>().With(t => t.Data, new List<VehiclePushDataPoint>()).Create();

    //    var actual = _vehiclePushService.Analysis(input);

    //    Assert.False(actual.IsOk);
    //    Assert.That(actual.Errors.Count(), Is.EqualTo(1));
    //    Assert.That(actual.Errors.First().Message, Is.EqualTo(ErrorCodes.VehiclepushDataIsLessThan));
    //}

    //[Test]
    //public void WHEN_analysis_VehiclePush_data_without_timestamp_positionlong_positionlat_THEN_return_Errors()
    //{
    //    var input = _fixture.Build<VehiclePush>().With(t => t.Data, new List<VehiclePushDataPoint>() { new VehiclePushDataPoint{ Timestamp = null}}).Create();

    //    var actual = _vehiclePushService.Analysis(input);

    //    Assert.False(actual.IsOk);
    //    Assert.That(actual.Errors.Count(), Is.EqualTo(3));
    //    Assert.True(actual.Errors.ToList().Any(t => t.Message == ErrorCodes.VehiclepushDataTimestampDoesNotHaveValue));
    //    Assert.True(actual.Errors.ToList().Any(t => t.Message == ErrorCodes.VehiclepushDataPositionlongDoesNotHaveValue));
    //    Assert.True(actual.Errors.ToList().Any(t => t.Message == ErrorCodes.VehiclepushDataPositionlatDoesNotHaveValue));
    //}


    //todo add more unit tests
}

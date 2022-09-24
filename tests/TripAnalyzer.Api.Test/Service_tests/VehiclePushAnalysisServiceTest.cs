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

    //todo add more unit tests
}

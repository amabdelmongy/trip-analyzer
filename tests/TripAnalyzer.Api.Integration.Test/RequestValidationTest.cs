namespace TripAnalyzer.Api.Integration.Test;

internal class RequestValidationTest
{
    private const string UrlTrip = "/v1/trip";
    private Fixture _fixture;
    private string _address = "Stuttgart";
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        var googleApisClientMock = new Mock<IGoogleApiClient>();
        googleApisClientMock
            .Setup(t =>
                t.GetAddress(It.IsAny<float>(), It.IsAny<float>())
            )
            .Returns(_address);

        var authenticationServiceMock = new Mock<IAuthenticationService>();
        authenticationServiceMock
            .Setup(t =>
                t.IsAuthenticated(It.IsAny<string>())
            )
            .Returns(true);

        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IVehiclePushService, VehiclePushService>();
                    services.AddSingleton(googleApisClientMock.Object);
                    services.AddSingleton(authenticationServiceMock.Object);
                });
            });

        _httpClient = application.CreateClient();
    }
    public static StringContent CreateContent(object contentAsObj) =>
        new StringContent(JsonConvert.SerializeObject(contentAsObj), Encoding.UTF8, "application/json");
    private async Task<List<string>> SendRequest(VehiclePush input)
    {
        using var message = new HttpRequestMessage(HttpMethod.Post, UrlTrip);
        message.Content = CreateContent(input);
        var response = await _httpClient.SendAsync(message);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

        var result = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<string>>(result);
    }

    [Test]
    public async Task WHEN_analysis_vehiclePush_without_vin_THEN_return_Error()
    {
        var input = _fixture.Build<VehiclePush>().With(t => t.Vin, "").Create();
        var actual = await SendRequest(input);
        Assert.That(actual.Count, Is.EqualTo(1));
        Assert.That(actual.First(), Is.EqualTo(ErrorCodes.VehiclepushVinDoesNotHaveValue));
    }


    [Test]
    public async Task WHEN_analysis_vehiclePush_without_Data_THEN_return_Error()
    {
        var input = _fixture.Build<VehiclePush>().With(t => t.Data, new List<VehiclePushDataPoint>()).Create();
        var actual = await SendRequest(input);
        Assert.That(actual.Count, Is.EqualTo(1));
        Assert.That(actual.First(), Is.EqualTo(ErrorCodes.VehiclepushDataIsLessThan));
    }

    [Test]
    public async Task WHEN_analysis_VehiclePush_data_without_timestamp_positionlong_positionlat_THEN_return_Errors()
    {
        var input =
            _fixture
                .Build<VehiclePush>()
                .Create();
        input.Data.Add(new()
        {
            Odometer = 12
        });
        var actual = await SendRequest(input);
        Assert.That(actual.Count, Is.EqualTo(3));
        Assert.True(actual.Any(t => t == ErrorCodes.VehiclepushDataTimestampDoesNotHaveValue));
        Assert.True(actual.Any(t => t == ErrorCodes.VehiclepushDataPositionlongDoesNotHaveValue));
        Assert.True(actual.Any(t => t == ErrorCodes.VehiclepushDataPositionlatDoesNotHaveValue));
    }

}
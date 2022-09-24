using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Moq;
using TripAnalyzer.Api.Models.Responses;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TripAnalyzer.Api.Middleware;
using System.Text;
using Domain;
using Domain.Service;
using TripAnalyzer.Api.Models.Requests;

namespace TripAnalyzer.Api.Integration.Test;

public class TripControllerTest
{
    private const string UrlTrip = "/v1/trip";

    private Fixture _fixture;
    private HttpClient _httpClient;
    private string _address = "Stuttgart";

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        var googleApisClientMock = new Mock<IGoogleApiClient>() ;
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

    [Test]
    public async Task WHEN_Post_vehiclePush_THEN_return_correct_result()
    {
        var input = _fixture.Create<VehiclePush>();

        using var message = new HttpRequestMessage(HttpMethod.Post, UrlTrip);
        message.Content = CreateContent(input);

        var response = await _httpClient.SendAsync(message);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();

        var actual = JsonConvert.DeserializeObject<VehiclePushAnalysis>(result);
        Assert.That(actual.Vin, Is.EqualTo(input.Vin));
        Assert.That(actual.Destination, Is.EqualTo(_address));
        Assert.That(actual.Departure, Is.EqualTo(_address));
    }

    //todo add more tests
}
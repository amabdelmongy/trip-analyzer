using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace TripAnalyzer.Api.GoogleApiClient;

public class GoogleApiClient : IGoogleApiClient
{
    private readonly HttpClient _httpClient;
    private readonly GoogleApiConfig _googleApiConfig;

    public GoogleApiClient(
        HttpClient httpClient,
        IOptions<GoogleApiConfig> googleApiConfig
        )
    {
        _httpClient = httpClient;
        _googleApiConfig = googleApiConfig?.Value ?? throw new ArgumentNullException(nameof(googleApiConfig));
    }
    public string? GetAddress(float positionLat, float positionLong)
    {
        var url = $"{_googleApiConfig.googleApiUrl}latlng={positionLat},{positionLong}&key={_googleApiConfig.GoogleApiKey}";
        var response = string.Empty;
        using (var client = new HttpClient())
        {
            var result = _httpClient.GetAsync(url).Result;
            if (result.IsSuccessStatusCode)
            {
                response = result.Content.ReadAsStringAsync().Result;
                var googleApiDto = JsonConvert.DeserializeObject<GoogleApiDto>(response);
                return googleApiDto?.results.FirstOrDefault()?.formatted_address;
            }
        }
        return response;
    }
}

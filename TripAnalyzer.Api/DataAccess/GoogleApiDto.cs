namespace TripAnalyzer.Api.DataAccess;

//todo
// use api-client-library from Google https://developers.google.com/api-client-library/dotnet/get_started
public class GoogleApiDto
{
    public List<Result> results { get; set; }
    public string status { get; set; }
}


public class Result
{
    public string formatted_address { get; set; }
}
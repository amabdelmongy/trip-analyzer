using System.Net.Http.Headers;
using Serilog;
using TripAnalyzer.Api.DataAccess;
using TripAnalyzer.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IVehiclePushAnalysisService, VehiclePushAnalysisService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddHttpClient<IGoogleApisClient, GoogleApisClient>(
        httpClient =>
        {
            httpClient.Timeout = Timeout.InfiniteTimeSpan;
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        });
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddOptions();
builder.Services.Configure<GoogleApiConfig>(builder.Configuration.GetSection("GoogleApiConfig"));

// remove default logging providers
builder.Logging.ClearProviders(); 
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog(logger);

var app = builder.Build();
app.MapHealthChecks("/health");
app.UseMiddleware<ApiKeyAuthenticationMiddleware>();
app.MapControllers();
app.Logger.LogInformation("The application started");
app.Run();
public partial class Program { }
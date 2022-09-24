using System.Net.Http.Headers;
using Serilog;
using TripAnalyzer.Api.GoogleApiClient;
using TripAnalyzer.Api.Middleware;
using TripAnalyzer.Api.Models.Requests.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IVehiclePushService, VehiclePushService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddHttpClient<IGoogleApiClient, GoogleApiClient>(
        httpClient =>
        {
            httpClient.Timeout = Timeout.InfiniteTimeSpan;
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        });
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddOptions();
builder.Services.Configure<GoogleApiConfig>(builder.Configuration.GetSection("GoogleApiConfig"));
builder.Services.AddTransient<IValidator<VehiclePush>, RequestValidator>();
builder.Services.AddSwaggerGen();
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
// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Trip API V1");
});

app.Logger.LogInformation("The application started");
app.Run();
namespace TripAnalyzer.Api
{
    public partial class Program { }
}
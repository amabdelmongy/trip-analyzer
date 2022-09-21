using Microsoft.AspNetCore.Mvc;
using TripAnalyzer.Api.Models.Requests;

namespace TripAnalyzer.Api.Controller.v1;

[ApiController]
[Route("v1/trip")]
public class TripController : ControllerBase
{
    private readonly ILogger<TripController> _logger;
    private readonly IVehiclePushAnalysisService _vehiclePushAnalysisService;

    public TripController(
        ILogger<TripController> logger,
        IVehiclePushAnalysisService vehiclePushAnalysisService)
    {
        _logger = logger;
        _vehiclePushAnalysisService = vehiclePushAnalysisService;
    }

    /// <summary>
    /// Analyze a vehicle trip
    /// this endpoints gets a list of data points from a vehicle. the whole list represents a trip from one location to another with several stops to refuel or just to eat some cookies.
    /// </summary>
    [HttpPost]
    public ActionResult post(VehiclePush vehiclePush)
    {
        var result = _vehiclePushAnalysisService.Analysis(vehiclePush);

        if (result.HasErrors)
            return new BadRequestObjectResult(
                result.Errors
                    .Select(error => new
                    {
                        subject = error.Subject
                    }));

        return Ok(result.Value);
    }
}
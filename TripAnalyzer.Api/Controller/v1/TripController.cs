using Domain.Service;
using Microsoft.AspNetCore.Mvc;
using TripAnalyzer.Api.Models.Responses;

namespace TripAnalyzer.Api.Controller.v1;

[ApiController]
[Route("v1/trip")]
public class TripController : ControllerBase
{
    private readonly IVehiclePushService _vehiclePushService;

    public TripController(IVehiclePushService vehiclePushService)
    {
        _vehiclePushService = vehiclePushService;
    }

    /// <summary>
    /// Analyze a vehicle trip
    /// this endpoints gets a list of data points from a vehicle. the whole list represents a trip from one location to another with several stops to refuel or just to eat some cookies.
    /// </summary>
    [HttpPost]
    public ActionResult post(VehiclePush vehiclePush)
    {
        var result = _vehiclePushService.Analysis(vehiclePush);

        if (result.HasErrors)
            return new BadRequestObjectResult(
                result.Errors
                    .Select(error => new
                    {
                        subject = error.Message
                    }));

        var aggregate = result.Value;

        return Ok(
            new VehiclePushAnalysis
            {
                Vin = aggregate!.Vin,
                Consumption = aggregate.Consumption,
                Departure = aggregate.Departure,
                Destination = aggregate.Destination,
                Breaks =
                    aggregate.Breaks.Select(t =>
                        new Break(
                            positionLat: t.PositionLat,
                            positionLong: t.PositionLong,
                            startTimestamp: t.StartTimestamp,
                            endTimestamp: t.EndTimestamp)).ToList(),
                RefuelStops =
                    aggregate.RefuelStops.Select(t =>
                        new Break(
                            positionLat: t.PositionLat,
                            positionLong: t.PositionLong,
                            startTimestamp: t.StartTimestamp,
                            endTimestamp: t.EndTimestamp)).ToList()
            }
            );
    }
}
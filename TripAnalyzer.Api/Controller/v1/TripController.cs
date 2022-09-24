using Domain;
using Domain.Aggregate;
using Domain.Service;
using Microsoft.AspNetCore.Mvc;
using TripAnalyzer.Api.Models.Requests;
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
        var errors = ValidateVehiclePush(vehiclePush);
        if (errors.Any())
            return new BadRequestObjectResult(
                errors
                    .Select(error => new
                    {
                        error.Message
                    }));

        var result = _vehiclePushService.Analysis(
            vehiclePush.Vin,
            vehiclePush.BreakThreshold,
            vehiclePush.GasTankSize,
            vehiclePush.Data.Select(
                t => new DataPoint(
                    t.Odometer,
                    t.PositionLat,
                    t.PositionLong,
                    t.Timestamp,
                    t.FuelLevel
                )).ToList());


        if (result.HasErrors)
            return new BadRequestObjectResult(
                result.Errors
                    .Select(error => new
                    {
                        error.Message
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
    private IEnumerable<Error> ValidateVehiclePush(VehiclePush vehiclePush)
    {
        var errors = new List<Error>();

        if (vehiclePush == null)
        {
            errors.Add(Error.CreateFrom(ErrorCodes.VehiclepushIsNull));
            return errors;
        }

        if (string.IsNullOrEmpty(vehiclePush.Vin))
            errors.Add(Error.CreateFrom(ErrorCodes.VehiclepushVinDoesNotHaveValue));

        if (vehiclePush.Data.Count < 1)
            errors.Add(Error.CreateFrom(ErrorCodes.VehiclepushDataIsLessThan));


        foreach (var data in vehiclePush.Data)
        {
            if (!data.PositionLat.HasValue)
                errors.Add(Error.CreateFrom(ErrorCodes.VehiclepushDataPositionlatDoesNotHaveValue));


            if (!data.PositionLong.HasValue)
                errors.Add(Error.CreateFrom(ErrorCodes.VehiclepushDataPositionlongDoesNotHaveValue));


            if (!data.Timestamp.HasValue)
                errors.Add(Error.CreateFrom(ErrorCodes.VehiclepushDataTimestampDoesNotHaveValue));
        }
        return errors;
    }
}
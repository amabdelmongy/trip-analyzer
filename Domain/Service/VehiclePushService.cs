using TripAnalyzer.Api.Models.Requests;

namespace Domain.Service;

public class VehiclePushService : IVehiclePushService
{
    private readonly IGoogleApiClient _googleApiClient;

    public VehiclePushService(IGoogleApiClient googleApiClient)
    {
        _googleApiClient = googleApiClient;
    }

    public Result<VehiclePushAnalysisAggregate?> Analysis(VehiclePush vehiclePush)
    {
        var errors = ValidateVehiclePush(vehiclePush);
        if (errors.Any())
            return Result.Failed<VehiclePushAnalysisAggregate>(errors)!;

        var data = vehiclePush.Data.OrderBy(t => t.Timestamp).ToList();
        var departureData = data.First();
        var destinationData = data.Last();

        return Result.Ok(
            new VehiclePushAnalysisAggregate(
                vin: vehiclePush.Vin,
                departure: _googleApiClient.GetAddress(
                    departureData.PositionLat!.Value,
                    departureData.PositionLong!.Value)!,
                destination: _googleApiClient.GetAddress(
                    destinationData.PositionLat!.Value,
                    destinationData.PositionLong!.Value)!,
                refuelStops: GetRefuelStops(data),
                consumption: null,
                breaks: GetBreaks(data)
            ));
    }

    private List<BreakAggregate> GetRefuelStops(List<VehiclePushDataPoint> data)
    {
        var dataGrouped =
            data
                .GroupBy(x =>
                    new
                    {
                        x.PositionLat,
                        x.PositionLong,
                        x.Odometer
                    }, (key, group) =>
                    new
                    {
                        key,
                        group
                    })
                .Where(t => t.group.Count() > 1);

        var dataWithDifferentFuelLevel =
            dataGrouped
                .Select(t =>
                    new
                    {
                        t.key.PositionLat,
                        t.key.PositionLong,
                        StartTimestamp = t.group.First().Timestamp,
                        EndTimestamp = t.group.Last().Timestamp,
                        FuelLevelFirst = t.group.First().FuelLevel,
                        FuelLevelLast = t.group.Last().FuelLevel

                    })
                .Where(t => t.FuelLevelLast > t.FuelLevelFirst);

        return dataWithDifferentFuelLevel
            .Select(t =>
                new BreakAggregate(positionLat: t.PositionLat, positionLong: t.PositionLong,
                    startTimestamp: t.StartTimestamp, endTimestamp: t.EndTimestamp)).ToList();
    }

    private List<BreakAggregate> GetBreaks(List<VehiclePushDataPoint> data) =>
        data
            .GroupBy(x =>
                new
                {
                    x.PositionLat,
                    x.PositionLong,
                    x.Odometer
                }, (key, group) =>
                new
                {
                    key,
                    group
                })
            .Where(t => t.group.Count() > 1)
            .Select(t =>
                new BreakAggregate(positionLat: t.key.PositionLat, positionLong: t.key.PositionLong,
                    startTimestamp: t.group.First().Timestamp, endTimestamp: t.group.Last().Timestamp))
            .ToList();

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

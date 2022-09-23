using TripAnalyzer.Api.Models.Requests;

namespace TripAnalyzer.Api.Service;

public class VehiclePushAnalysisService : IVehiclePushAnalysisService
{
    private readonly IGoogleApisClient _googleApisClient;

    public VehiclePushAnalysisService(IGoogleApisClient googleApisClient)
    {
        this._googleApisClient = googleApisClient;
    }

    public Result<VehiclePushAnalysis> Analysis(VehiclePush vehiclePush)
    {
        var errors = ValidateVehiclePush(vehiclePush);
        if (errors.Any())
            return Result.Failed<VehiclePushAnalysis>(errors);

        var data = vehiclePush.Data.OrderBy(t => t.Timestamp).ToList();
        var departureData = data.First();
        var destinationData = data.Last();

        return Result.Ok(new VehiclePushAnalysis
        {
            Vin = vehiclePush.Vin,
            Departure = _googleApisClient.GetAddress(departureData.PositionLat.Value, departureData.PositionLong.Value),
            Destination =
                _googleApisClient.GetAddress(destinationData.PositionLat.Value, destinationData.PositionLong.Value),
            RefuelStops = GetRefuelStops(data),
            Breaks = GetBreaks(data)
        });
    }

    private List<Break> GetRefuelStops(List<VehiclePushDataPoint> data)
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
                .Where(t => t.FuelLevelLast > t.FuelLevelFirst).ToList();

        return dataWithDifferentFuelLevel
            .Select(t =>
                new Break
                {
                    PositionLat = t.PositionLat,
                    PositionLong = t.PositionLong,
                    StartTimestamp = t.StartTimestamp,
                    EndTimestamp = t.EndTimestamp
                }).ToList();
    }

    private List<Break> GetBreaks(List<VehiclePushDataPoint> data) =>
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
                new Break
                {
                    PositionLat = t.key.PositionLat,
                    PositionLong = t.key.PositionLong,
                    StartTimestamp = t.group.First().Timestamp,
                    EndTimestamp = t.group.Last().Timestamp
                })
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

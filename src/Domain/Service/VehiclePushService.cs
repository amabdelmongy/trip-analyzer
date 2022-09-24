namespace Domain.Service;

public class VehiclePushService : IVehiclePushService
{
    private readonly IGoogleApiClient _googleApiClient;

    public VehiclePushService(IGoogleApiClient googleApiClient)
    {
        _googleApiClient = googleApiClient;
    }

    public Result<VehiclePushAnalysisAggregate?> Analysis(
        string vin,
        int? breakThreshold,
        int? gasTankSize,
        List<DataPoint> data)
    {


        var aggregate = new VehiclePushAnalysisAggregate(
            vin,
            breakThreshold,
            gasTankSize,
            data
        );
        aggregate.Departure = _googleApiClient.GetAddress(
            aggregate.DepartureData.PositionLat!.Value,
            aggregate.DepartureData.PositionLong!.Value)!;

        aggregate.Destination = _googleApiClient.GetAddress(
            aggregate.DestinationData.PositionLat!.Value,
            aggregate.DestinationData.PositionLong!.Value)!;

        return Result.Ok(aggregate);
    }

}

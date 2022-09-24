namespace Domain.Service;

public interface IVehiclePushService
{
    Result<VehiclePushAnalysisAggregate?> Analysis(
        string vin,
        int? breakThreshold,
        int? gasTankSize,
        List<DataPoint> data);
}
namespace Domain.Service;

public interface IVehiclePushService
{
    Result<VehiclePushAnalysisAggregate?> Analysis(VehiclePush vehiclePush);
}
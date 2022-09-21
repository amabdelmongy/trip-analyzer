using TripAnalyzer.Api.Models.Requests;

namespace TripAnalyzer.Api.Service;

public interface IVehiclePushAnalysisService
{
    Result<VehiclePushAnalysis> Analysis(VehiclePush vehiclePush);
}
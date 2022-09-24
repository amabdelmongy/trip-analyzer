using FluentValidation.AspNetCore;

namespace TripAnalyzer.Api.Models.Requests.Validators;

public class VehiclePushDataPointValidator : AbstractValidator<VehiclePushDataPoint>
{
    public VehiclePushDataPointValidator()
    {
        RuleFor(x => x.PositionLong)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull()
                .WithErrorCode(ErrorCodes.VehiclepushDataPositionlongDoesNotHaveValue)
            .GreaterThanOrEqualTo(1)
                .WithErrorCode(ErrorCodes.VehiclepushDataPositionlongDoesNotHaveValue);

        RuleFor(x => x.PositionLat)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull()
            .WithErrorCode(ErrorCodes.VehiclepushDataPositionlatDoesNotHaveValue)
            .GreaterThanOrEqualTo(1)
            .WithErrorCode(ErrorCodes.VehiclepushDataPositionlatDoesNotHaveValue);

        RuleFor(x => x.Timestamp)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull()
            .WithErrorCode(ErrorCodes.VehiclepushDataTimestampDoesNotHaveValue)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.VehiclepushDataPositionlatDoesNotHaveValue);

        RuleFor(x => x.Odometer).GreaterThanOrEqualTo(0);
        RuleFor(x => x.FuelLevel).GreaterThanOrEqualTo(0).LessThanOrEqualTo(100);
    }
}
 
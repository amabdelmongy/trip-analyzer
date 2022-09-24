namespace TripAnalyzer.Api.Models.Requests.Validators;

public class RequestValidator : AbstractValidator<VehiclePush>
{
    public RequestValidator()
    {
        RuleFor(x => x.Vin)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull()
                .WithErrorCode(ErrorCodes.VehiclepushVinDoesNotHaveValue)
            .NotEmpty()
                .WithErrorCode(ErrorCodes.VehiclepushVinDoesNotHaveValue);

        RuleFor(x => x.Data)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull()
                .WithErrorCode(ErrorCodes.VehiclepushDataIsLessThan);

        RuleFor(x => x.Data.Count)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .GreaterThanOrEqualTo(2)
                .WithErrorCode(ErrorCodes.VehiclepushDataIsLessThan);

        RuleForEach(x => x.Data).SetValidator(new VehiclePushDataPointValidator());


        RuleFor(x => x.GasTankSize).GreaterThanOrEqualTo(1);
        RuleFor(x => x.BreakThreshold).GreaterThanOrEqualTo(0);

    }
}

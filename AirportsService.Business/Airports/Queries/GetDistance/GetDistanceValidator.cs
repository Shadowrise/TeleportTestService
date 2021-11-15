using FluentValidation;

namespace AirportsService.Business.Airports.Queries.GetDistance;

public class GetDistanceValidator: AbstractValidator<GetDistance>
{
    public GetDistanceValidator()
    {
        RuleFor(x => x.Code1).NotEmpty().Matches("[A-Z]{3}");
        RuleFor(x => x.Code2).NotEmpty().Matches("[A-Z]{3}");
    }
}
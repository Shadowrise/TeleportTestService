using MediatR;

namespace AirportsService.Business.Airports.Queries.GetDistance;

public class GetDistance: IRequest<double>
{
    public string Code1 { get; set; }
    public string Code2 { get; set; }
}
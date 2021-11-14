using System.ComponentModel.DataAnnotations;
using AirportsService.Business.Airports.Queries.GetDistance;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TeleportTestService.Controllers;

/// <summary>
/// Airports API
/// </summary>
[ApiController]
[Route("airports")]
public class AirportsController : ControllerBase
{
    private IMediator _mediator { get; }
    
    /// <summary>
    /// Airports API
    /// </summary>
    /// <param name="mediator">Instance of <see cref="IMediator"/></param>
    public AirportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns distance in miles between two airports
    /// </summary>
    /// <param name="code1">IATA code of the first airport</param>
    /// <param name="code2">IATA code of the second airport</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("distance")]
    public async Task<double> GetDistance(
        [FromQuery, Required] string code1, 
        [FromQuery, Required] string code2,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetDistance()
        {
            Code1 = code1,
            Code2 = code2
        }, cancellationToken);

        return result;
    }
}
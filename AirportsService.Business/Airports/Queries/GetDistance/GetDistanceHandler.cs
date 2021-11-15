using System.Text.Json;
using GeoCoordinatePortable;
using MediatR;
using Microsoft.Extensions.Logging;
using TeleportPlacesClient;

namespace AirportsService.Business.Airports.Queries.GetDistance;

public class GetDistanceHandler: IRequestHandler<GetDistance, double>
{
    private ILogger<GetDistanceHandler> _logger;
    private ITeleportPlacesClient _teleportPlacesClient;

    public GetDistanceHandler(ILogger<GetDistanceHandler> logger, ITeleportPlacesClient teleportPlacesClient)
    {
        _logger = logger;
        _teleportPlacesClient = teleportPlacesClient;
    }

    public async Task<double> Handle(GetDistance request, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(new Dictionary<string, object>()
               {
                   ["QueryPayload"] = JsonSerializer.Serialize(request)
               }))
        {
            _logger.LogInformation("Start processing {QueryName} query for airports {Code1} and {Code2}", 
                nameof(GetDistance), request.Code1, request.Code2);    
        }

        var airportInfoRequest1 = _teleportPlacesClient.GetAirportInfo(request.Code1, cancellationToken);
        var airportInfoRequest2 = _teleportPlacesClient.GetAirportInfo(request.Code2, cancellationToken);
        await Task.WhenAll(airportInfoRequest1, airportInfoRequest2);
        
        _logger.LogDebug("First airport: {@AirportInfo}", airportInfoRequest1.Result); 
        _logger.LogDebug("Second airport: {@AirportInfo}", airportInfoRequest2.Result); 
        
        var distanceInMeters = new GeoCoordinate(airportInfoRequest1.Result.Location.Lat, airportInfoRequest1.Result.Location.Lon)
            .GetDistanceTo(new GeoCoordinate(airportInfoRequest2.Result.Location.Lat, airportInfoRequest2.Result.Location.Lon));
        var distanceInMiles = distanceInMeters * 0.000621D;
        _logger.LogDebug("Calculated distance is {DistanceInMiles:0.00} miles ({DistanceInMeters:0.00} meters)", distanceInMiles, distanceInMeters); 
        
        return distanceInMiles;
    }
    
}
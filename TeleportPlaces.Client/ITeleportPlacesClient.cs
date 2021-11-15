using RestEase;
using TeleportPlacesClient.Dtos;

namespace TeleportPlacesClient;

public interface ITeleportPlacesClient
{
    [Get("airports/{code}")]
    Task<AirportInfoDto> GetAirportInfo([Path] string code, CancellationToken cancellationToken);
}
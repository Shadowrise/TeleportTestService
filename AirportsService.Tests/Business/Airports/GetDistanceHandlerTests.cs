using System.Threading;
using System.Threading.Tasks;
using AirportsService.Business.Airports.Queries.GetDistance;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TeleportPlacesClient;
using TeleportPlacesClient.Dtos;
using Xunit;

namespace AirportsService.Tests.Business.Airports;

public class GetDistanceHandlerTests
{
    [Theory]
    [InlineData("LED", 59.800278D, 30.262500D, "LED", 59.800278D, 30.262500D, 0.0D)]
    [InlineData("LED", 59.800278D, 30.262500D, "VKO", 55.591389D, 37.261389D, 389.585D)]
    [InlineData("OKI", 36.180833D, 133.324722D, "GKK", 0.732222D, 73.433333D, 4534.664D)]
    [InlineData("BLW", 4.766944D, 45.238611D, "VUP", 10.435000D, -73.249444D, 8086.139D)]
    public async Task GetDistanceHandler_CalculatesDistanceCorrectly(
        string code1,
        double lat1,
        double lon1,
        string code2,
        double lat2,
        double lon2,
        double expectedDistance
        )
    {
        var teleportPlacesClientMock = new Mock<ITeleportPlacesClient>();
        teleportPlacesClientMock.Setup(
                x => x.GetAirportInfo(
                    It.Is<string>(v => v == code1),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AirportInfoDto()
            {
                Location = new LocationDto()
                {
                    Lon = lon1,
                    Lat = lat1,
                }
            });
        teleportPlacesClientMock.Setup(
                x => x.GetAirportInfo(
                    It.Is<string>(v => v == code2),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AirportInfoDto()
            {
                Location = new LocationDto()
                {
                    Lon = lon2,
                    Lat = lat2,
                }
            });
        var logger = new Mock<ILogger<GetDistanceHandler>>();
        var handler = new GetDistanceHandler(logger.Object, teleportPlacesClientMock.Object);
        var query = new GetDistance()
        {
            Code1 = code1,
            Code2 = code2,
        };
        
        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeApproximately(expectedDistance, expectedDistance * 0.01D);
    }
}
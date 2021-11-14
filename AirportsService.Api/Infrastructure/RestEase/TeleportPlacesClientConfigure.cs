using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Polly;
using RestEase.HttpClientFactory;
using TeleportPlacesClient;

namespace TeleportTestService.Infrastructure.RestEase;

public static class TeleportPlacesClientConfigure
{
    public static void AddTeleportPlacesClient(this WebApplicationBuilder builder)
    {
        var baseAddress = builder.Configuration["TeleportPlacesClient:Url"];
        builder.Services.AddRestEaseClient<ITeleportPlacesClient>(baseAddress, x => x.JsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        }).AddTransientHttpErrorPolicy(x => x.WaitAndRetryAsync(new[]
        {
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(5),
            TimeSpan.FromSeconds(10)
        }));
    }
}
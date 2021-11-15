using Microsoft.AspNetCore.Mvc;

namespace TeleportTestService.Infrastructure.Caching;

public static class ResponseCacheConfigure
{
    public static void AddCustomResponseCache(this IServiceCollection services)
    {
        services.AddResponseCaching((options) =>
        {
            options.MaximumBodySize = 1024;
            options.UseCaseSensitivePaths = true;
        });
    }
    
    public static void AddCustomDefaultCacheProfile(this MvcOptions options)
    {
        options.CacheProfiles.Add(CacheProfiles.Default, new CacheProfile()
        {
            Duration = 300,
            Location = ResponseCacheLocation.Any,
            VaryByQueryKeys = new[] {"*"}
        });
    }
}
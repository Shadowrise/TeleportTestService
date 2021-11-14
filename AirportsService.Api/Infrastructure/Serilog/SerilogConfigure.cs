using Serilog;

namespace TeleportTestService.Infrastructure.Serilog;

public static class SerilogConfigure
{
    public static void AddCustomSerilog(this IHostBuilder host)
    {
        host.UseSerilog((ctx, lc) => lc
            .ReadFrom.Configuration(ctx.Configuration));
    }

    public static void UseCustomSerilog(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();
    }
}
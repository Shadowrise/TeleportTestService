using AirportsService.Business.Airports.Queries.GetDistance;
using FluentValidation.AspNetCore;
using MediatR;
using TeleportTestService.Infrastructure.Caching;
using TeleportTestService.Infrastructure.ExceptionHandling;
using TeleportTestService.Infrastructure.RestEase;
using TeleportTestService.Infrastructure.Serilog;
using TeleportTestService.Infrastructure.Swagger;
using TeleportTestService.Infrastructure.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddCustomSerilog();
builder.Services.AddCustomSwagger();
builder.Services.AddCustomResponseCache();

builder.Services.AddControllers(x =>
{
    x.Filters.Add<ExceptionFilter>();
    x.AddCustomDefaultCacheProfile();
}).ConfigureApiBehaviorOptions(options => {
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddMediatR(typeof(GetDistance).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<GetDistance>());

builder.AddTeleportPlacesClient();

var app = builder.Build();

app.UseCustomSerilog();
app.UseCustomSwagger();

app.UseResponseCaching();
app.UseRouting();
app.UseEndpoints(x => x.MapControllers());

app.Run();
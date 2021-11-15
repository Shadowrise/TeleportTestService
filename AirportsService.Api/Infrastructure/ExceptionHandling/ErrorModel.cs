using TeleportTestService.Infrastructure.ExceptionHandling;

namespace TeleportTestService.Infrastructure;

public class ErrorModel
{
    public string Code { get; set; }
    public string Message { get; set; }
    public IEnumerable<ValidationErrorModel> ValidationErrors { get; set; }
    public DateTime ServerTime { get; set; }
    public string Stack { get; set; }
}
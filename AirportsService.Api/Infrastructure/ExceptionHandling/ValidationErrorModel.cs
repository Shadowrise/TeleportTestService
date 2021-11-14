namespace TeleportTestService.Infrastructure.ExceptionHandling;

public class ValidationErrorModel
{
    public string PropertyName { get; set; }
    public string ErrorMessage { get; set; }
    public object AttemptedValue { get; set; }
}
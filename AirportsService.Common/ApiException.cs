namespace AirportsService.Common;

public class ApiException : Exception
{
    public string ErrorCode { get; set; }
    
    public string ErrorMessage { get; set; }

    public ApiException(Exception ex, string errorCode, params object[] errorVars): base(string.Empty, ex)
    {
        SetError(errorCode, errorVars);
    }
    
    public ApiException(string errorCode, params object[] errorVars): base(string.Empty)
    {
        SetError(errorCode, errorVars);
    }

    private void SetError(string errorCode, params object[] errorVars)
    {
        ErrorCode = errorCode;
        ErrorMessage = string.Format(Errors.ResourceManager.GetString(errorCode), errorVars);
    }
}
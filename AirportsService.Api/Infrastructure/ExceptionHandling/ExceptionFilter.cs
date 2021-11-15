using System.Net;
using System.Text.Json;
using AirportsService.Common;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TeleportTestService.Infrastructure.ExceptionHandling;

public class ExceptionFilter : ExceptionFilterAttribute
{
    private ILogger<ExceptionFilter> _logger;

    private IWebHostEnvironment _hostEnvironment;

    public ExceptionFilter(ILogger<ExceptionFilter> logger, IWebHostEnvironment hostEnvironment)
    {
        _logger = logger;
        _hostEnvironment = hostEnvironment;
    }

    public override void OnException(ExceptionContext context)
    {
        var errorModel = new ErrorModel
        {
            ServerTime = DateTime.UtcNow
        };
        int statusCode;
        
        if (context.Exception is ApiException apiException)
        {
            statusCode = (int)HttpStatusCode.BadRequest;
            errorModel.Code = apiException.ErrorCode;
            errorModel.Message = apiException.ErrorMessage;
        }
        else if (context.Exception is ValidationException validationException)
        {
            statusCode = (int)HttpStatusCode.UnprocessableEntity;
            errorModel.Code = nameof(Errors.ER0002);
            errorModel.Message = Errors.ER0002;
            errorModel.ValidationErrors = validationException.Errors.Select(x => new ValidationErrorModel()
            {
                PropertyName = x.PropertyName,
                ErrorMessage = x.ErrorMessage,
                AttemptedValue = x.AttemptedValue
            });
        }
        else
        {
            statusCode = (int)HttpStatusCode.InternalServerError;
            errorModel.Code = nameof(Errors.ER0001);
            errorModel.Message =_hostEnvironment.IsProduction() ? Errors.ER0001: context.Exception.Message;
        }

        if (!_hostEnvironment.IsProduction())
        {
            errorModel.Stack = context.Exception.StackTrace;   
        }
        
        using (_logger.BeginScope(new Dictionary<string, object>()
               {
                   ["ErrorCode"] = errorModel.Code,
                   ["ErrorPayload"] = JsonSerializer.Serialize(errorModel)
               }))
        {
            _logger.LogError(context.Exception, "{ErrorCode}: {Message}", errorModel.Code, errorModel.Message);   
        }

        context.HttpContext.Response.StatusCode = statusCode;
        context.Result = new JsonResult(errorModel);

        base.OnException(context);
    }
}
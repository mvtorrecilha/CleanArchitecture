using Domain.Abstractions;
using Domain.Abstractions.ErrorData;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

public class BaseController : Controller
{
    #region Public Constructors

    public BaseController()
    {
    }

    #endregion Public Constructors

    #region Protected Methods

    protected IActionResult FluentResult(ResultBase result)
    {
        IEnumerable<ErrorBaseData> errors = result.Errors
            .Cast<ErrorBase>()
            .Select(e => new ErrorBaseData
            {
                Message = e.Message,
                Details = e.Reasons?.Select(r => r.Message),
                Code = e.Code,
                Data = e.Data
            });

        ErrorBase firstResultBase = (ErrorBase)result.Errors.First();
        int statusCode = (int)GetStatusCode(firstResultBase);

        ResponseErrorBaseData responseErrorBase = new()
        {
            Errors = errors
        };

        return new ObjectResult(responseErrorBase)
        {
            StatusCode = statusCode
        };
    }

    #endregion Protected Methods

    #region Private Methods

    private static HttpStatusCode GetStatusCode(ErrorBase errorBase) =>

        errorBase.ErrorType switch
        {
            ErrorType.Validation => HttpStatusCode.BadRequest,
            ErrorType.NotFound => HttpStatusCode.NotFound,
            ErrorType.Conflict => HttpStatusCode.Conflict,
            ErrorType.Forbidden => HttpStatusCode.Forbidden,
            ErrorType.Unauthorized => HttpStatusCode.Unauthorized,
            _ => HttpStatusCode.InternalServerError
        };

    #endregion Private Methods
}
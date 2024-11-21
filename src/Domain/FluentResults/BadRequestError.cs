using Domain.Abstractions;

namespace Domain.FluentResults;

public static class BadRequestError
{
    public static ErrorBase Conflict(string message) => ErrorBase.Validation(
        "BadRequest", message);
}

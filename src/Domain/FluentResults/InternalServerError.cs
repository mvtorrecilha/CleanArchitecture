using Domain.Abstractions;

namespace Domain.FluentResults;

public static class InternalServerError
{
    public static ErrorBase UnexpectedError(string message) => ErrorBase.Failure(
        "UnexpectedError", message);
}

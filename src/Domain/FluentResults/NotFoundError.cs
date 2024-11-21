using Domain.Abstractions;

namespace Domain.FluentResults;

public static class NotFoundError
{
    public static ErrorBase NotFound(string message) => ErrorBase.NotFound(
        "NotFoundError", message);
}

using Domain.Abstractions;

namespace Domain.FluentResults;

public static class UnauthorizedError
{
    public static ErrorBase Unauthorized(string message) => ErrorBase.Unauthorized(
        "Unauthorized", message);
}

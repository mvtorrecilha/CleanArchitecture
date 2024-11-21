using Domain.Abstractions;

namespace Domain.FluentResults;

public static class ConflictError
{
    public static ErrorBase Conflict(string message) => ErrorBase.Conflict(
        "Conflict", message);
}
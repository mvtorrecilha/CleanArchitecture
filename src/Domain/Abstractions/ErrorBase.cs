using Domain.FluentResults;
using FluentResults;

namespace Domain.Abstractions;

public class ErrorBase : Error
{
    public string Code { get; private set; }

    public ErrorType ErrorType { get; private set; }

    public IErrorData Data { get; private set; }

    public ErrorBase(string code, string message, ErrorType errorType)
        : base(message)
    {
        Code = code;
        ErrorType = errorType;
    }

    /// <summary>
    /// Sets the data of the error.
    /// </summary>
    /// <param name="errorData">Error data. Must create a class and implements the interface.</param>
    /// <returns>ErrorBase like fluent api.</returns>
    public ErrorBase WithData(IErrorData errorData)
    {
        Data = errorData;
        return this;
    }

    public static ErrorBase Failure(string code, string message) =>
        new(code, message, ErrorType.Failure);

    public static ErrorBase Validation(string code, string message) =>
        new(code, message, ErrorType.Validation);

    public static ErrorBase NotFound(string code, string message) =>
        new(code, message, ErrorType.NotFound);

    public static ErrorBase Conflict(string code, string message) =>
        new(code, message, ErrorType.Conflict);

    public static ErrorBase Forbidden(string code, string message) =>
        new(code, message, ErrorType.Forbidden);

    public static ErrorBase Unauthorized(string code, string message) =>
       new(code, message, ErrorType.Unauthorized);
}

public enum ErrorType
{
    Failure = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3,
    Forbidden = 4,
    Unauthorized = 5
}
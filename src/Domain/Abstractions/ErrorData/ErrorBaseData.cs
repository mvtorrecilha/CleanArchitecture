using Domain.FluentResults;

namespace Domain.Abstractions.ErrorData;

public class ErrorBaseData
{
    public string Message { get; set; }

    public IEnumerable<string> Details { get; set; }

    public string Code { get; set; }

    public IErrorData? Data { get; set; }
}
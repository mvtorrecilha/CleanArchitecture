using Domain.FluentResults;

namespace Domain.Abstractions.ErrorData;

public class ResponseErrorBaseData : IErrorData
{
    public IEnumerable<ErrorBaseData> Errors { get; set; }
}
namespace FinalRound.Contracts.Api;

public sealed record ApiError(string Code, string Message);

public sealed record ApiResponse<T>(
    bool Success,
    T? Data,
    IReadOnlyList<ApiError>? Errors,
    string TraceId,
    string CorrelationId
)
{
    public static ApiResponse<T> Ok(T data, string traceId, string correlationId)
        => new(true, data, null, traceId, correlationId);

    public static ApiResponse<T> Fail(IEnumerable<ApiError> errors, string traceId, string correlationId)
        => new(false, default, errors.ToArray(), traceId, correlationId);
}

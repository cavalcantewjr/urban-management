namespace UrbanManagement.Api.Responses;

public record ApiResponse<T>(T? Data, object? Meta, object? Links, ApiError? Error)
{
    public static ApiResponse<T> Success(T data, object? meta = null, object? links = null)
    {
        return new ApiResponse<T>(data, meta, links, null);
    }

    public static ApiResponse<T> Failure(ApiError error, object? meta = null, object? links = null)
    {
        return new ApiResponse<T>(default, meta, links, error);
    }
}

public record ApiError(string Code, string Message, IReadOnlyCollection<string>? Details = null);


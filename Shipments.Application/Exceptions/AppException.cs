namespace Shipments.Application.Exceptions;
public abstract class AppException : Exception
{
    public int StatusCode { get; }
    public int ErrorCode { get; }
    public bool IsAuthenticationFailed { get; }

    protected AppException(
        int statusCode,
        int errorCode,
        string message,
        bool isAuthenticationFailed = false)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        IsAuthenticationFailed = isAuthenticationFailed;
    }
}

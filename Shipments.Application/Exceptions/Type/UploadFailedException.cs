namespace Shipments.Application.Exceptions;

public sealed class UploadFailedException : AppException
{
    /// <summary>
    /// Upload file exception
    /// </summary>
    /// <param name="message"></param>
    public UploadFailedException(string message = "Upload neuspešan.")
        : base(400, 400, message)
    {
    }
}

namespace Shipments.Application.Exceptions;

public sealed class NotFoundException : AppException
{
    /// <summary>
    /// Not found exception
    /// </summary>
    /// <param name="message"></param>
    public NotFoundException(string message = "Pošiljka ne postoji.")
        : base(404, 404, message)
    {
    }
}

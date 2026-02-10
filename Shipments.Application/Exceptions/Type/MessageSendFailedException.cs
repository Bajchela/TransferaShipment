namespace Shipments.Application.Exceptions;

public sealed class MessageSendFailedException : AppException
{
    /// <summary>
    /// Message send exception
    /// </summary>
    /// <param name="message"></param>
    public MessageSendFailedException(string message = "Poruka ne može da se pošalje.")
        : base(502, 502, message)
    {
    }
}

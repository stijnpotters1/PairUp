namespace PairUpShared.Middleware.GeneralExceptions;

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException()
    {
    }

    protected AlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public AlreadyExistsException(string? message) : base(message)
    {
    }

    public AlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
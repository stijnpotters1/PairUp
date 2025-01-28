namespace PairUpShared.Middleware.GeneralExceptions;

public class BadRequestException : Exception
{
    public BadRequestException()
    {
    }

    protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public BadRequestException(string? message) : base(message)
    {
    }

    public BadRequestException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
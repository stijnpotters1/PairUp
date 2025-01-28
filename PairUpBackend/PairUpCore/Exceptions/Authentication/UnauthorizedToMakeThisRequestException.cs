namespace PairUpCore.Exceptions.Authentication;

public class UnauthorizedToMakeThisRequestException : UnauthorizedException
{
    public override string Message => "Unauthorized to make this request.";

    public UnauthorizedToMakeThisRequestException()
    {
    }

    protected UnauthorizedToMakeThisRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public UnauthorizedToMakeThisRequestException(string? message) : base(message)
    {
    }

    public UnauthorizedToMakeThisRequestException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
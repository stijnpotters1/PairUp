namespace PairUpCore.Exceptions.Authentication;

public class ForbiddenToMakeThisRequestException : ForbiddenException
{
    public override string Message => "Unauthorized to make this request.";

    public ForbiddenToMakeThisRequestException()
    {
    }

    protected ForbiddenToMakeThisRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ForbiddenToMakeThisRequestException(string? message) : base(message)
    {
    }

    public ForbiddenToMakeThisRequestException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
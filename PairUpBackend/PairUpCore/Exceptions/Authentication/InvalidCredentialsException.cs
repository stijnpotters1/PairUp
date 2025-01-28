namespace PairUpCore.Exceptions.Authentication;

public class InvalidCredentialsException : BadRequestException
{
    public override string Message => "Invalid credentials";

    public InvalidCredentialsException()
    {
    }

    protected InvalidCredentialsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public InvalidCredentialsException(string? message) : base(message)
    {
    }

    public InvalidCredentialsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
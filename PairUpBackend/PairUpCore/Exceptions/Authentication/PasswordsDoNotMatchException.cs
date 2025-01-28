namespace PairUpCore.Exceptions.Authentication;

public class PasswordsDoNotMatchException: BadRequestException
{
    public override string Message => "Passwords do not match.";

    public PasswordsDoNotMatchException()
    {
    }

    protected PasswordsDoNotMatchException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public PasswordsDoNotMatchException(string? message) : base(message)
    {
    }

    public PasswordsDoNotMatchException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
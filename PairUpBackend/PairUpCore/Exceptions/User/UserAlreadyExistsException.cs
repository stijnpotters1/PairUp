namespace PairUpCore.Exceptions.User;

public class UserAlreadyExistsException : AlreadyExistsException
{
    public override string Message => "User already exists.";

    public UserAlreadyExistsException()
    {
    }

    protected UserAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public UserAlreadyExistsException(string? message) : base(message)
    {
    }

    public UserAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
namespace PairUpCore.Exceptions.User;

public class UserNotFoundException : NotFoundException
{
    public override string Message => "User not found.";

    public UserNotFoundException()
    {
    }

    protected UserNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public UserNotFoundException(string? message) : base(message)
    {
    }

    public UserNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
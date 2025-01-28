namespace PairUpCore.Exceptions.Authentication;

public class PasswordMustBeAtLeast12CharactersLongException : BadRequestException
{
    public override string Message => "Password must be at least 12 characters long.";

    public PasswordMustBeAtLeast12CharactersLongException()
    {
    }

    protected PasswordMustBeAtLeast12CharactersLongException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public PasswordMustBeAtLeast12CharactersLongException(string? message) : base(message)
    {
    }

    public PasswordMustBeAtLeast12CharactersLongException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
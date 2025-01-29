namespace PairUpCore.Exceptions.Activity;

public class ActivityIsAlreadyLikedByThisUserException : AlreadyExistsException
{
    public override string Message => "Activity is already liked by this user.";

    public ActivityIsAlreadyLikedByThisUserException()
    {
    }

    protected ActivityIsAlreadyLikedByThisUserException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ActivityIsAlreadyLikedByThisUserException(string? message) : base(message)
    {
    }

    public ActivityIsAlreadyLikedByThisUserException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
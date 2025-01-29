namespace PairUpCore.Exceptions.Activity;

public class ActivityIsNotLikedByThisUserException : NotFoundException
{
    public override string Message => "Activity is not liked by this user.";

    public ActivityIsNotLikedByThisUserException()
    {
    }

    protected ActivityIsNotLikedByThisUserException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ActivityIsNotLikedByThisUserException(string? message) : base(message)
    {
    }

    public ActivityIsNotLikedByThisUserException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
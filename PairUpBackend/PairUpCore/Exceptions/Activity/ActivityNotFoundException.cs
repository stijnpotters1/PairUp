namespace PairUpCore.Exceptions.Activity;

public class ActivityNotFoundException : NotFoundException
{
    public override string Message => "Activity not found.";

    public ActivityNotFoundException()
    {
    }

    protected ActivityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ActivityNotFoundException(string? message) : base(message)
    {
    }

    public ActivityNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
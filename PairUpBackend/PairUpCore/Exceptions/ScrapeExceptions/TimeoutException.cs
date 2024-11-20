namespace PairUpCore.Exceptions.ScrapeExceptions;

public class TimeoutException : Exception
{
    public override string Message => "Timeout Error while scraping activities";
 
    public TimeoutException()
    {
    }

    protected TimeoutException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public TimeoutException(string? message) : base(message)
    {
    }

    public TimeoutException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
namespace PairUpCore.Exceptions.ScrapeExceptions;

public class HttpRequestException : Exception
{
    public override string Message => "HTTP Error while scraping activities";
 
    public HttpRequestException()
    {
    }

    protected HttpRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public HttpRequestException(string? message) : base(message)
    {
    }

    public HttpRequestException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
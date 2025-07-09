namespace AdventureWorks.Service.Exceptions;

public class ForbiddenException : ApiExceptionBase
{
    public ForbiddenException()
        : this("403 Forbidden.")
    {
    }

    public ForbiddenException(Exception innerException)
        : this("403 Forbidden.", innerException)
    {
    }

    public ForbiddenException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
        ErrorCode = (int)Exceptions.ErrorCode.Forbidden;
        StatusCode = HttpStatusCode.Forbidden;
    }
}

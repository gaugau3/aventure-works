namespace AdventureWorks.Service.Exceptions;

public class InternalException : ApiExceptionBase
{
    public InternalException()
        : this("An unexpected error occurred.")
    {
    }

    public InternalException(string message)
        : base(message)
    {
        ErrorCode = (int)Exceptions.ErrorCode.InternalError;
    }
}

namespace AdventureWorks.Service.Exceptions;

public class ParameterInvalidException : ApiExceptionBase
{
    public ParameterInvalidException()
        : this("An error occurred when getting application cache.")
    {
    }

    public ParameterInvalidException(string message)
        : base(message)
    {
        ErrorCode = (int)Exceptions.ErrorCode.InvalidParameters;
        StatusCode = HttpStatusCode.BadRequest;
    }
}

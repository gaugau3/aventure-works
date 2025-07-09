namespace AdventureWorks.Service.Exceptions;

public class ResourceNotFoundException : ApiExceptionBase
{
    public ResourceNotFoundException()
        : this("The requested resource is not found.")
    {
    }

    public ResourceNotFoundException(string message)
        : base(message)
    {
        ErrorCode = (int)Exceptions.ErrorCode.ResourceNotFound;
        StatusCode = HttpStatusCode.NotFound;
    }
}

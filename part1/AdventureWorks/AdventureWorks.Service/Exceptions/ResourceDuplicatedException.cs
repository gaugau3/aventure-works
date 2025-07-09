namespace AdventureWorks.Service.Exceptions;

public class ResourceDuplicatedException : ApiExceptionBase
{
    public ResourceDuplicatedException()
        : this("The resource is duplicated.")
    {
    }

    public ResourceDuplicatedException(string message) : base(message)
    {
        ErrorCode = (int)Exceptions.ErrorCode.ResourceDuplicated;
        StatusCode = HttpStatusCode.Conflict;
    }
}

namespace AdventureWorks.Service.Exceptions;

public class ApiExceptionBase : Exception
{
    public ApiExceptionBase() { }
    public ApiExceptionBase(string message) : base(message) { }
    public ApiExceptionBase(string message, Exception? innerException) : base(message, innerException) { }

    public int ErrorCode { get; set; } = (int)Exceptions.ErrorCode.InternalError;
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;
    public string ToJsonString()
    {
        return string.Format("{{\"ErrorCode\":{0},\"Message\":\"{1}\"}}", ErrorCode, Message);
    }
}

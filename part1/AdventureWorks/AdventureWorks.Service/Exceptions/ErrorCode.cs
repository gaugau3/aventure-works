namespace AdventureWorks.Service.Exceptions;

public enum ErrorCode
{
    InvalidRequest = 10000,
    Unauthorized = 10001,
    InvalidAuthorizationToken = 10002,
    ExpiredAuthorizationToken = 10003,
    ResourceNotFound = 10004,
    ResourceDuplicated = 10005,
    OutofUserSeats = 10010,
    InvalidParameters = 20001,
    InvalidTenantID = 20002,
    InvalidTenantIDandApplicationType = 20003,
    InvalidApplicationType = 20004,
    InternalError = 30001,
    CorruptedTenant = 40001,
    Forbidden = 40002,
}

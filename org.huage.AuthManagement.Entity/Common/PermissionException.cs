namespace org.huage.AuthManagement.Entity.Common;

public class PermissionException : Exception
{
    public int ErrorCode { get; set; }
    
    public PermissionException()
    {
    }

    public PermissionException(string? message) : base(message)
    {
    }

    public PermissionException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public PermissionException(int errorCode)
    {
        ErrorCode = errorCode;
    }
}
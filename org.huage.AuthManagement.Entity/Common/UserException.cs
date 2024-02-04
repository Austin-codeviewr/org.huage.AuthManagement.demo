namespace org.huage.AuthManagement.Entity.Common;

public class UserException : Exception
{
    public int ErrorCode { get; set; }
    
    public UserException()
    {
    }

    public UserException(string? message) : base(message)
    {
    }

    public UserException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public UserException(int errorCode)
    {
        ErrorCode = errorCode;
    }
}
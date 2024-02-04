namespace org.huage.AuthManagement.Entity.Common;

[Serializable]
public class BaseResult<T> 
{
    private int code { get; set; }
    
    private string msg { get; set; }
    
    private T data { get; set; }
    
    
    public static BaseResult<T> success(T data) {
        var result = new BaseResult<T>()
        {
            code = 0,
            msg = "success",
            data = data
        };
        return result;
    }
    
}
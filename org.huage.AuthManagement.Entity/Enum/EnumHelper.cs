namespace org.huage.AuthManagement.Entity.Enum;

public class EnumHelper
{
    public static string GetDescription(ResultStatus status)
    {
        switch (status)
        {
            case ResultStatus.Success:
                return "请求失败";
            case ResultStatus.Error:
                return "请求异常";
            case ResultStatus.Fail:
                return "请求失败";
            default:
                return "未知异常";
        }
    }
}
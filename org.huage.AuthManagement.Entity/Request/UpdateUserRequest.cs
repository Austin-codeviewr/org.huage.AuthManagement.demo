using System.Diagnostics.CodeAnalysis;

namespace org.huage.AuthManagement.Entity.Request;

public class UpdateUserRequest
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string? UserName { get; set; }
    
    /// <summary>
    /// 密码
    /// </summary>
    public string? PassWord { get; set; }
    
    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }
    
    /// <summary>
    /// 邮箱地址
    /// </summary>
    public string? Email { get; set; }
    
    
    public string? Remark { get; set; }
}
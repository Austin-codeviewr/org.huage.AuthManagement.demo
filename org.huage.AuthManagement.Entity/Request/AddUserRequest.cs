using System.ComponentModel.DataAnnotations;
using org.huage.AuthManagement.Entity.Common;

namespace org.huage.AuthManagement.Entity.Request;

public class AddUserRequest : BaseFiled
{
    
    /// <summary>
    /// 用户名
    /// </summary>
    [Required]
    public string UserName { get; set; }
    
    /// <summary>
    /// 密码
    /// </summary>
    [Required]
    public string PassWord { get; set; }
    
    /// <summary>
    /// 手机号
    /// </summary>
    [Required]
    public string Phone { get; set; }
    
    /// <summary>
    /// 邮箱地址
    /// </summary>
    public string Email { get; set; }
}


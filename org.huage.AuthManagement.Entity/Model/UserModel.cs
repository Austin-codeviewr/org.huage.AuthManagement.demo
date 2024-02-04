using org.huage.AuthManagement.Entity.Common;

namespace org.huage.AuthManagement.Entity.Model;

public class UserModel: BaseFiled
{
    public int Id { get; set; }
    
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }
    
    /// <summary>
    /// 密码
    /// </summary>
    public string PassWord { get; set; }
    
    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }
    
    /// <summary>
    /// 邮箱地址
    /// </summary>
    public string Email { get; set; }

}
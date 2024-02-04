using org.huage.AuthManagement.Entity.Model;

namespace org.huage.AuthManagement.Entity.Response;

public class QueryOrganizationUserAndRoleResponse
{
    public List<OrganizationUserAndRole> Users { get; set; } = new List<OrganizationUserAndRole>();
}

public class OrganizationUserAndRole
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

    /// <summary>
    /// 角色集合
    /// </summary>
    public List<RoleModel>? RoleModels { get; set; } = new List<RoleModel>();
}
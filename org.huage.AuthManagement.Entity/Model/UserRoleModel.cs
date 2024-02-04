using org.huage.AuthManagement.Entity.Common;

namespace org.huage.AuthManagement.Entity.Model;

public class UserRoleModel : BaseFiled
{
    
    public int? Id { get; set; }
    /// <summary>
    /// 用户id
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// 角色id
    /// </summary>
    public int RoleId { get; set; }
    
    /// <summary>
    /// 组织id
    /// </summary>
    public int OrganizationId { get; set; }
}
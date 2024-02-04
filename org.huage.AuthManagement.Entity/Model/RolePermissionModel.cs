using org.huage.AuthManagement.Entity.Common;

namespace org.huage.AuthManagement.Entity.Model;

public class RolePermissionModel : BaseFiled
{
    public int? Id { get; set; }
    
    /// <summary>
    /// 角色id
    /// </summary>
    public int RoleId { get; set; }
    
    /// <summary>
    /// 权限id
    /// </summary>
    public int PermissionId { get; set; }
}
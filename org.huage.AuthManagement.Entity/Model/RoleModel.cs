using org.huage.AuthManagement.Entity.Common;

namespace org.huage.AuthManagement.Entity.Model;

public class RoleModel : BaseFiled
{
    
    public int Id { get; set; }
    
    /// <summary>
    /// 角色名称
    /// </summary>
    public string Name { get; set; }
   
}
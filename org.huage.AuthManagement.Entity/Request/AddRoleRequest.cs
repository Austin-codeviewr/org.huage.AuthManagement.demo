using System.ComponentModel.DataAnnotations;
using org.huage.AuthManagement.Entity.Model;

namespace org.huage.AuthManagement.Entity.Request;

public class AddRoleRequest
{
    //新增角色名称
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// 角色ids
    /// </summary>
    public List<int> RoleIds { get; set; } = new List<int>();
    
    public string CreateBy { get; set; }
    
}
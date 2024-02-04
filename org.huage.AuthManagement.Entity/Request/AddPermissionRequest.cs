using System.ComponentModel.DataAnnotations;
using org.huage.AuthManagement.Entity.Common;

namespace org.huage.AuthManagement.Entity.Request;

public class AddPermissionRequest : BaseFiled
{
    /// <summary>
    /// 权限名：如读取，删除，xxx。。
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// 权限描述
    /// </summary>
    public string Description { get; set; }
}
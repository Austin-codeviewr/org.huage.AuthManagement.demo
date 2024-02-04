using org.huage.AuthManagement.Entity.Common;

namespace org.huage.AuthManagement.Entity.Model;

public class PermissionModel : BaseFiled
{
    public int Id { get; set; }
    /// <summary>
    /// 权限名：如读取，删除，xxx。。
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 权限描述
    /// </summary>
    public string Description { get; set; } = "";
}
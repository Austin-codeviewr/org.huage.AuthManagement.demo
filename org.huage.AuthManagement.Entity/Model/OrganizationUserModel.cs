using org.huage.AuthManagement.Entity.Common;

namespace org.huage.AuthManagement.Entity.Model;

public class OrganizationUserModel : BaseFiled
{
    public int Id { get; set; }
    /// <summary>
    /// 组织id
    /// </summary>
    public int OrganizationId { get; set; }
    
    /// <summary>
    /// 用户id
    /// </summary>
    public int UserId { get; set; }
}
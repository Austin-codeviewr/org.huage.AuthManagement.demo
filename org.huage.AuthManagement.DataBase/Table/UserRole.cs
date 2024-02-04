namespace org.huage.AuthManagement.DataBase.Table;

public class UserRole : BaseTable
{
    public int Id { get; set; }
    
    /// <summary>
    /// 用户id
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// 角色id
    /// </summary>
    public int RoleId { get; set; }
    
    /// <summary>
    /// 组织id,标识该用户归属于哪个组织
    /// </summary>
    public int OrganizationId { get; set; }
}
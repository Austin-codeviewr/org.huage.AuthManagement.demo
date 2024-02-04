namespace org.huage.AuthManagement.DataBase.Table;

/// <summary>
/// 一个用户可以归属于多个组织，一个组织也可以有多个用户，细分，明确职责
/// </summary>
public class OrganizationUser : BaseTable
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
namespace org.huage.AuthManagement.DataBase.Table;

public class Permission : BaseTable
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
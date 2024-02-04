namespace org.huage.AuthManagement.DataBase.Table;

public class Role : BaseTable
{
    public int Id { get; set; }
    
    /// <summary>
    /// 角色名称
    /// </summary>
    public string Name { get; set; }
    
}
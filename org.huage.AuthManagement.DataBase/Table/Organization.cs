namespace org.huage.AuthManagement.DataBase.Table;

public class Organization : BaseTable
{
    public int Id { get; set; }

    /// <summary>
    /// 公司名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 公司注册编码，唯一
    /// </summary>
    public string OrgCode { get; set; }
    
    /// <summary>
    /// 系统版本
    /// </summary>
    public string Version { get; set; }
    
}
using System.ComponentModel.DataAnnotations;
using org.huage.AuthManagement.Entity.Common;

namespace org.huage.AuthManagement.Entity.Model;

public class OrganizationModel : BaseFiled
{
    public int Id { get; set; }

    /// <summary>
    /// 公司名称
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// 公司注册编码，唯一
    /// </summary>
    /// 
    [Required]
    public string OrgCode { get; set; }
    
    /// <summary>
    /// 系统版本
    /// </summary>
    public string Version { get; set; }
    
}
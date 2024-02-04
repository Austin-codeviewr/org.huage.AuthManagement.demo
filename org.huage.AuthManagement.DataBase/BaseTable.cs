namespace org.huage.AuthManagement.DataBase;

public class BaseTable
{
    /**
    * 是否删除
    */
    public bool IsDeleted { get; set; }

    /**
     * 备注
     */
    public string Remark { get; set; } = string.Empty;

    /**
     * 创建时间
     */
    public DateTime CreateTime { get; set; } = DateTime.Now;

    /**
     * 更新时间
     */
    public DateTime UpdateTime { get; set; }

    /**
     * 更新人
     */
    public string UpdateBy { get; set; }

    /**
     * 创建人
     */
    public string CreateBy { get; set; }
}
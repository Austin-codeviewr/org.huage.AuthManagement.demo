using org.huage.AuthManagement.Entity.Model;

namespace org.huage.AuthManagement.Entity.Response;

public class QueryAllPermissionResponse
{
    public List<PermissionModel> PermissionModels { get; set; } = new List<PermissionModel>();
}
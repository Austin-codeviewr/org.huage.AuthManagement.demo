using org.huage.AuthManagement.Entity.Model;

namespace org.huage.AuthManagement.Entity.Response;

public class QueryPermissionByRoleIdResponse
{
    public List<PermissionModel> PermissionModels { get; set; } = new List<PermissionModel>();
}
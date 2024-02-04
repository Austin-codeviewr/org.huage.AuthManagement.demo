using org.huage.AuthManagement.Entity.Model;

namespace org.huage.AuthManagement.Entity.Response;

public class QueryRoleByUserIdResponse
{
    public List<RoleModel> RoleModels { get; set; } = new List<RoleModel>();
}
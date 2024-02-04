using org.huage.AuthManagement.Entity.Model;
using org.huage.AuthManagement.Entity.Request;
using org.huage.AuthManagement.Entity.Response;

namespace org.huage.AuthManagement.Entity.Managers;

public interface IRoleManager
{
    Task<QueryAllRolesResponse> QueryAllRolesAsync(QueryAllRolesRequest request);
    Task<QueryPermissionByRoleIdResponse> QueryPermissionByRoleIdAsync(int roleId);

    Task<List<RoleModel>> QueryRoleByPageAsync(QueryRoleRequest request);

    Task AddRoleAsync(AddRoleRequest request);
}
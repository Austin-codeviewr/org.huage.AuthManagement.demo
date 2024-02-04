using org.huage.AuthManagement.Entity.Request;
using org.huage.AuthManagement.Entity.Response;

namespace org.huage.AuthManagement.Entity.Managers;

public interface IPermissionManager
{
    Task<QueryAllPermissionResponse> QueryAllPermissionAsync();
    Task AddPermissionAsync(AddPermissionRequest request);
}
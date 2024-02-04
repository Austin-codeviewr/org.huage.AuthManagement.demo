using org.huage.AuthManagement.Entity.Model;
using org.huage.AuthManagement.Entity.Request;
using org.huage.AuthManagement.Entity.Response;

namespace org.huage.AuthManagement.Entity.Managers;

public interface IOrganizationManager
{
    Task<QueryOrganizationUserAndRoleResponse> QueryOrganizationAllUserAndRoleAsync(int organizationId);

    Task<OrganizationModel> QueryOrganizationDetailAsync(int organizationId);

    Task<bool> OrganizationAddUserAsync(AddOrganizationUserRequest request);

    Task AddOrganizationAsync(AddOrganizationRequest request);

    Task<QueryAllOrganizationResponse> QueryAllOrganizationAsync();

    Task<bool> BatchDeleteOrganizationByIdAsync(List<int> ids);
}
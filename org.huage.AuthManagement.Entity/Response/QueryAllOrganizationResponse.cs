using org.huage.AuthManagement.Entity.Model;

namespace org.huage.AuthManagement.Entity.Response;

public class QueryAllOrganizationResponse
{
    public List<OrganizationModel> OrganizationModels { get; set; } = new List<OrganizationModel>();
}
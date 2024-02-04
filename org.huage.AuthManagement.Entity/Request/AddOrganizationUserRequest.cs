using org.huage.AuthManagement.Entity.Model;

namespace org.huage.AuthManagement.Entity.Request;

public class AddOrganizationUserRequest
{
    public int OrganizationId { get; set; }
    public AddUserRequest UserModel { get; set; }
    
    public List<RoleModel>? RoleModels { get; set; }
}
using org.huage.AuthManagement.Entity.Model;

namespace org.huage.AuthManagement.Entity.Request;

public class SetUserRoleRequest
{
    public List<UserRoleModel> userRoles { get; set; }
}
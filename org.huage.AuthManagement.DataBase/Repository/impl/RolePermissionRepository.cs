using org.huage.AuthManagement.DataBase.DBContext;
using org.huage.AuthManagement.DataBase.Table;

namespace org.huage.AuthManagement.DataBase.Repository.impl;

public class RolePermissionRepository : RepositoryBase<RolePermission>,IRolePermissionRepository
{
    public RolePermissionRepository(AuthDBContext context) : base(context)
    {
    }
}
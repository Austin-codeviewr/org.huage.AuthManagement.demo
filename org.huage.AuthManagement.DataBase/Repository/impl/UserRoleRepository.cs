using org.huage.AuthManagement.DataBase.DBContext;
using org.huage.AuthManagement.DataBase.Table;

namespace org.huage.AuthManagement.DataBase.Repository.impl;

public class UserRoleRepository : RepositoryBase<UserRole>,IUserRoleRepository
{
    public UserRoleRepository(AuthDBContext context) : base(context)
    {
    }
}
using org.huage.AuthManagement.DataBase.DBContext;
using org.huage.AuthManagement.DataBase.Table;

namespace org.huage.AuthManagement.DataBase.Repository.impl;

public class UserRepository: RepositoryBase<User>,IUserRepository
{
    public UserRepository(AuthDBContext context) : base(context)
    {
    }
}
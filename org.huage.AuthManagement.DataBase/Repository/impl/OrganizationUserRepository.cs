using org.huage.AuthManagement.DataBase.DBContext;
using org.huage.AuthManagement.DataBase.Table;

namespace org.huage.AuthManagement.DataBase.Repository.impl;

public class OrganizationUserRepository : RepositoryBase<OrganizationUser>,IOrganizationUserRepository
{
    public OrganizationUserRepository(AuthDBContext context) : base(context)
    {
    }
}
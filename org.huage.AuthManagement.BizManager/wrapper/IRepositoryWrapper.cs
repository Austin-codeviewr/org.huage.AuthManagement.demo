using Microsoft.EntityFrameworkCore.Storage;
using org.huage.AuthManagement.DataBase.Repository;

namespace org.huage.AuthManagement.BizManager.wrapper;

public interface IRepositoryWrapper
{
    IRoleRepository Role { get; }
    IOrganizationRepository Organization { get; }
    IPermissionRepository Permission { get; }
    
    IOrganizationUserRepository OrganizationUser { get; }
    IUserRoleRepository UserRole { get; }
    
    IUserRepository User { get; }
    
    IRolePermissionRepository RolePermission { get; }
    Task<int> SaveChangeAsync();

    Task<IDbContextTransaction> StartTransactionAsync();
}
using Microsoft.EntityFrameworkCore.Storage;
using org.huage.AuthManagement.DataBase.DBContext;
using org.huage.AuthManagement.DataBase.Repository;
using org.huage.AuthManagement.DataBase.Repository.impl;

namespace org.huage.AuthManagement.BizManager.wrapper.impl;

public class RepositoryWrapper : IRepositoryWrapper
{
    
    private readonly AuthDBContext _context;

    private  IRoleRepository _roleRepository;
    private  IOrganizationRepository _organizationRepository;
    private  IPermissionRepository _permissionRepository;
    private  IUserRoleRepository _userRoleRepository;
    private  IUserRepository _userRepository;
    private  IOrganizationUserRepository _organizationUserRepository;
    private  IRolePermissionRepository _rolePermissionRepository;
    
    public IRoleRepository Role
    {
        get { return _roleRepository ??= new RoleRepository(_context); }
    }
    
    public IRolePermissionRepository RolePermission
    {
        get { return _rolePermissionRepository ??= new RolePermissionRepository(_context); }
    }
    
    
    public IOrganizationUserRepository OrganizationUser
    {
        get { return _organizationUserRepository ??= new OrganizationUserRepository(_context); }
    }
    
    public IOrganizationRepository Organization
    {
        get { return _organizationRepository ??= new OrganizationRepository(_context); }
    }
    
    public IPermissionRepository Permission
    {
        get { return _permissionRepository ??= new PermissionRepository(_context); }
    }
    
    public IUserRepository User
    {
        get { return _userRepository ??= new UserRepository(_context); }
    }
    
    public IUserRoleRepository UserRole
    {
        get { return _userRoleRepository ??= new UserRoleRepository(_context); }
    }
    public Task<int> SaveChangeAsync()
    {
        return _context.SaveChangesAsync();
    }

    public RepositoryWrapper(AuthDBContext context)
    {
        _context = context;
    }

    public Task<IDbContextTransaction> StartTransactionAsync()
    {
        return _context.Database.BeginTransactionAsync();
    }
}
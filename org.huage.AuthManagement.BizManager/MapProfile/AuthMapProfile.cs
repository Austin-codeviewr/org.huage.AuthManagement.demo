using AutoMapper;
using org.huage.AuthManagement.DataBase.Table;
using org.huage.AuthManagement.Entity.Model;
using org.huage.AuthManagement.Entity.Request;
using org.huage.AuthManagement.Entity.Response;

namespace org.huage.AuthManagement.BizManager.MapProfile;

/// <summary>
/// 实体属性映射
/// </summary>
public class AuthMapProfile : Profile
{
    public AuthMapProfile()
    {
        CreateMap<Permission,PermissionModel>();
        CreateMap<Role,RoleModel>();
        CreateMap<User,UserModel>();
        CreateMap<Organization,OrganizationModel>()
            .ForMember(dest =>dest.Id,
                opt=> opt.MapFrom(src=> src.Id));
        CreateMap<UserRole,UserRoleModel>();
        CreateMap<UserRoleModel,UserRole>();
        CreateMap<User, OrganizationUserAndRole>();
        CreateMap<RolePermission,RolePermissionModel>();
        CreateMap<OrganizationUser,OrganizationUserModel>();
        CreateMap<AddUserRequest,User>();
        CreateMap<AddOrganizationRequest, Organization>();
        CreateMap<AddPermissionRequest,Permission>();
        CreateMap<OrganizationUserModel,OrganizationUser>();
        
    }
}
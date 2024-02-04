using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using org.huage.AuthManagement.BizManager.Redis;
using org.huage.AuthManagement.BizManager.wrapper;
using org.huage.AuthManagement.DataBase.Table;
using org.huage.AuthManagement.Entity.Common;
using org.huage.AuthManagement.Entity.Managers;
using org.huage.AuthManagement.Entity.Model;
using org.huage.AuthManagement.Entity.Request;
using org.huage.AuthManagement.Entity.Response;

namespace org.huage.AuthManagement.BizManager.Manager;

//组织管理
public class OrganizationManager : IOrganizationManager
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IRedisManager _redisManager;
    private readonly ILogger<OrganizationManager> _logger;
    private readonly IMapper _mapper;
    private readonly IUserManager _userManager;

    public OrganizationManager(IRepositoryWrapper wrapper, IRedisManager redisManager,
        ILogger<OrganizationManager> logger, IMapper mapper,IUserManager userManager)
    {
        _wrapper = wrapper;
        _redisManager = redisManager;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }


    /// <summary>
    /// 查询该组织的用户，以及用户对应的角色:
    /// 1.查询该组织下的用户
    /// 2.查询用户角色
    /// </summary>
    /// <param name="organizationId"></param>
    public async Task<QueryOrganizationUserAndRoleResponse> QueryOrganizationAllUserAndRoleAsync(int organizationId)
    {
        var response = new QueryOrganizationUserAndRoleResponse();
        try
        {
            //查询缓存
            var organizationUsers =
                await _redisManager.HGetAllValue<OrganizationUserAndRole>(RedisKeyGenerator.OrganizationAllUserAndRole(organizationId));
            
            if (organizationUsers.Any())
            {
                response.Users = organizationUsers;
                return response;
            }
            
            //1.从中间表中查询出该组织对应的用户有那些，然后拿到用户信息
            var userIdList = await _wrapper.OrganizationUser
                .FindByCondition(_ => _.OrganizationId == organizationId && _.IsDeleted == false).Select(_ => _.UserId)
                .ToListAsync();
            //2.拿到所有的用户
            if (userIdList.Any())
            {
                foreach (var userId in userIdList)
                {
                    //拿到用户
                    var user = _wrapper.User.FindByCondition(_ => _.Id == userId && _.IsDeleted==false).FirstOrDefault();
                    if (user is null)
                    {
                        continue;
                    }
                    var realUser = _mapper.Map<OrganizationUserAndRole>(user);

                    //拿到该用户的角色id列表
                    var ids = await _wrapper.UserRole
                        .FindByCondition(_ => organizationId == _.OrganizationId && userId == _.UserId)
                        .Select(_ => _.RoleId).ToListAsync();

                    foreach (var id in ids)
                    {
                        //拿到角色
                        var role = _wrapper.Role.FindByCondition(_ => _.Id == id).FirstOrDefault();
                        //添加进入集合
                        var roleModel = _mapper.Map<RoleModel>(role);
                        realUser.RoleModels?.Add(roleModel);
                    }

                    //加入返回结果集
                    response.Users.Add(realUser);
                }
            }

            var userAndRoles = response.Users.ToDictionary(_ => _.Id);

            //添加缓存
            await userAndRoles.ParallelForEachAsync(async ur =>
            {
                var redisKey = RedisKeyGenerator.OrganizationAllUserAndRole(organizationId);
                try
                {
                    foreach (var s in response.Users)
                    {
                        await _redisManager.HashSet(redisKey, RedisKeyGenerator.OrganizationUser(ur.Key),
                            JsonConvert.SerializeObject(ur.Value));
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }
            });
        }
        catch (Exception e)
        {
            _logger.LogError($"Excuse QueryOrganizationUserAndRole error: {e.Message}");
            throw;
        }

        return response;
    }


    //查询所有的组织
    public async Task<QueryAllOrganizationResponse> QueryAllOrganizationAsync()
    {
        var response = new QueryAllOrganizationResponse();
        try
        {
            //查缓存
            var value = await _redisManager.HGetAllValue<Organization>(RedisKeyGenerator.AllOrganizationsRedisKey());
            if (value.Any())
            {
                response.OrganizationModels = _mapper.Map<List<OrganizationModel>>(value);
                return response;
            }

            //查询数据库
            var organizationList = await _wrapper.Organization.FindByCondition(_ => _.IsDeleted == false).ToListAsync();
            response.OrganizationModels = _mapper.Map<List<OrganizationModel>>(organizationList);

            //设置缓存
            var organizationDic = organizationList.ToDictionary(_ => _.Id);
            await organizationDic.ParallelForEachAsync(async organization =>
            {
                var redisKey = RedisKeyGenerator.AllOrganizationsRedisKey();
                try
                {
                    foreach (var s in organizationDic)
                    {
                        await _redisManager.HashSet(redisKey, organization.Key.ToString(),
                            JsonConvert.SerializeObject(organization.Value));
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }
            });
        }
        catch (Exception e)
        {
            _logger.LogError($"QueryAllOrganizationAsync error: {e.Message}");
            throw;
        }

        return response;
    }

    /// <summary>
    /// 查询该组织明细
    /// </summary>
    /// <param name="organizationId"></param>
    /// <returns></returns>
    public async Task<OrganizationModel> QueryOrganizationDetailAsync(int organizationId)
    {
        var organization = _wrapper.Organization.FindByCondition(_ => _.Id == organizationId).FirstOrDefault();
        return _mapper.Map<OrganizationModel>(organization);
    }


    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    public async Task<bool> BatchDeleteOrganizationByIdAsync(List<int> ids)
    {
        if (!ids.Any())
            return false;
        
        foreach (var id in ids)
        {
            var first = _wrapper.Organization.FindByCondition(_ => _.Id == id && _.IsDeleted==false).FirstOrDefault();
            if (first is not null)
            {
                //逻辑删除
                first.IsDeleted = true;
                _wrapper.Organization.Update(first);
                
                //删除关联表organizationUser
                var organizationUserList = await _wrapper.OrganizationUser.FindByCondition(_=>_.OrganizationId==id && _.IsDeleted==false).ToListAsync();
                if (organizationUserList.Any())
                {
                    foreach (var organizationUser in organizationUserList)
                    {
                        //逻辑删除
                        organizationUser.IsDeleted = true;
                        _wrapper.OrganizationUser.Update(organizationUser);
                    }
                }
            }
        }

        return true;

    }

    /// <summary>
    /// 组织添加用户：添加用户的时候，可以选择是否给该用户配角色
    /// </summary>
    /// <summary>
    /// 比如说A组织创建了一个用户a,然后用户表插入了一个用户（手机号唯一），接着在organizationUser表加入一条id, A.ID , a.id的的数据；
    /// 然后我们给该A组织的a用户设置角色的时候，我们去UserRole表加入一条数据：userid,roleId,organizationId.
    ///
    /// 这时候B组织把a也加入自己的组织了，给该用户设置B组织a用户的权限：在UserRole角色表里面加入一条，userid,roleId,organizationId.
    /// 希望解决同一个用户在不同系统拥有不同的权限：逻辑就是组织添加用户的时候，加入organizationUser表，并且需要给其授予角色（默认是游客），也可以从用户表中挑选用户；
    /// </summary>
    public async Task<bool> OrganizationAddUserAsync(AddOrganizationUserRequest request)
    {
        if (!IsValidOrgCode(request.OrganizationId.ToString()))
            throw new OrganizationException("OrganizationId invalid，please fill in valid orgCode");
        if (string.IsNullOrEmpty(request.UserModel.Phone))
            throw new OrganizationException("Phone is required.");

        //开启事务
        await using var transaction = await _wrapper.StartTransactionAsync();

        try
        {
            //添加用户
            var addUser = await _userManager.AddUserAsync(request.OrganizationId, request.UserModel);

            //添加OrganizationUser
            var organizationUserModel = new OrganizationUserModel()
            {
                OrganizationId = request.OrganizationId,
                UserId = addUser.Id
            };
            var organizationUser = _mapper.Map<OrganizationUser>(organizationUserModel);
            _wrapper.OrganizationUser.Create(organizationUser);

            //添加UserRole
            if (request.RoleModels != null && request.RoleModels.Any())
            {
                foreach (var role in request.RoleModels)
                {
                    var userRoleModel = new UserRoleModel()
                    {
                        UserId = addUser.Id,
                        OrganizationId = request.OrganizationId,
                        RoleId = role.Id
                    };
                    var userRole = _mapper.Map<UserRole>(userRoleModel);
                    _wrapper.UserRole.Create(userRole);
                }
            }
            else
            {
                //设置该用户默认角色为游客
                var ur = new UserRoleModel()
                {
                    UserId = addUser.Id,
                    OrganizationId = request.OrganizationId,
                    RoleId = 2
                };
                var userRole = _mapper.Map<UserRole>(ur);
                _wrapper.UserRole.Create(userRole);
            }

            await _wrapper.SaveChangeAsync();

            await transaction.CommitAsync();
        }
        catch (UserException ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex.Message);
            throw;
        }
        catch (Exception e)
        {
            //发生异常回滚
            await transaction.RollbackAsync();
            _logger.LogError($"Excuse OrganizationAddUser error: {e.Message}");
            throw;
        }

        return true;
    }


    /// <summary>
    /// 新增组织
    /// </summary>
    /// <param name="request"></param>
    /// <exception cref="Exception"></exception>
    public async Task AddOrganizationAsync(AddOrganizationRequest request)
    {
        try
        {
            //params check
            if (string.IsNullOrEmpty(request.OrgCode) ||
                string.IsNullOrEmpty(request.Name))
            {
                throw new OrganizationException("组织名称和组织注册编码不能为空");
            }

            if (string.IsNullOrEmpty(request.Version))
            {
                request.Version = "v1";
            }

            //mapping
            var organization = _mapper.Map<Organization>(request);
            _wrapper.Organization.Create(organization);
            await _wrapper.SaveChangeAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"Excuse AddOrganization error: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// 判断传进来的营业执照是否有效,这里可以根据自己的业务具体实现,当前假设为邮箱
    /// </summary>
    /// <param name="orgCode"></param>
    /// <returns></returns>
    private bool IsValidOrgCode(string orgCode)
    {
        var regex = new Regex("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$");
        return regex.IsMatch(orgCode);
    }
}
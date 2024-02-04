using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using org.huage.AuthManagement.BizManager.Redis;
using org.huage.AuthManagement.BizManager.wrapper;
using org.huage.AuthManagement.DataBase.Table;
using org.huage.AuthManagement.Entity.Managers;
using org.huage.AuthManagement.Entity.Model;
using org.huage.AuthManagement.Entity.Request;
using org.huage.AuthManagement.Entity.Response;

namespace org.huage.AuthManagement.BizManager.Manager;

//角色管理
public class RoleManager : IRoleManager
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IRedisManager _redisManager;
    private readonly ILogger<RoleManager> _logger;
    private readonly IMapper _mapper;

    public RoleManager(IRedisManager redisManager, ILogger<RoleManager> logger, IMapper mapper,
        IRepositoryWrapper wrapper)
    {
        _redisManager = redisManager;
        _logger = logger;
        _mapper = mapper;
        _wrapper = wrapper;
    }

    /// <summary>
    /// 查询指定页的数据
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<List<RoleModel>> QueryRoleByPageAsync(QueryRoleRequest request)
    {
        //考虑分页
        //参数校验,设置错误，返回第一页
        if (request.PageSize <=0 || request.PageNumber<0)
        {
            request.PageNumber = 1;
            request.PageSize = 10;
        }
        //查询数据库,TODO：后面需要考虑查询的页数是否超过最大页数
        var skip = (request.PageNumber - 1) * request.PageSize;
        var schedulers =await _wrapper.Role.FindAll().Where(_ => _.IsDeleted == false).OrderByDescending(_ => _.CreateTime)
            .Skip(skip).Take(request.PageSize).ToListAsync();
        
        return _mapper.Map<List<RoleModel>>(schedulers);
    }
    
    /// <summary>
    /// 查询所有的角色
    /// </summary>
    /// <returns></returns>
    public async Task<QueryAllRolesResponse> QueryAllRolesAsync(QueryAllRolesRequest request)
    {
        var response = new QueryAllRolesResponse();

        try
        {
            //查询缓存
            var allSchedulers = await _redisManager.HGetAllValue<RoleModel>(RedisKeyGenerator.AllRolesRedisKey());
            if (allSchedulers.Any())
            {
                response.RoleModels = allSchedulers;
                return response;
            }
            
            //查询数据库
            var roles = _wrapper.Role.FindAll().Where(_ => _.IsDeleted == false).ToList();
            var data = _mapper.Map<List<RoleModel>>(roles);
            response.RoleModels = data;

            var rolesDic = data.ToDictionary(_ => _.Id);
            //添加缓存
            await rolesDic.ParallelForEachAsync(async role =>
            {
                var redisKey = RedisKeyGenerator.AllRolesRedisKey();
                try
                {
                    foreach (var s in data)
                    {
                        await _redisManager.HashSet(redisKey, role.Key.ToString(),
                            JsonConvert.SerializeObject(role.Value));
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
            _logger.LogError($"QueryAllRolesAsync error: {e.Message}");
            throw;
        }

        return response;
    }


    /// <summary>
    /// 查询角色的权限
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task<QueryPermissionByRoleIdResponse> QueryPermissionByRoleIdAsync(int roleId)
    {
        var response = new QueryPermissionByRoleIdResponse();
        //查询权限ids
        var permissionIds = await _wrapper.RolePermission
            .FindByCondition(_ => _.RoleId == roleId && _.IsDeleted == false)
            .Select(_ => _.PermissionId).ToListAsync();
        if (permissionIds.Any())
        {
            foreach (var id in permissionIds)
            {
                var permission = _wrapper.Permission.FindByCondition(_ => _.Id == id && _.IsDeleted == false)
                    .FirstOrDefault();
                var permissionModel = _mapper.Map<PermissionModel>(permission);
                response.PermissionModels.Add(permissionModel);
            }
        }

        return response;
    }

    
    /// <summary>
    /// 新增角色
    /// </summary>
    /// <param name="request"></param>
    public async Task AddRoleAsync(AddRoleRequest request)
    {
        //开启事务
        await using var transaction = await _wrapper.StartTransactionAsync();
        try
        {
            if (request.RoleIds.Any())
            {
                //插入角色
                var role = new Role()
                {
                    Name = request.Name,
                    CreateBy= request.CreateBy
                };
                _wrapper.Role.Create(role);
                //这里提前保存是为了拿到数据库中该对象的id.
                await _wrapper.SaveChangeAsync();
                //先插rolePermission表
                foreach (var p in request.RoleIds)
                {
                    var rolePermission = new RolePermission()
                    {
                        RoleId = role.Id,
                        PermissionId = p
                    };
                    _wrapper.RolePermission.Create(rolePermission);
                }
            }
            else
            {
                var role = _mapper.Map<Role>(request);
                _wrapper.Role.Create(role);
            }

            await _wrapper.SaveChangeAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            _logger.LogError("AddRoleAsync error.");
            throw;
        }
    }
}
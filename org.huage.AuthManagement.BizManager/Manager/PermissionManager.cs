using AutoMapper;
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

//权限管理
public class PermissionManager : IPermissionManager
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IRedisManager _redisManager;
    private readonly ILogger<PermissionManager> _logger;
    private readonly IMapper _mapper;

    public PermissionManager(IRepositoryWrapper wrapper, IRedisManager redisManager, ILogger<PermissionManager> logger, IMapper mapper)
    {
        _wrapper = wrapper;
        _redisManager = redisManager;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// 查询所有的权限信息。
    /// </summary>
    /// <returns></returns>
    public async Task<QueryAllPermissionResponse> QueryAllPermissionAsync()
    {
        var response = new QueryAllPermissionResponse();

        try
        {
            //查询缓存
            var permissionModels = await _redisManager.HGetAllValue<PermissionModel>(RedisKeyGenerator.AllRolesRedisKey());
            if (permissionModels.Any())
            {
                response.PermissionModels = permissionModels;
                return response;
            }
            
            //查询数据库
            var permissions = _wrapper.Permission.FindAll().Where(_ => _.IsDeleted == false).ToList();
            var data = _mapper.Map<List<PermissionModel>>(permissions);
            response.PermissionModels = data;

            var permissionsDic = data.ToDictionary(_ => _.Id);
            //添加缓存
            await permissionsDic.ParallelForEachAsync(async permission =>
            {
                var redisKey = RedisKeyGenerator.AllPermissions();
                try
                {
                    foreach (var s in data)
                    {
                        await _redisManager.HashSet(redisKey, permission.Key.ToString(),
                            JsonConvert.SerializeObject(permission.Value));
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
            _logger.LogError($"QueryAllPermissionAsync error: {e.Message}");
            throw;
        }

        return response;
    }

    /// <summary>
    /// 添加权限
    /// </summary>
    /// <param name="request"></param>
    public async Task AddPermissionAsync(AddPermissionRequest request)
    {
        var permission = _mapper.Map<Permission>(request);
        _wrapper.Permission.Create(permission);
        await _wrapper.SaveChangeAsync();
    }
}
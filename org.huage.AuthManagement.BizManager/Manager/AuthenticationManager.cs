using Microsoft.Extensions.Logging;
using org.huage.AuthManagement.BizManager.Redis;
using org.huage.AuthManagement.BizManager.wrapper;
using org.huage.AuthManagement.Entity.Managers;

namespace org.huage.AuthManagement.BizManager.Manager;

//权限认证
public class AuthenticationManager : IAuthenticationManager
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IRedisManager _redisManager;
    private readonly ILogger<AuthenticationManager> _logger;
    
    public AuthenticationManager(IRepositoryWrapper wrapper, IRedisManager redisManager, ILogger<AuthenticationManager> logger)
    {
        _wrapper = wrapper;
        _redisManager = redisManager;
        _logger = logger;
    }

    /// <summary>
    /// 拿token,里面需要存取用户的基本信息，用户名，以及是哪个组织下的什么角色，角色列表用集合保存（一般用户都是给单个角色）
    /// </summary>
    /// <returns></returns>
    public void GetToken()
    {
        
    }
}
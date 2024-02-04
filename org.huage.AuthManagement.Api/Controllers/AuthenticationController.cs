using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using org.huage.AuthManagement.Api.Auth;
using org.huage.AuthManagement.BizManager.Helper;
using org.huage.AuthManagement.BizManager.Redis;
using org.huage.AuthManagement.BizManager.service;
using org.huage.AuthManagement.Entity.Managers;
using org.huage.AuthManagement.Entity.Request;
using StackExchange.Redis;

namespace org.huage.AuthManagement.Api.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class AuthenticationController : Controller 
{
    private readonly ILogger<AuthenticationController> _logger;

    private readonly IJwtService _jwtService;
    private readonly IUserManager _userManager;
    private readonly IRedisManager _redisManager;
    public AuthenticationController(IJwtService jwtService, IUserManager userManager, IRedisManager redisManager, ILogger<AuthenticationController> logger)
    {
        _jwtService = jwtService;
        _userManager = userManager;
        _redisManager = redisManager;
        _logger = logger;
    }

    /// <summary>
    /// 过期时间由redis控制，不再jwt里面设置，不好refresh 时间
    /// 而且避免token被破解修改过期时间。
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<string> Login(LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Phone) || string.IsNullOrEmpty(request.PassWord))
            return "Account and Password is must.";

        string userId = "";
        IDatabase dataBase = null;
        try
        {
            //查询数据库校验参数
            var user = await _userManager.QueryUserByPhone(request.Phone);
            
            userId = user.Id.ToString();
            //密码加密
            var realPwd = PwdBCrypt.Encryption(request.PassWord);
            if (!user.Phone.Equals(request.Phone) || !user.PassWord.Equals(realPwd))
            {
                return "Account or Password wrong.";
            }

            //切库，token专门设立一个库,避免混乱
            dataBase = await _redisManager.SwitchDataBase(1);
            
            var token = (await dataBase.StringGetAsync(userId)).ToString();
            if (string.IsNullOrEmpty(token))
            {
                //首次登录 ，生成token
                token = await _jwtService.GetToken(request.OrganizationId,user);
            }
            else
            {
                //校验token
                await _jwtService.VerifyToken(token);
                //刷新过期时间
                await _jwtService.RefreshToken(userId);
            }
            
            //将token 放到请求头
            Response.Headers.Add(new KeyValuePair<string, StringValues>("Authorization",token));
            
            return "Login success.";
        }
        catch (Exception e)
        {
            //删除token
            await dataBase.KeyDeleteAsync(userId);
            return "Login credentials have expired, please log in again";
        }
    }

    /// <summary>
    /// 加载页面的时候调用这个方法，然后判断是否有token.
    /// </summary>
    [HttpGet]
    public async Task<bool> NoPwdLogin()
    {
        try
        {
            //无密码登录，解析token信息
            string userToken = Request.Headers["token"].ToString();
            await _jwtService.AnalysisToken(userToken);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogWarning("user token is valid,please login.");
            return false;
        }
    }


    /// <summary>
    /// 退出登录
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Logout(int userId)
    {
        
        //页面跳转
        return new RedirectResult("/index/login");
    }
}
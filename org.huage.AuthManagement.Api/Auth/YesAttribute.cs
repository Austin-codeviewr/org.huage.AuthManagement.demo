using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using org.huage.AuthManagement.BizManager.service;

namespace org.huage.AuthManagement.Api.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class YesAttribute : Attribute,IAuthorizationFilter
{
    public string Roles { get; set; }

    public YesAttribute()
    {
    }

    //自定义权限认证
    public async void OnAuthorization(AuthorizationFilterContext context)
    {
        //如果不携带这个参数则不校验
        if (! context.ActionDescriptor.EndpointMetadata.Any(item => item is YesAttribute))
        {
            return;
        }
        //如果方法标有AllowAnonymousAttribute则跳过权限检查
        if (context.ActionDescriptor.EndpointMetadata.Any(item => item is AllowAnonymousAttribute))
        {
            return;
        }
        //拿到token
        string userToken = context.HttpContext.Request.Headers["token"].ToString(); //获取token
        if (string.IsNullOrEmpty(userToken))
        {
            context.Result = new ContentResult() { StatusCode = 403, Content = "Headers no token,please check." };  //没有Cookie则跳转到登陆页面
            return;
        }
        else
        {
            //拿到jwt服务来校验
            var jwtService = context.HttpContext.RequestServices.GetService<IJwtService>();
            //解析token,拿到role集合。
            var roles =await jwtService!.AnalysisToken(userToken);
            if (! roles.Any())
            {
                context.Result = new ContentResult() { StatusCode = 401, Content = "Please check the roles you owned." };
                return;
            }
            //获取attribute的roles;
            var methodInfo = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo;
            var attribute = methodInfo.GetCustomAttribute<YesAttribute>();
            if (attribute ==null)
            {
                context.Result = new ContentResult() { StatusCode = 401, Content = "Please check the roles you owned." };   
                return;
            }
            
            var attributeRoles = attribute.Roles;
            var a = attributeRoles.Split(",").ToHashSet();
            var of = roles.ToHashSet().IsSupersetOf(a);
            if (!of)
            {
                context.Result = new ContentResult() { StatusCode = 401, Content = "You don't have this role" };
            }
        }
       
    }
}
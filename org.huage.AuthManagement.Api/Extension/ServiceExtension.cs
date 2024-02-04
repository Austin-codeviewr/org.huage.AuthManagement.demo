using System.Text;
using Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using org.huage.AuthManagement.Api.Consul;
using org.huage.AuthManagement.BizManager.wrapper;
using org.huage.AuthManagement.BizManager.wrapper.impl;
using org.huage.AuthManagement.DataBase.DBContext;
using org.huage.AuthManagement.Entity.Common;
using StackExchange.Redis;

namespace org.huage.AuthManagement.Api.Extension;

public static class ServiceExtension
{
    public static void ConfigureCors(this IServiceCollection service)
    {
        service.AddCors(option =>
        {
            option.AddPolicy("AnyPolicy", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });
    }
    
    public static void ConfigureMysql(this IServiceCollection service,IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Auth");
        service.AddDbContext<AuthDBContext>(builder =>
            builder.UseMySql(connectionString, MySqlServerVersion.LatestSupportedServerVersion));
    }
    
    
    public static WebApplication  UseConsulRegistry(this WebApplication webApplication, IHostApplicationLifetime builder)
    {
        var optionsMonitor = webApplication.Services.GetService<IOptionsMonitor<ConsulOption>>();
        var consulOption = optionsMonitor!.CurrentValue;
        //获取心跳监测的Ip和port
        var consulOptionIp = webApplication.Configuration["ip"] ?? consulOption.IP;
        var port = webApplication.Configuration["Port"] ?? consulOption.Port;
        
        //生成serviceId
        var id = Guid.NewGuid().ToString();
        
        //创建连接Consul客户端对象
        var consulClient = new ConsulClient(c =>
        {
            c.Address = new Uri(consulOption.ConsulHost!);
            c.Datacenter = consulOption.ConsulDataCenter;
        });
        //把服务注册到consul上
        consulClient.Agent.ServiceRegister(new AgentServiceRegistration()
        {
            ID = id,
            Name = consulOption.ServiceName,
            Address = consulOptionIp,
            Port = Convert.ToInt32(port),
            Check = new AgentServiceCheck()
            {
                Interval = TimeSpan.FromSeconds(12),
                HTTP = $"http://{consulOptionIp}:{port}/api/health",
                Timeout = TimeSpan.FromSeconds(5),
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(20)
            }
        });
        
        builder.ApplicationStopped.Register(async () =>
        {
            Console.WriteLine("服务注销");
            await consulClient.Agent.ServiceDeregister(id);
        });

        return webApplication;
    }
    
    
    public static void ConfigureRedis(this IServiceCollection service,IConfiguration configuration)
    {
        service.AddSingleton<IConnectionMultiplexer>(provider =>
        {
            var connectionString = configuration.GetConnectionString("redis");
            return ConnectionMultiplexer.Connect(connectionString!);
        });
    }
    
    public static void ConfigureRepositoryWrapper(this IServiceCollection service)
    {
        service.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
    }

    public static void JwtAuthentication(this WebApplicationBuilder builder)
    {
        JWTTokenOptions tokenOptions = new JWTTokenOptions();//初始化
        builder.Configuration.Bind("JWTTokenOptions", tokenOptions);
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //JWT有一些默认的属性，就是给鉴权时就可以筛选了
                    ValidateLifetime = false,
                    ValidateIssuer = true, //是否验证Issuer
                    ValidateAudience = true, //是否验证Audience
                    ValidateIssuerSigningKey = true, //是否验证SecurityKey
                    ValidAudience = tokenOptions.Audience,
                    ValidIssuer = tokenOptions.Isuser, //Issuer,这两项和前面签发jwt的设置一致
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey))
                };
            });
    }
}
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using org.huage.AuthManagement.Api.Auth;
using org.huage.AuthManagement.Api.Consul;
using org.huage.AuthManagement.Api.Extension;
using org.huage.AuthManagement.BizManager.Manager;
using org.huage.AuthManagement.BizManager.MapProfile;
using org.huage.AuthManagement.BizManager.Redis;
using org.huage.AuthManagement.BizManager.service;
using org.huage.AuthManagement.BizManager.service.impl;
using org.huage.AuthManagement.Entity.Common;
using org.huage.AuthManagement.Entity.Managers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureMysql(builder.Configuration);
builder.Services.ConfigureRedis(builder.Configuration);
builder.JwtAuthentication();
builder.Services.ConfigureRepositoryWrapper();
//注册过滤器
builder.Services.AddControllers(o =>
{
    o.Filters.Add<YesAttribute>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AuthMapProfile).Assembly);
builder.Services.AddTransient<IRedisManager, RedisManager>();
builder.Services.AddTransient<IPermissionManager,PermissionManager>();
builder.Services.AddTransient<IUserManager,UserManager>();
builder.Services.AddTransient<IRoleManager,RoleManager>();
builder.Services.AddTransient<IOrganizationManager,OrganizationManager>();
builder.Services.AddTransient<IAuthenticationManager,AuthenticationManager>();
builder.Services.AddTransient<IJwtService,JwtService>();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<ConsulOption>(builder.Configuration.GetSection("ConsulOption"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
RedisKeyGenerator.SetPrefix("Auth");
app.UseConsulRegistry(app.Lifetime);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/api/health", () =>
{
    global::System.Console.WriteLine("Ok");
    return new
    {
        Message = "Ok"
    };
});
app.UseCors("AnyPolicy");
app.MapControllers();

app.Run();
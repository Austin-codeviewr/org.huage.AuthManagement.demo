using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using org.huage.AuthManagement.BizManager.Helper;
using org.huage.AuthManagement.BizManager.Redis;
using org.huage.AuthManagement.BizManager.wrapper;
using org.huage.AuthManagement.DataBase.Table;
using org.huage.AuthManagement.Entity.Common;
using org.huage.AuthManagement.Entity.Managers;
using org.huage.AuthManagement.Entity.Model;
using org.huage.AuthManagement.Entity.Request;
using org.huage.AuthManagement.Entity.Response;

namespace org.huage.AuthManagement.BizManager.Manager;

//用户管理
public class UserManager : IUserManager
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly IRedisManager _redisManager;
    private readonly ILogger<UserManager> _logger;
    private readonly IMapper _mapper;

    public UserManager(IMapper mapper, ILogger<UserManager> logger, IRedisManager redisManager, IRepositoryWrapper wrapper)
    {
        _mapper = mapper;
        _logger = logger;
        _redisManager = redisManager;
        _wrapper = wrapper;
    }


    /// <summary>
    /// 根据id查询用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<UserModel> QueryUserById(int id)
    {
        var user = _wrapper.User.FindByCondition(_=>_.Id==id).FirstOrDefault();
        if (user is null)
            return null;
        
        var userModel = _mapper.Map<UserModel>(user);
        return userModel;
    }

    /// <summary>
    /// 根据手机号查询用户
    /// </summary>
    /// <param name="phone"></param>
    /// <returns></returns>
    public async Task<UserModel> QueryUserByPhone(string phone)
    {
        var user = _wrapper.User.FindByCondition(_=>_.Phone == phone).FirstOrDefault();
        if (user is null)
            return null;
        
        var userModel = _mapper.Map<UserModel>(user);
        return userModel;
    }
    
    /// <summary>
    /// 新增用户,用户至少归属于一个组织
    /// </summary>
    /// <param name="request"></param>
    /// <exception cref="Exception"></exception>
    public async Task<UserModel> AddUserAsync(int organizationId, AddUserRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.PassWord))
                throw new UserException("Account and pwd is must.");
            if (string.IsNullOrEmpty(request.Phone))
                throw new UserException("Phone & OrgCode is required");

            //密码加密
            request.PassWord = PwdBCrypt.Encryption(request.PassWord);
            
            //判断用户表是否存在该用户
            var existOrNot = _wrapper.User.FindByCondition(_ => _.Phone.Equals(request.Phone )&& _.UserName.Equals(request.UserName))
                .FirstOrDefault();
            if (existOrNot is not null)
            {
                //判断关联表是否存在该用户
                var organizationUser = _wrapper.OrganizationUser
                    .FindByCondition(_ => _.UserId == existOrNot.Id && _.OrganizationId == organizationId)
                    .FirstOrDefault();
                if (organizationUser is not null)
                {
                    //说明该用户已经存在
                    throw new UserException("该用户已存在");
                }
            }

            //删除缓存
            await _redisManager.DelKey(RedisKeyGenerator.AllUsersRedisKey());
            var user = _mapper.Map<User>(request);
            _wrapper.User.Create(user);
            await _wrapper.SaveChangeAsync();

            //
            await _redisManager.DelKey(RedisKeyGenerator.AllUsersRedisKey());
            
            var userModel = _mapper.Map<UserModel>(user);
            return userModel;
            
        }
        catch (Exception _)
        {
            _logger.LogError($"Excuse AddUserAsync error: {_.Message}");
            throw;
        }
    }

  
    
    /// <summary>
    /// 更新用户信息
    /// </summary>
    /// <param name="request"></param>
    public async Task UpdateUserAsync(UpdateUserRequest request)
    {
        var user = _wrapper.User.FindByCondition(_ => _.Phone == request.Phone && _.IsDeleted == false).FirstOrDefault();
        if (user is null)
        {
            //user not exist
            return;
        }
        if (! string.IsNullOrEmpty(request.PassWord))
        {
            request.PassWord = PwdBCrypt.Encryption(request.PassWord);
        }
        
        if (string.Equals(user.UserName,request.UserName) && string.Equals(user.PassWord,request.PassWord) && string.Equals(user.Phone,request.Phone) && string.Equals(user.Remark,request.Remark))
        {
            //data not change
            return;
        }
        //set value
        if (!string.IsNullOrEmpty(request.UserName))
        {
            user.UserName = request.UserName;
        }
        if (!string.IsNullOrEmpty(request.PassWord))
        {
            user.PassWord = request.PassWord;
        }
        if (!string.IsNullOrEmpty(request.Phone))
        {
            user.Phone = request.Phone;
        }
        if (!string.IsNullOrEmpty(request.Remark))
        {
            user.Remark = request.Remark;
        }
        user.UpdateTime=DateTime.Now;
        _wrapper.User.Update(user);
        await _wrapper.SaveChangeAsync();
    }

    /// <summary>
    /// 批量删除用户
    /// </summary>
    /// <param name="ids"></param>
    /// <exception cref="Exception"></exception>
    public async Task DeleteBatchUserAsync(List<int> ids)
    {
        var list =await _wrapper.User.FindByCondition(x=>ids.Contains(x.Id)).ToListAsync();
        foreach (var item in list)
        {
            item.IsDeleted = true;
            _wrapper.User.Update(item);
        }
        await _wrapper.SaveChangeAsync();
    }

    /// <summary>
    /// 查询用户拥有的角色
    /// </summary>
    /// <param name="organizationId"></param>
    /// <param name="userId">用户id</param>
    /// <returns></returns>
    public async Task<QueryRoleByUserIdResponse> QueryRoleByUserIdAsync(int organizationId,int userId)
    {
        var response = new QueryRoleByUserIdResponse();
        //拿到角色列表
        var roleIds =await _wrapper.UserRole.FindByCondition(_=>_.UserId==userId && _.OrganizationId==organizationId && _.IsDeleted==false)
            .Select(_=>_.RoleId).ToListAsync();
        if (roleIds.Any())
        {
            foreach (var roleId in roleIds)
            {
                //拿到具体的角色
                var role = _wrapper.Role.FindByCondition(_=>_.Id==roleId && _.IsDeleted==false).FirstOrDefault();
                var roleModel = _mapper.Map<RoleModel>(role);
                response.RoleModels.Add(roleModel);
            }
        }

        return response;
    }
    
    
    /// <summary>
    /// 查询所有的用户
    /// </summary>
    /// <returns></returns>
    public async Task<List<UserModel>> QueryAllUsersAsync()
    {
        var response = new List<UserModel>();
        try
        {
            var allUsers = await _redisManager.HGetAllValue<User>(RedisKeyGenerator.AllUsersRedisKey());
            if (allUsers.Any())
            {
                response = _mapper.Map<List<UserModel>>(allUsers);
                return response;
            }

            //查数据库，并设置redis；
            var users = _wrapper.User.FindAll().Where(_ => _.IsDeleted == false).ToList();
            response = _mapper.Map<List<UserModel>>(users);

            var usersDic = users.ToDictionary(_ => _.Id);

            await usersDic.ParallelForEachAsync(async user =>
            {
                var redisKey = RedisKeyGenerator.AllUsersRedisKey();
                try
                {
                    foreach (var s in users)
                    {
                        await _redisManager.HashSet(redisKey, user.Key.ToString(),
                            JsonConvert.SerializeObject(user.Value));
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
            _logger.LogError("QueryAllUsersAsync error: {e.Message}");
            throw;
        }

        return response;
    }

    public void QueryUserByCondition(QueryUserByConditionRequest request)
    {
        
    }
}
using Microsoft.AspNetCore.Mvc;
using org.huage.AuthManagement.Entity.Common;
using org.huage.AuthManagement.Entity.Managers;
using org.huage.AuthManagement.Entity.Model;
using org.huage.AuthManagement.Entity.Request;
using org.huage.AuthManagement.Entity.Response;

namespace org.huage.AuthManagement.Api.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class UserController
{
    private readonly IUserManager _manager;

    private readonly ILogger<UserController> _logger;

    public UserController(IUserManager manager, ILogger<UserController> logger)
    {
        _manager = manager;
        _logger = logger;
    }
    
    
    [HttpPost]
    public async Task<UserModel> AddUser(int organizationId,AddUserRequest request)
    {
        try
        {
            return await _manager.AddUserAsync(organizationId,request);
        }
        catch (UserException ex)
        {
            _logger.LogError(
                $"Error in org.huage.Service.UserController.UserManager.AddUser.");
            throw;
        }
        catch (Exception e)
        {
            var userException = new UserException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.UserController.UserManager.AddUser.");
            throw userException;
        }
    }
    
    [HttpPost]
    public async Task UpdateUser(UpdateUserRequest request)
    {
        try
        {
             await _manager.UpdateUserAsync(request);
        }
        catch (UserException ex)
        {
            _logger.LogError(
                $"Error in org.huage.Service.UserController.UserManager.UpdateUser.");
            throw;
        }
        catch (Exception e)
        {
            var userException = new UserException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.UserController.UserManager.UpdateUser.");
            throw userException;
        }
    }
    
    [HttpPost]
    public async Task DeleteBatchUser(List<int> ids)
    {
        try
        {
            if (! ids.Any())
            {
                return;
            }
            await _manager.DeleteBatchUserAsync(ids);
        }
        catch (UserException ex)
        {
            _logger.LogError(
                $"Error in org.huage.Service.UserController.UserManager.UpdateUser.");
            throw;
        }
        catch (Exception e)
        {
            var userException = new UserException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.UserController.UserManager.UpdateUser.");
            throw userException;
        }
    }
    
    [HttpPost]
    public async Task<QueryRoleByUserIdResponse> QueryRoleByUserId(int organizationId,int userId)
    {
        try
        {
            return await _manager.QueryRoleByUserIdAsync(organizationId,userId);
        }
        catch (UserException ex)
        {
            _logger.LogError(
                $"Error in org.huage.Service.UserController.UserManager.QueryRoleByUserId.");
            throw;
        }
        catch (Exception e)
        {
            var userException = new UserException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.UserController.UserManager.QueryRoleByUserId.");
            throw userException;
        }
    }
    
    [HttpGet]
    public async Task<List<UserModel>> QueryAllUsers()
    {
        try
        {
            return await _manager.QueryAllUsersAsync();
        }
        catch (UserException ex)
        {
            _logger.LogError(
                $"Error in org.huage.UserController.UserManager.QueryAllUsers.");
            throw;
        }
        catch (Exception e)
        {
            var userException = new UserException(e.Message);
            _logger.LogError(
                $"Error in org.huage.UserController.UserManager.QueryAllUsers.");
            throw userException;
        }
    }
}
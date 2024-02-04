using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using org.huage.AuthManagement.Entity.Common;
using org.huage.AuthManagement.Entity.Managers;
using org.huage.AuthManagement.Entity.Model;
using org.huage.AuthManagement.Entity.Request;
using org.huage.AuthManagement.Entity.Response;

namespace org.huage.AuthManagement.Api.Controllers;
[ApiController]
[Route("/[controller]/[action]")]
public class RoleController
{
    private readonly IRoleManager _manager;

    private readonly ILogger<RoleController> _logger;

    public RoleController( ILogger<RoleController> logger, IRoleManager manager)
    {
        _logger = logger;
        _manager = manager;
    }
    
    [HttpPost]
    public async Task<List<RoleModel>> QueryRoleByPage(QueryRoleRequest request)
    {
        try
        {
            return await _manager.QueryRoleByPageAsync(request);
        }
        catch (RoleException ex)
        {
            _logger.LogError(
                $"Error in org.huage.Service.RoleController.RoleController.QueryRoleByPage.");
            throw;
        }
        catch (Exception e)
        {
            var roleException = new OrganizationException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.RoleController.RoleController.QueryRoleByPage.");
            throw roleException;
        }
    }
    
    [HttpPost]
    public async Task<QueryAllRolesResponse> QueryAllRoles(QueryAllRolesRequest request)
    {
        try
        {
            return await _manager.QueryAllRolesAsync(request);
        }
        catch (RoleException ex)
        {
            _logger.LogError(
                $"Error in org.huage.Service.RoleController.RoleController.QueryAllRoles.");
            throw;
        }
        catch (Exception e)
        {
            var roleException = new OrganizationException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.RoleController.RoleController.QueryAllRoles.");
            throw roleException;
        }
    }
    
    [HttpPost]
    public async Task<QueryPermissionByRoleIdResponse> QueryPermissionByRoleId(int roleId)
    {
        try
        {
            return await _manager.QueryPermissionByRoleIdAsync(roleId);
        }
        catch (RoleException ex)
        {
            _logger.LogError(
                $"Error in org.huage.Service.RoleController.RoleController.QueryPermissionByRoleId.");
            throw;
        }
        catch (Exception e)
        {
            var roleException = new OrganizationException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.RoleController.RoleController.QueryPermissionByRoleId.");
            throw roleException;
        }
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task AddRole(AddRoleRequest request)
    {
        try
        {
            await _manager.AddRoleAsync(request);
        }
        catch (RoleException ex)
        {
            _logger.LogError(
                $"Error in org.huage.Service.RoleController.RoleController.AddRole.");
            throw;
        }
        catch (Exception e)
        {
            var roleException = new OrganizationException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.RoleController.RoleController.AddRole.");
            throw roleException;
        }
    }
    
}
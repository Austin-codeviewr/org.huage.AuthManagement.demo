using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using org.huage.AuthManagement.Entity.Common;
using org.huage.AuthManagement.Entity.Managers;
using org.huage.AuthManagement.Entity.Request;
using org.huage.AuthManagement.Entity.Response;

namespace org.huage.AuthManagement.Api.Controllers;
[ApiController]
[Route("/[controller]/[action]")]
public class PermissionController
{
    private readonly IPermissionManager _manager;

    private readonly ILogger<PermissionController> _logger;

    public PermissionController(IPermissionManager manager, ILogger<PermissionController> logger)
    {
        _manager = manager;
        _logger = logger;
    }
    
    [HttpPost]
    public async Task<QueryAllPermissionResponse> QueryAllPermission()
    {
        try
        {
            return await _manager.QueryAllPermissionAsync();
        }
        catch (PermissionException ex)
        {
            _logger.LogError(
                $"Error in org.huage.Service.PermissionManager.PermissionController.QueryAllPermission.");
            throw;
        }
        catch (Exception e)
        {
            var schedulerException = new OrganizationException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.PermissionManager.PermissionController.QueryAllPermission.");
            throw schedulerException;
        }
    }
    
    
    [HttpPost]
    public async Task AddPermission(AddPermissionRequest request)
    {
        try
        {
            await _manager.AddPermissionAsync(request);
        }
        catch (PermissionException ex)
        {
            _logger.LogError(
                $"Error in org.huage.Service.PermissionManager.PermissionController.AddPermission.");
            throw;
        }
        catch (Exception e)
        {
            var schedulerException = new OrganizationException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.PermissionManager.PermissionController.AddPermission.");
            throw schedulerException;
        }
    }
    
}
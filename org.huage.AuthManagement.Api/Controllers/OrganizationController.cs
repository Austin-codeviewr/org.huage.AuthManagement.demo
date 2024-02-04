using Microsoft.AspNetCore.Mvc;
using org.huage.AuthManagement.Entity.Common;
using org.huage.AuthManagement.Entity.Managers;
using org.huage.AuthManagement.Entity.Model;
using org.huage.AuthManagement.Entity.Request;
using org.huage.AuthManagement.Entity.Response;

namespace org.huage.AuthManagement.Api.Controllers;
[ApiController]
[Route("/[controller]/[action]")]
public class OrganizationController : Controller
{
    private readonly IOrganizationManager _organizationManager;
    private readonly ILogger<OrganizationController> _logger;

    public OrganizationController(IOrganizationManager organizationManager, ILogger<OrganizationController> logger)
    {
        _organizationManager = organizationManager;
        _logger = logger;
    }
    
    
    [HttpPost]
    public async Task<bool> OrganizationAddUser(AddOrganizationUserRequest request)
    {
        try
        {
            return await _organizationManager.OrganizationAddUserAsync(request);
        }
        catch (OrganizationException ex)
        {
            _logger.LogError(
                $"Error in org.huage.Service.OrganizationManager.OrganizationController.OrganizationAddUser. {ex.Message}");
            throw;
        }
        catch (Exception e)
        {
            var organizationExceptionException = new OrganizationException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.OrganizationManager.OrganizationController.OrganizationAddUser.");
            throw organizationExceptionException;
        }
    }
    
    [HttpPost]
    public async Task<QueryOrganizationUserAndRoleResponse> QueryOrganizationAllUserAndRole(int organizationId)
    {
        try
        {
            return await _organizationManager.QueryOrganizationAllUserAndRoleAsync(organizationId);
        }
        catch (OrganizationException ex)
        {
            _logger.LogError(
                $"Error in org.huage.Service.OrganizationManager.OrganizationController.QueryOrganizationAllUserAndRole. {ex.Message}");
            throw;
        }
        catch (Exception e)
        {
            var organizationExceptionException = new OrganizationException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.OrganizationManager.OrganizationController.QueryOrganizationAllUserAndRole.");
            throw organizationExceptionException;
        }
    }
    
    [HttpPost]
    public async Task<OrganizationModel> QueryOrganizationDetail(int organizationId)
    {
        try
        {
            return await _organizationManager.QueryOrganizationDetailAsync(organizationId);
        }
        catch (OrganizationException ex)
        {
            _logger.LogError(
                $"Error in org.huage.Service.OrganizationManager.OrganizationController.QueryOrganizationDetailAsync. {ex.Message}");
            throw;
        }
        catch (Exception e)
        {
            var organizationExceptionException = new OrganizationException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.OrganizationManager.OrganizationController.QueryOrganizationDetailAsync.");
            throw organizationExceptionException;
        }
    }
    
    [HttpPost]
    public async Task AddOrganization(AddOrganizationRequest request)
    {
        try
        {
            await _organizationManager.AddOrganizationAsync(request);
        }
        catch (OrganizationException ex)
        {
            _logger.LogError(
                $"Error in org.huage.Service.OrganizationManager.OrganizationController.AddOrganizationAsync. {ex.Message}");
            throw;
        }
        catch (Exception e)
        {
            var organizationExceptionException = new OrganizationException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.OrganizationManager.OrganizationController.AddOrganizationAsync.");
            throw organizationExceptionException;
        }
    }
    
    
    [HttpGet]
    public async Task<QueryAllOrganizationResponse> QueryAllOrganization()
    {
        try
        {
            return await _organizationManager.QueryAllOrganizationAsync();
        }
        catch (OrganizationException ex)
        {
            _logger.LogError(
                $"Error in org.huage.Service.OrganizationManager.OrganizationController.QueryAllOrganization. {ex.Message}");
            throw;
        }
        catch (Exception e)
        {
            var organizationExceptionException = new OrganizationException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.OrganizationManager.OrganizationController.QueryAllOrganization.");
            throw organizationExceptionException;
        }
    }
    
    [HttpPost]
    public async Task<bool> BatchDeleteOrganizationById(List<int> ids)
    {
        try
        {
            return await _organizationManager.BatchDeleteOrganizationByIdAsync(ids);
        }
        catch (OrganizationException ex)
        {
            _logger.LogError(
                $"Error in org.huage.Service.OrganizationManager.OrganizationController.BatchDeleteOrganizationById. {ex.Message}");
            throw;
        }
        catch (Exception e)
        {
            var organizationExceptionException = new OrganizationException(e.Message);
            _logger.LogError(
                $"Error in org.huage.Service.OrganizationManager.OrganizationController.BatchDeleteOrganizationById.");
            throw organizationExceptionException;
        }
    }
}
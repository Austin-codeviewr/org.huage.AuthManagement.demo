using System.IdentityModel.Tokens.Jwt;
using org.huage.AuthManagement.Entity.Model;
namespace org.huage.AuthManagement.BizManager.service;

public interface IJwtService
{
    Task<string> GetToken(int organizationId,UserModel userModel);

    Task<List<string>> AnalysisToken(string token);

    Task<JwtSecurityTokenHandler> VerifyToken(string token);

    Task<bool> RefreshToken(string id);
}
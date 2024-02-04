using org.huage.AuthManagement.Entity.Model;
using org.huage.AuthManagement.Entity.Request;
using org.huage.AuthManagement.Entity.Response;

namespace org.huage.AuthManagement.Entity.Managers;

public interface IUserManager
{
    Task<UserModel> AddUserAsync(int organizationId,AddUserRequest request);
    Task UpdateUserAsync(UpdateUserRequest request);
    Task DeleteBatchUserAsync(List<int> ids);
    Task<List<UserModel>>  QueryAllUsersAsync();
    void QueryUserByCondition(QueryUserByConditionRequest request);
    Task<QueryRoleByUserIdResponse> QueryRoleByUserIdAsync(int organizationId, int userId);
    Task<UserModel> QueryUserById(int id);
    Task<UserModel> QueryUserByPhone(string phone);

}
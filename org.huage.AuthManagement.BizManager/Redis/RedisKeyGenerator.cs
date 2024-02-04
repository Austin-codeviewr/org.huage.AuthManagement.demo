﻿namespace org.huage.AuthManagement.BizManager.Redis;

public static class RedisKeyGenerator
{
    private static string _prefix = string.Empty;
    public static void SetPrefix(string prefix) => _prefix = !string.IsNullOrWhiteSpace(prefix) ? $"{prefix}:" : string.Empty;
    
    public static string AllRolesRedisKey() => $"{_prefix}AllRoles";
    public static string AllUsersRedisKey() => $"{_prefix}AllUsers";
    public static string AllOrganizationsRedisKey() => $"{_prefix}AllUsers";
    public static string OrganizationUser(object id) => $"{_prefix}_{id}User";
    public static string OrganizationAllUserAndRole(object id) => $"{_prefix}{id}:OrganizationAllUserAndRole";
    public static string AllPermissions() => $"{_prefix}AllPermissions";
    public static string UserToken(object id) => $"{_prefix}UserToken:{id}";
    
    public static async Task ParallelForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> asyncAction, int maxDegreeOfParallelism = 10)
    {
        var throttler = new SemaphoreSlim(initialCount: maxDegreeOfParallelism);
        var tasks = source.Select(async item =>
        {
            await throttler.WaitAsync();
            try
            {
                await asyncAction(item).ConfigureAwait(false);
            }
            finally
            {
                throttler.Release();
            }
        });
        await Task.WhenAll(tasks);
    }

}
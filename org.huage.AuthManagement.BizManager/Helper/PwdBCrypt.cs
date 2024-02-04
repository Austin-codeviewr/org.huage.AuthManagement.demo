namespace org.huage.AuthManagement.BizManager.Helper;

public static class PwdBCrypt
{
    private const string PwdSalt = "$2a$11$jY8wPl8a0t6vaSQDz/Y1KO";
    
    /// <summary>
    /// 给密码加密
    /// </summary>
    /// <param name="pwd"></param>
    /// <returns></returns>
    public static string Encryption(string pwd)
    {
        
        return BCrypt.Net.BCrypt.HashPassword(pwd,PwdSalt);
    }
}
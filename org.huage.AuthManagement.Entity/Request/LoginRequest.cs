namespace org.huage.AuthManagement.Entity.Request;

public class LoginRequest
{
    public string Phone { get; set; }
    public string PassWord { get; set; }
    
    public int OrganizationId { get; set; }
}
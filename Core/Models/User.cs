using ClassLibrary1.Enums;
namespace ClassLibrary1.Models;




public class User
{

    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string Verification { get; set; }

    
    public Roles Role { get; set; } = Roles.User;
    
    
    
    

}
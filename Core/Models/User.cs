public class User
{
    public DateTime LastLogin { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string VerificationCode { get; set; }

    public User(DateTime lastLogin, string username, string email, string password, string verificationCode)
    {
        LastLogin = lastLogin;
        Username = username;
        Email = email;
        Password = password;
        VerificationCode = verificationCode;
    }

    public User(string username, string email, string password, string verificationCode)
    {
        LastLogin = DateTime.Now;
        Username = username;
        Email = email;
        Password = password;
        VerificationCode = verificationCode;
    }

    public User() { } 
}
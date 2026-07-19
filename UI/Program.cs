using Application.Services;
using ClassLibrary1;
using ClassLibrary1.Interfaces;

namespace UI;

public class Program
{
    static void Main()
    {
        IUserManager repository = new UserRepository();
        UserServices users = new UserServices(repository);  
        EmailService emailService = new EmailService();
        
        UserServices userServices = new UserServices(repository, emailService);
        
        users.RegisterUser("Nini Ch", "nini13@gmail.com","123456");
        userServices.VerifyUser("Nini Ch", "123456");
    }
}
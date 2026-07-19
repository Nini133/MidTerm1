using Application.Services;
using ClassLibrary1;
using ClassLibrary1.Interfaces;

namespace UI;

public class Program
{
    static void Main(string[] args)
    {
        IUserManager repository = new UserRepository();
        UserServices users = new UserServices(repository);  
        
        users.RegisterUser("Nini Ch", "nini@gmail.com","123456");
    }
}
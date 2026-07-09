using System.Text.Json;
using ClassLibrary1.Interfaces;
using ClassLibrary1.Models;

namespace ClassLibrary1;

public class UserRepository : IUserManager
{
    private readonly string _filepath = "/Users/Nini.Chachkhalia/Library/CloudStorage/OneDrive-JSCSpaceInternational,402178442/Documents/ClassLibrary1/Repository/Data/Users.txt";
    
    
    public List<User> GetAll()
    {
        string[] lines = File.ReadAllLines(_filepath);
        foreach (var line in lines)
        {
            User user = JsonSerializer.Deserialize<User>(line);
        }
    }

    public User GetUserByEmail(string Email)
    {
        throw new NotImplementedException();
    }

    public void AddUser(User user)
    {
        throw new NotImplementedException();
    }

    public void UpdateUser(User user)
    {
        throw new NotImplementedException();
    }

    public void DeleteUser(string Email)
    {
        throw new NotImplementedException();
    }
}


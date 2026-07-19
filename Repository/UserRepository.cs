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
        List<User> users = new List<User>();
        if(!File.Exists(_filepath)){
            return new List<User>();
        }
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            User user = JsonSerializer.Deserialize<User>(line);
            users.Add(user);
        }
        
        return users;
    }

    public User GetUserByEmail(string Email)
    
    {
       List<User> users = GetAll();
       var  user = users.FirstOrDefault(u => u.Email == Email);
       return user;
    }

    public void AddUser(User user)
    {
        
        string line = JsonSerializer.Serialize<User>(user);
        File.AppendAllLines(_filepath, new[] { line });
        throw new NotImplementedException();
    }

    public void UpdateUser(User user)
    {
        List<User> users = GetAll();
        int index = users.FindIndex(u => u.Email == user.Email);
        if (index != -1)
        {
            users[index] = user;
        }
        Savechanges(users);
            
        
        throw new NotImplementedException();
    }

    public void DeleteUser(string Email)
    {
        throw new NotImplementedException();
    }
    
    public void Savechanges(List<User> users)
    {
        File.Delete(_filepath);
            File.AppendAllLines(_filepath, users.Select(u => JsonSerializer.Serialize<User>(u)));
    }
    
    
}


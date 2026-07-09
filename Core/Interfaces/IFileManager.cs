using ClassLibrary1.Models;

namespace ClassLibrary1.Interfaces;

public interface IFileManager
{
    List<User> GetAll();
    
    User GetUserByEmail(String Email);
    
    void AddUser(User user);
    void UpdateUser(User user);
    void DeleteUser(String Email);
}
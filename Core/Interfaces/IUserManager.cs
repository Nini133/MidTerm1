using ClassLibrary1.Models;

namespace ClassLibrary1.Interfaces;

public interface IUserManager
{
    List<User> GetAll();
    User? GetUserByUsername(string username);
    User? GetUserById(int id);
    void AddUser(User user);
    void UpdateUser(User user);
    void DeleteUser(int id);
}
using Application.Services;
using ClassLibrary1.Enums;
using ClassLibrary1.Models;
using UI.Helpers;

namespace UI.Menus;


public class AuthMenu
{
    private readonly UserServices _userServices;

    public AuthMenu(UserServices userServices)
    {
        _userServices = userServices;
    }

    public void Register()
    {
        var username = ConsoleInput.ReadLine("მომხმარებლის სახელი: ");

        Console.Write("პაროლი: ");
        var password = ConsoleInput.ReadPassword();

        var roleInput = ConsoleInput.ReadLine("როლი (client/admin): ").Trim().ToLowerInvariant();
        var role = roleInput == "admin" ? Roles.Admin : Roles.Client;

        try
        {
            var newUser = _userServices.RegisterUser(username, password, role);
            Console.WriteLine($"რეგისტრაცია წარმატებულია! ID: {newUser.Id}, როლი: {newUser.Role}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"რეგისტრაცია ვერ მოხერხდა: {ex.Message}");
        }
    }

    public User? Login()
    {
        var username = ConsoleInput.ReadLine("მომხმარებლის სახელი: ");

        Console.Write("პაროლი: ");
        var password = ConsoleInput.ReadPassword();

        try
        {
            var user = _userServices.Login(username, password);
            Console.WriteLine($"შესვლა წარმატებულია! {user.Username}.");
            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"შესვლა ვერ მოხერხდა: {ex.Message}");
            return null;
        }
    }
}
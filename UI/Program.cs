using Application.Services;
using ClassLibrary1;
using ClassLibrary1.Enums;
using ClassLibrary1.Interfaces;

namespace UI;

public class Program
{
    static void Main()
    {
        IUserManager userManager = new UserRepository();
        var userServices = new UserServices(userManager);

        Console.WriteLine("--- ბიბლიოთეკის მართვის სისტემა --- ");

        // --- რეგისტრაცია ---
        try
        {
            var newUser = userServices.RegisterUser("nini_ch", "123456", Roles.Client);
            Console.WriteLine($"რეგისტრაცია წარმატებულია: {newUser.Username} (ID: {newUser.Id}, როლი: {newUser.Role})");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"რეგისტრაცია ვერ მოხერხდა: {ex.Message}");
        }

        // --- ავტორიზაცია ---
        try
        {
            var loggedInUser = userServices.Login("nini_ch", "123456");
            Console.WriteLine($"სისტემაში შესვლა წარმატებულია. კეთილი იყოს თქვენი მობრძანება, {loggedInUser.Username}!");
            loggedInUser.DisplayMenu();
        }
        
        catch (Exception ex)
        {
            Console.WriteLine($"შესვლა წარუმატებელია: {ex.Message}");
        }

        // --- არასწორი პაროლის მაგალითი ---
        try
        {
            userServices.Login("nini_ch", "wrong-password");
        }
        catch (Exception ex)
        
        {
            Console.WriteLine($"მოსალოდნელი შეცდომა არასწორ პაროლზე: {ex.Message}");
        }
    }
}

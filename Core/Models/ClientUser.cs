using ClassLibrary1.Enums;

namespace ClassLibrary1.Models;

public class ClientUser : User
{
    public ClientUser(int id, string username, string passwordHash, decimal fines = 0m)
        : base(id, username, passwordHash, Roles.Client, fines)
    {
    }

    public override void DisplayMenu()
    {
        Console.WriteLine();
        Console.WriteLine("---  მომხმარებლის მენიუ --- ");
        Console.WriteLine("1. კატალოგის დათვალიერება");
        Console.WriteLine("2. წიგნის ძებნა (სათაური/ავტორი)");
        Console.WriteLine("3. წიგნის გამოწერა");
        Console.WriteLine("4. წიგნის დაბრუნება");
        Console.WriteLine("5. ჩემი ჯარიმების ნახვა");
        Console.WriteLine("0. გასვლა");
    }
}
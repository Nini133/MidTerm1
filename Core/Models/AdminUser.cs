using ClassLibrary1.Enums;

namespace ClassLibrary1.Models;

public class AdminUser : User
{
    public AdminUser(int id, string username, string passwordHash, decimal fines = 0m)
        : base(id, username, passwordHash, Roles.Admin, fines)
    {
    }

    public override void DisplayMenu()
    {
        Console.WriteLine();
        Console.WriteLine("--- ადმინისტრატორის მენიუ ---");
        Console.WriteLine("1. წიგნების მართვა (დამატება/წაშლა/რაოდენობა)");
        Console.WriteLine("2. მოთხოვნების დადასტურება/უარყოფა");
        Console.WriteLine("3. ვადაგადაცილებულების შეტყობინებები");
        Console.WriteLine("0. გასვლა");
    }
}
using Application.Services;
using ClassLibrary1.Models;
using UI.Helpers;

namespace UI.Menus;


public class ClientMenu
{
    private readonly BookServices _bookServices;

    public ClientMenu(BookServices bookServices)
    {
        _bookServices = bookServices;
    }

    public void Run(User user)
    {
        while (true)
        {
            user.DisplayMenu();
            Console.Write("აირჩიეთ ფუნქციონალი: ");

            switch (Console.ReadLine())
            {
                case "1":
                    CatalogPrinter.ShowCatalog(_bookServices);
                    break;
                case "2":
                    CatalogPrinter.SearchCatalog(_bookServices);
                    break;
                case "3":
                    Console.WriteLine("ფუნქციონალი ჯერჯერობით არ არის ხელმისაწვდომი.");
                    break;
                case "4":
                    Console.WriteLine("ფუნქციონალი ჯერჯერობით არ არის ხელმისაწვდომი.");
                    break;
                case "5":
                    Console.WriteLine($"თქვენი ჯარიმა: {user.Fines:0.00} ლარი.");
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("არასწორი არჩევანი, სცადეთ ხელახლა.");
                    break;
            }
        }
    }
}
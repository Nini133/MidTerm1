using Application.Services;
using ClassLibrary1.Models;
using UI.Helpers;

namespace UI.Menus;


public class ClientMenu
{
    private readonly BookServices _bookServices;
    private readonly BorrowServices _borrowServices;

    public ClientMenu(BookServices bookServices, BorrowServices borrowServices)
    {
        _bookServices = bookServices;
        _borrowServices = borrowServices;
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
                    BorrowFlow(user);
                    break;
                case "4":
                    ReturnFlow(user);
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

    private void BorrowFlow(User user)
    {
        var isbn = ConsoleInput.ReadLine("რომელი წიგნის ISBN გსურთ გამოწერა: ");

        try
        {
            var record = _borrowServices.BorrowBook(user.Id, isbn);
            Console.WriteLine($"წიგნი წარმატებით აიღეთ. დაბრუნების ვადა: {record.DueDate:yyyy-MM-dd} (გამოწერის ID: {record.Id})");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"წიგნის აღება ვერ მოხერხდა: {ex.Message}");
        }
    }

    private void ReturnFlow(User user)
    {
        var activeBorrows = _borrowServices.GetActiveBorrowsForUser(user.Id);

        if (activeBorrows.Count == 0)
        {
            Console.WriteLine("თქვენ არ გაქვთ გამოწერილი წიგნი დასაბრუნებელი.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("--- თქვენი გამოწერილი წიგნები ---");
        foreach (var record in activeBorrows)
        {
            var overdueTag = record.IsOverdue(DateTime.Today) ? " (ვადაგადაცილებული!)" : "";
            Console.WriteLine($"{record.Id} | ISBN: {record.Isbn} | ვადა: {record.DueDate:yyyy-MM-dd}{overdueTag}");
        }

        var borrowId = ConsoleInput.ReadLine("რომელი გამოწერის ID გსურთ დაბრუნება: ");

        try
        {
            var fineCharged = _borrowServices.ReturnBook(borrowId);
            if (fineCharged > 0)
            {
                Console.WriteLine($"წიგნი დაბრუნებულია. ვადაგადაცილებისთვის დაერიცხათ ჯარიმა: {fineCharged:0.00} ლარი.");
            }
            else
            {
                Console.WriteLine("წიგნი წარმატებით დაბრუნდა, ვადაში.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"დაბრუნება ვერ მოხერხდა: {ex.Message}");
        }
    }
}
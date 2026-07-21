using Application.Services;
using ClassLibrary1;
using ClassLibrary1.Enums;
using ClassLibrary1.Interfaces;
using ClassLibrary1.Models;

namespace UI;

public class Program
{
    private static readonly IUserManager _userManager = new UserRepository();
    private static readonly UserServices _userServices = new UserServices(_userManager);

    private static readonly IBookRepository _bookRepository = new BookRepository();
    private static readonly BookServices _bookServices = new BookServices(_bookRepository);

    static void Main()
    {
        MainMenu();
    }

    private static void MainMenu()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("------ ბიბლიოთეკის მართვის სისტემა ------ ");
            Console.WriteLine("1. რეგისტრაცია");
            Console.WriteLine("2. ავტორიზაცია");
            Console.WriteLine("0. გასვლა");
            Console.Write("აირჩიეთ სასურველი მოქმედება: ");

            switch (Console.ReadLine())
            {
                case "1":
                    HandleRegister();
                    break;
                case "2":
                    HandleLogin();
                    break;
                case "0":
                    Console.WriteLine("ნახვამდის!");
                    return;
                default:
                    Console.WriteLine("არასწორი არჩევანი, სცადეთ თავიდან.");
                    break;
            }
        }
    }

    private static void HandleRegister()
    {
        Console.Write("მომხმარებლის სახელი: ");
        var username = Console.ReadLine() ?? "";

        Console.Write("პაროლი: ");
        var password = Console.ReadLine() ?? "";

        Console.Write("როლი (client/admin): ");
        var roleInput = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
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

    private static void HandleLogin()
    {
        Console.Write("მომხმარებლის სახელი: ");
        var username = Console.ReadLine() ?? "";

        Console.Write("პაროლი: ");
        var password = Console.ReadLine() ?? "";

        try
        {
            var user = _userServices.Login(username, password);
            Console.WriteLine($"შესვლა წარმატებულია!  {user.Username}.");

            if (user.Role == Roles.Admin)
            {
                AdminMenuLoop(user);
            }
            else
            {
                ClientMenuLoop(user);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"შესვლა ვერ მოხერხდა: {ex.Message}");
        }
    }

    private static void ClientMenuLoop(User user)
    {
        while (true)
        {
            user.DisplayMenu();
            Console.Write("აირჩიეთ ფუნქციონალი: ");

            switch (Console.ReadLine())
            {
                case "1":
                    ShowCatalog();
                    break;
                case "2":
                    SearchCatalog();
                    break;
                case "3":
                    Console.WriteLine("ფუნქციონალი ჯერჯერობით არ არის ხელმისაწვდომი.");
                    break;
                case "4":
                    Console.WriteLine("ფუნქციონალი ჯერჯერობით არ არის ხელმისაწვდომი");
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

    private static void AdminMenuLoop(User user)
    {
        while (true)
        {
            user.DisplayMenu();
            Console.Write("აირჩიეთ ოფცია: ");

            switch (Console.ReadLine())
            {
                case "1":
                    ManageBooksMenu();
                    break;
                case "2":
                    Console.WriteLine("მოთხოვნების დადასტურება მალე დაემატება ");
                    break;
                case "3":
                    Console.WriteLine("შეტყობინებების სისტემა მალე დაემატება.");
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("არასწორი არჩევანი, სცადეთ ხელახლა.");
                    break;
            }
        }
    }

    private static void ManageBooksMenu()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== წიგნების მართვა ===");
            Console.WriteLine("1. ახალი წიგნის დამატება");
            Console.WriteLine("2. ყველა წიგნის ნახვა");
            Console.WriteLine("3. წიგნის ძებნა");
            Console.WriteLine("4. რაოდენობის ცვლილება");
            Console.WriteLine("5. წიგნის წაშლა");
            Console.WriteLine("0. უკან დაბრუნება");
            Console.Write("აირჩიეთ ფუნქციონალი: ");

            switch (Console.ReadLine())
            {
                case "1":
                    AddBookFlow();
                    break;
                case "2":
                    ShowCatalog();
                    break;
                case "3":
                    SearchCatalog();
                    break;
                case "4":
                    ChangeQuantityFlow();
                    break;
                case "5":
                    DeleteBookFlow();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("არასწორი არჩევანი, სცადეთ ხელახლა.");
                    break;
            }
        }
    }

    private static void AddBookFlow()
    {
        Console.Write("ISBN: ");
        var isbn = Console.ReadLine() ?? "";

        Console.Write("სათაური: ");
        var title = Console.ReadLine() ?? "";

        Console.Write("ავტორი: ");
        var author = Console.ReadLine() ?? "";

        Console.Write("რაოდენობა: ");
        var quantityInput = Console.ReadLine();

        if (!int.TryParse(quantityInput, out var quantity) || quantity < 0)
        {
            Console.WriteLine("რაოდენობა უნდა იყოს დადებითი მთელი რიცხვი. დამატება ვერ მოხერხდა.");
            return;
        }

        try
        {
            _bookServices.AddBook(isbn, title, author, quantity);
            Console.WriteLine("წიგნი წარმატებით დაემატა.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"წიგნის დამატება ვერ მოხერხდა: {ex.Message}");
        }
    }

    private static void ShowCatalog()
    {
        var books = _bookServices.ListAllBooks();

        if (books.Count == 0)
        {
            Console.WriteLine("კატალოგი ცარიელია.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("--- კატალოგი ---");
        foreach (var book in books)
        {
            var availability = book.IsAvailable() ? $"ხელმისაწვდომია ({book.Quantity})" : "არ არის ხელმისაწვდომი";
            Console.WriteLine($"{book.Isbn} | {book.Title} | {book.Author} | {availability}");
        }
    }

    private static void SearchCatalog()
    {
        Console.Write("საძიებო სიტყვა (სათაური ან ავტორი): ");
        var query = Console.ReadLine() ?? "";

        var results = _bookServices.Search(query);

        if (results.Count == 0)
        {
            Console.WriteLine("შედეგი ვერ მოიძებნა.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("--- ძებნის შედეგი ---");
        foreach (var book in results)
        {
            Console.WriteLine($"{book.Isbn} | {book.Title} | {book.Author} | რაოდენობა: {book.Quantity}");
        }
    }

    private static void ChangeQuantityFlow()
    {
        Console.Write("ISBN: ");
        var isbn = Console.ReadLine() ?? "";

        var book = _bookRepository.GetByIsbn(isbn);
        if (book == null)
        {
            Console.WriteLine("წიგნი ვერ მოიძებნა.");
            return;
        }

        Console.Write("რაოდენობის ცვლილება (მაგ. 3 დასამატებლად, -2 გამოსაკლებად): ");
        var deltaInput = Console.ReadLine();

        if (!int.TryParse(deltaInput, out var delta) || delta == 0)
        {
            Console.WriteLine("არასწორი მნიშვნელობა. ცვლილება ვერ განხორციელდა.");
            return;
        }

        try
        {
            if (delta > 0)
            {
                book.IncreaseQuantity(delta);
            }
            else
            {
                book.DecreaseQuantity(-delta);
            }

            _bookRepository.UpdateBook(book);
            Console.WriteLine($"ახალი რაოდენობა: {book.Quantity}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ცვლილება ვერ განხორციელდა: {ex.Message}");
        }
    }

    private static void DeleteBookFlow()
    {
        Console.Write("წასაშლელი წიგნის ISBN: ");
        var isbn = Console.ReadLine() ?? "";

        try
        {
            _bookRepository.DeleteBook(isbn);
            Console.WriteLine("წიგნი წაიშალა.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"წაშლა ვერ მოხერხდა: {ex.Message}");
        }
    }
}
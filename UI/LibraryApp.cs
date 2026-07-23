using Application.Services;
using ClassLibrary1;
using ClassLibrary1.Enums;
using ClassLibrary1.Interfaces;
using UI.Menus;

namespace UI;

public class LibraryApp
{
    private readonly AuthMenu _authMenu;
    private readonly ClientMenu _clientMenu;
    private readonly AdminMenu _adminMenu;

    public LibraryApp()
    {
        IUserManager userManager = new UserRepository();
        var userServices = new UserServices(userManager);

        IBookRepository bookRepository = new BookRepository();
        var bookServices = new BookServices(bookRepository);

        _authMenu = new AuthMenu(userServices);
        _clientMenu = new ClientMenu(bookServices);
        _adminMenu = new AdminMenu(new BookManagementMenu(bookRepository, bookServices));
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("------ ბიბლიოთეკის მართვის სისტემა ------");
            Console.WriteLine("1. რეგისტრაცია");
            Console.WriteLine("2. ავტორიზაცია");
            Console.WriteLine("0. გასვლა");
            Console.Write("აირჩიეთ სასურველი მოქმედება: ");

            switch (Console.ReadLine())
            {
                case "1":
                    _authMenu.Register();
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

    private void HandleLogin()
    {
        var user = _authMenu.Login();
        if (user == null)
        {
            return; 
        }

        if (user.Role == Roles.Admin)
        {
            _adminMenu.Run(user);
        }
        else
        {
            _clientMenu.Run(user);
        }
    }
}
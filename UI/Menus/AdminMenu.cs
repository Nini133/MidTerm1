using ClassLibrary1.Models;

namespace UI.Menus;


public class AdminMenu
{
    private readonly BookManagementMenu _bookManagementMenu;

    public AdminMenu(BookManagementMenu bookManagementMenu)
    {
        _bookManagementMenu = bookManagementMenu;
    }

    public void Run(User user)
    {
        while (true)
        {
            user.DisplayMenu();
            Console.Write("აირჩიეთ ოფცია: ");

            switch (Console.ReadLine())
            {
                case "1":
                    _bookManagementMenu.Run();
                    break;
                case "2":
                    Console.WriteLine("მოთხოვნების დადასტურება მალე დაემატება.");
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
}
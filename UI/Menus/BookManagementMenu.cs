using Application.Services;
using ClassLibrary1.Interfaces;
using UI.Helpers;

namespace UI.Menus;


public class BookManagementMenu
{
    private readonly IBookRepository _bookRepository;
    private readonly BookServices _bookServices;

    public BookManagementMenu(IBookRepository bookRepository, BookServices bookServices)
    {
        _bookRepository = bookRepository;
        _bookServices = bookServices;
    }

    public void Run()
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
                    CatalogPrinter.ShowCatalog(_bookServices);
                    break;
                case "3":
                    CatalogPrinter.SearchCatalog(_bookServices);
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

    private void AddBookFlow()
    {
        var isbn = ConsoleInput.ReadLine("ISBN: ");
        var title = ConsoleInput.ReadLine("სათაური: ");
        var author = ConsoleInput.ReadLine("ავტორი: ");
        var quantityInput = ConsoleInput.ReadLine("რაოდენობა: ");

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

    private void ChangeQuantityFlow()
    {
        var isbn = ConsoleInput.ReadLine("ISBN: ");

        var book = _bookRepository.GetByIsbn(isbn);
        if (book == null)
        {
            Console.WriteLine("წიგნი ვერ მოიძებნა.");
            return;
        }

        var deltaInput = ConsoleInput.ReadLine("რაოდენობის ცვლილება (მაგ. 3 დასამატებლად, -2 გამოსაკლებად): ");

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

    private void DeleteBookFlow()
    {
        var isbn = ConsoleInput.ReadLine("წასაშლელი წიგნის ISBN: ");

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
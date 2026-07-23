using Application.Services;

namespace UI.Helpers;


public static class CatalogPrinter
{
    public static void ShowCatalog(BookServices bookServices)
    {
        var books = bookServices.ListAllBooks();

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

    public static void SearchCatalog(BookServices bookServices)
    {
        var query = ConsoleInput.ReadLine("საძიებო სიტყვა (სათაური ან ავტორი): ");

        var results = bookServices.Search(query);

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
}
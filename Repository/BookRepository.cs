using ClassLibrary1.Interfaces;
using ClassLibrary1.Models;

namespace ClassLibrary1;


public class BookRepository : IBookRepository
{
    private readonly string _filePath;

    public BookRepository(string? filePath = null)
    {
        _filePath = filePath ?? ResolveDefaultPath();

        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(_filePath))
        {
            File.Create(_filePath).Dispose();
        }
    }


    private static string ResolveDefaultPath()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);

        while (dir != null && dir.GetFiles("*.sln").Length == 0)
        {
            dir = dir.Parent;
        }

        var solutionRoot = dir?.FullName ?? AppContext.BaseDirectory;
        return Path.Combine(solutionRoot, "Repository", "Data", "Books.txt");
    }

    public List<Book> GetAll()
    {
        var books = new List<Book>();

        try
        {
            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var book = ParseLine(line);
                if (book != null)
                {
                    books.Add(book);
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"წიგნების ფაილის წაკითხვისას მოხდა შეცდომა: {ex.Message}");
        }

        return books;
    }

    public Book? GetByIsbn(string isbn)
    {
        return GetAll().FirstOrDefault(b => b.Isbn.Equals(isbn, StringComparison.OrdinalIgnoreCase));
    }

    public List<Book> SearchByTitle(string titleQuery)
    {
        if (string.IsNullOrWhiteSpace(titleQuery)) return new List<Book>();

        return GetAll()
            .Where(b => b.Title.Contains(titleQuery, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public List<Book> SearchByAuthor(string authorQuery)
    {
        if (string.IsNullOrWhiteSpace(authorQuery)) return new List<Book>();

        return GetAll()
            .Where(b => b.Author.Contains(authorQuery, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public void AddBook(Book book)
    {
        try
        {
            File.AppendAllLines(_filePath, new[] { ToLine(book) });
        }
        catch (IOException ex)
        {
            Console.WriteLine($"წიგნის შენახვისას მოხდა შეცდომა: {ex.Message}");
            throw;
        }
    }

    public void UpdateBook(Book book)
    {
        var books = GetAll();
        var index = books.FindIndex(b => b.Isbn.Equals(book.Isbn, StringComparison.OrdinalIgnoreCase));
        if (index == -1)
        {
            throw new InvalidOperationException($"წიგნი ISBN={book.Isbn} ვერ მოიძებნა.");
        }

        books[index] = book;
        SaveChanges(books);
    }

    public void DeleteBook(string isbn)
    {
        var books = GetAll();
        var removed = books.RemoveAll(b => b.Isbn.Equals(isbn, StringComparison.OrdinalIgnoreCase));
        if (removed == 0)
        {
            throw new InvalidOperationException($"წიგნი ISBN={isbn} ვერ მოიძებნა.");
        }

        SaveChanges(books);
    }

    private void SaveChanges(List<Book> books)
    {
        try
        {
            File.WriteAllLines(_filePath, books.Select(ToLine));
        }
        catch (IOException ex)
        {
            Console.WriteLine($"ფაილში ცვლილებების შენახვისას მოხდა შეცდომა: {ex.Message}");
            throw;
        }
    }

    private static string ToLine(Book book)
    {
        return $"{book.Isbn}|{book.Title}|{book.Author}|{book.Quantity}";
    }

    private static Book? ParseLine(string line)
    {
        var parts = line.Split('|');
        if (parts.Length != 4)
        {
            Console.WriteLine($"missing the line: {line}");
            return null;
        }

        try
        {
            var isbn = parts[0].Trim();
            var title = parts[1].Trim();
            var author = parts[2].Trim();
            var quantity = int.Parse(parts[3].Trim());

            return new Book(isbn, title, author, quantity);
        }
        catch (FormatException)
        {
            Console.WriteLine($"missing the line: {line}");
            return null;
        }
    }
}
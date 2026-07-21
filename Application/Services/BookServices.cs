using ClassLibrary1.Interfaces;
using ClassLibrary1.Models;

namespace Application.Services;


public class BookServices
{
    private readonly IBookRepository _bookRepository;

    public BookServices(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public Book AddBook(string isbn, string title, string author, int quantity)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN სავალდებულოა.");

        if (_bookRepository.GetByIsbn(isbn) != null)
            throw new InvalidOperationException($"წიგნი ISBN='{isbn}'-ით უკვე არსებობს.");

        var book = new Book(isbn, title, author, quantity);
        _bookRepository.AddBook(book);
        return book;
    }

    public List<Book> ListAllBooks()
    {
        return _bookRepository.GetAll();
    }

    public List<Book> SearchByTitle(string titleQuery)
    {
        return _bookRepository.SearchByTitle(titleQuery);
    }

    public List<Book> SearchByAuthor(string authorQuery)
    {
        return _bookRepository.SearchByAuthor(authorQuery);
    }


    public List<Book> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return new List<Book>();

        var byTitle = SearchByTitle(query);
        var byAuthor = SearchByAuthor(query);

        return byTitle
            .Union(byAuthor, Comparer())
            .ToList();
    }

    private static IEqualityComparer<Book> Comparer()
    {
        return new BookIsbnComparer();
    }

    private class BookIsbnComparer : IEqualityComparer<Book>
    {
        public bool Equals(Book? x, Book? y) =>
            x != null && y != null && x.Isbn.Equals(y.Isbn, StringComparison.OrdinalIgnoreCase);

        public int GetHashCode(Book obj) => obj.Isbn.ToLowerInvariant().GetHashCode();
    }
}
using ClassLibrary1.Models;

namespace ClassLibrary1.Interfaces;

public interface IBookRepository
{
    List<Book> GetAll();
    Book? GetByIsbn(string isbn);
    List<Book> SearchByTitle(string titleQuery);
    List<Book> SearchByAuthor(string authorQuery);
    void AddBook(Book book);
    void UpdateBook(Book book);
    void DeleteBook(string isbn);
}
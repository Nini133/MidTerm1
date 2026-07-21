namespace ClassLibrary1.Models;

/// <summary>
/// Represents a book title in the catalog. Quantity is encapsulated -
/// it can only change through IncreaseQuantity/DecreaseQuantity, so a
/// borrow/return can never push it below zero from outside code.
/// </summary>
public class Book
{
    public string Isbn { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public int Quantity { get; private set; }

    public Book(string isbn, string title, string author, int quantity)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN არ შეიძლება იყოს ცარიელი.", nameof(isbn));
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("სათაური არ შეიძლება იყოს ცარიელი.", nameof(title));
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("ავტორი არ შეიძლება იყოს ცარიელი.", nameof(author));
        if (quantity < 0)
            throw new ArgumentException("რაოდენობა არ შეიძლება იყოს უარყოფითი.", nameof(quantity));

        Isbn = isbn;
        Title = title;
        Author = author;
        Quantity = quantity;
    }

    public bool IsAvailable() => Quantity > 0;

  
    public void IncreaseQuantity(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("დასამატებელი რაოდენობა უნდა იყოს დადებითი.", nameof(amount));
        Quantity += amount;
    }

    public void DecreaseQuantity(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("გამოსაკლები რაოდენობა უნდა იყოს დადებითი.", nameof(amount));
        if (amount > Quantity)
            throw new InvalidOperationException("ბიბლიოთეკაში საკმარისი რაოდენობა არ არის.");
        Quantity -= amount;
    }
}
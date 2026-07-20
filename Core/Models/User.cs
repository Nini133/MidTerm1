using ClassLibrary1.Enums;

namespace ClassLibrary1.Models;


public abstract class User
{
    public int Id { get; private set; }
    public string Username { get; private set; }

    public string PasswordHash { get;  set; }

    public Roles Role { get; private set; }
    public decimal Fines { get; private set; }
    

    protected User(int id, string username, string passwordHash, Roles role, decimal fines = 0m)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty.", nameof(username));
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash cannot be empty.", nameof(passwordHash));
        if (fines < 0)
            throw new ArgumentException("Fines cannot be negative.", nameof(fines));

        Id = id;
        Username = username;
        PasswordHash = passwordHash;
        Role = role;
        Fines = fines;
    }

    public void AddFine(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Fine amount must be positive.", nameof(amount));
        Fines += amount;
    }

    public void PayFine(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Payment amount must be positive.", nameof(amount));
        if (amount > Fines)
            throw new InvalidOperationException("Payment exceeds outstanding fines.");
        Fines -= amount;
    }

    public bool HasUnpaidFines() => Fines > 0;
    
    public abstract void DisplayMenu();
}
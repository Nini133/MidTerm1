using ClassLibrary1.Interfaces;
using ClassLibrary1.Models;

namespace Application.Services;



public class BorrowServices
{
    private const int DefaultLoanDays = 14;
    private const decimal FinePerLateDay = 1.00m;

    private readonly IBorrowRepository _borrowRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IUserManager _userManager;

    public BorrowServices(IBorrowRepository borrowRepository, IBookRepository bookRepository, IUserManager userManager)
    {
        _borrowRepository = borrowRepository;
        _bookRepository = bookRepository;
        _userManager = userManager;
    }

    public BorrowRecord BorrowBook(int userId, string isbn)
    {
        var user = _userManager.GetUserById(userId)
            ?? throw new InvalidOperationException("მომხმარებელი ვერ მოიძებნა.");

        if (user.HasUnpaidFines())
            throw new InvalidOperationException($"თქვენ გაქვთ დაუფარავი ჯარიმა ({user.Fines:0.00} ლარი) - ახალი წიგნის აღება შეუძლებელია, სანამ არ დაფარავთ.");

        var book = _bookRepository.GetByIsbn(isbn)
            ?? throw new InvalidOperationException("წიგნი ვერ მოიძებნა.");

        if (!book.IsAvailable())
            throw new InvalidOperationException("წიგნი ამჟამად არ არის ხელმისაწვდომი.");

        book.DecreaseQuantity(1);
        _bookRepository.UpdateBook(book);

        var record = new BorrowRecord(
            id: GenerateNextId(),
            userId: userId,
            isbn: isbn,
            dueDate: DateTime.Today.AddDays(DefaultLoanDays));

        _borrowRepository.AddBorrowRecord(record);
        return record;
    }


    public decimal ReturnBook(string borrowId)
    {
        var record = _borrowRepository.GetById(borrowId)
            ?? throw new InvalidOperationException("გამოწერა ვერ მოიძებნა.");

        if (record.Status == ClassLibrary1.Enums.BorrowStatus.Returned)
            throw new InvalidOperationException("ეს წიგნი უკვე დაბრუნებულია.");

        var today = DateTime.Today;
        var lateDays = record.DaysLate(today);

        record.MarkReturned(today);
        _borrowRepository.UpdateBorrowRecord(record);

        var book = _bookRepository.GetByIsbn(record.Isbn);
        if (book != null)
        {
            book.IncreaseQuantity(1);
            _bookRepository.UpdateBook(book);
        }

        var fineCharged = 0m;
        if (lateDays > 0)
        {
            fineCharged = lateDays * FinePerLateDay;
            var user = _userManager.GetUserById(record.UserId);
            if (user != null)
            {
                user.AddFine(fineCharged);
                _userManager.UpdateUser(user);
            }
        }

        return fineCharged;
    }

    public List<BorrowRecord> GetActiveBorrowsForUser(int userId)
    {
        return _borrowRepository.GetActiveByUserId(userId);
    }

    private string GenerateNextId()
    {
        var existingNumbers = _borrowRepository.GetAll()
            .Select(r => r.Id)
            .Where(id => id.StartsWith("B") && int.TryParse(id.Substring(1), out _))
            .Select(id => int.Parse(id.Substring(1)))
            .ToList();

        var next = existingNumbers.Count == 0 ? 201 : existingNumbers.Max() + 1;
        return $"B{next}";
    }
}
using ClassLibrary1.Enums;

namespace ClassLibrary1.Models;


public class BorrowRecord
{
    public string Id { get; private set; }
    public int UserId { get; private set; }
    public string Isbn { get; private set; }
    public DateTime DueDate { get; private set; }
    public BorrowStatus Status { get; private set; }
    public DateTime? ReturnedDate { get; private set; }

    public BorrowRecord(
        string id,
        int userId,
        string isbn,
        DateTime dueDate,
        BorrowStatus status = BorrowStatus.Active,
        DateTime? returnedDate = null)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Borrow ID არ შეიძლება იყოს ცარიელი.", nameof(id));
        if (userId <= 0)
            throw new ArgumentException("UserId უნდა იყოს დადებითი.", nameof(userId));
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN არ შეიძლება იყოს ცარიელი.", nameof(isbn));

        Id = id;
        UserId = userId;
        Isbn = isbn;
        DueDate = dueDate.Date;
        Status = status;
        ReturnedDate = returnedDate;
    }

    public bool IsOverdue(DateTime asOf) => Status == BorrowStatus.Active && asOf.Date > DueDate;

    public int DaysLate(DateTime asOf)
    {
        var effectiveDate = (ReturnedDate ?? asOf).Date;
        var lateDays = (effectiveDate - DueDate).Days;
        return lateDays > 0 ? lateDays : 0;
    }

    public void MarkReturned(DateTime returnedOn)
    {
        if (Status == BorrowStatus.Returned)
            throw new InvalidOperationException("წიგნი უკვე დაბრუნებულია.");

        Status = BorrowStatus.Returned;
        ReturnedDate = returnedOn.Date;
    }
}
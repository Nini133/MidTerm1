using ClassLibrary1.Models;

namespace ClassLibrary1.Interfaces;

public interface IBorrowRepository
{
    List<BorrowRecord> GetAll();
    BorrowRecord? GetById(string id);
    List<BorrowRecord> GetByUserId(int userId);
    List<BorrowRecord> GetActiveByUserId(int userId);
    void AddBorrowRecord(BorrowRecord record);
    void UpdateBorrowRecord(BorrowRecord record);
}
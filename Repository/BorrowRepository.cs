using System.Globalization;
using ClassLibrary1.Enums;
using ClassLibrary1.Interfaces;
using ClassLibrary1.Models;

namespace ClassLibrary1;


public class BorrowRepository : IBorrowRepository
{
    private const string DateFormat = "yyyy-MM-dd";
    private readonly string _filePath;

    public BorrowRepository(string? filePath = null)
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
        return Path.Combine(solutionRoot, "Repository", "Data", "Borrows.txt");
    }

    public List<BorrowRecord> GetAll()
    {
        var records = new List<BorrowRecord>();

        try
        {
            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var record = ParseLine(line);
                if (record != null)
                {
                    records.Add(record);
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"გამოწერების ფაილის წაკითხვისას მოხდა შეცდომა: {ex.Message}");
        }

        return records;
    }

    public BorrowRecord? GetById(string id)
    {
        return GetAll().FirstOrDefault(r => r.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
    }

    public List<BorrowRecord> GetByUserId(int userId)
    {
        return GetAll().Where(r => r.UserId == userId).ToList();
    }

    public List<BorrowRecord> GetActiveByUserId(int userId)
    {
        return GetAll()
            .Where(r => r.UserId == userId && r.Status == BorrowStatus.Active)
            .ToList();
    }

    public void AddBorrowRecord(BorrowRecord record)
    {
        try
        {
            File.AppendAllLines(_filePath, new[] { ToLine(record) });
        }
        catch (IOException ex)
        {
            Console.WriteLine($"გამოწერის შენახვისას მოხდა შეცდომა: {ex.Message}");
            throw;
        }
    }

    public void UpdateBorrowRecord(BorrowRecord record)
    {
        var records = GetAll();
        var index = records.FindIndex(r => r.Id.Equals(record.Id, StringComparison.OrdinalIgnoreCase));
        if (index == -1)
        {
            throw new InvalidOperationException($"გამოწერა ID={record.Id} ვერ მოიძებნა.");
        }

        records[index] = record;
        SaveChanges(records);
    }

    private void SaveChanges(List<BorrowRecord> records)
    {
        try
        {
            File.WriteAllLines(_filePath, records.Select(ToLine));
        }
        catch (IOException ex)
        {
            Console.WriteLine($"ფაილში ცვლილებების შენახვისას მოხდა შეცდომა: {ex.Message}");
            throw;
        }
    }

    private static string ToLine(BorrowRecord record)
    {
        var returnedDateText = record.ReturnedDate.HasValue
            ? record.ReturnedDate.Value.ToString(DateFormat, CultureInfo.InvariantCulture)
            : "";

        return $"{record.Id}|{record.UserId}|{record.Isbn}|{record.DueDate.ToString(DateFormat, CultureInfo.InvariantCulture)}|{record.Status}|{returnedDateText}";
    }

    private static BorrowRecord? ParseLine(string line)
    {
        var parts = line.Split('|');
        if (parts.Length != 6)
        {
            Console.WriteLine($"არასწორი ფორმატის ხაზი გამოტოვებულია: {line}");
            return null;
        }

        try
        {
            var id = parts[0].Trim();
            var userId = int.Parse(parts[1].Trim());
            var isbn = parts[2].Trim();
            var dueDate = DateTime.ParseExact(parts[3].Trim(), DateFormat, CultureInfo.InvariantCulture);
            var status = Enum.Parse<BorrowStatus>(parts[4].Trim());
            var returnedDateText = parts[5].Trim();
            DateTime? returnedDate = string.IsNullOrEmpty(returnedDateText)
                ? null
                : DateTime.ParseExact(returnedDateText, DateFormat, CultureInfo.InvariantCulture);

            return new BorrowRecord(id, userId, isbn, dueDate, status, returnedDate);
        }
        catch (FormatException)
        {
            Console.WriteLine($"არასწორი ფორმატის ხაზი გამოტოვებულია: {line}");
            return null;
        }
    }
}
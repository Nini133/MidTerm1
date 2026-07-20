using System.Globalization;
using ClassLibrary1.Enums;
using ClassLibrary1.Interfaces;
using ClassLibrary1.Models;

namespace ClassLibrary1;


public class UserRepository : IUserManager
{
    private readonly string _filePath;

    public UserRepository(string? filePath = null)
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

        // TEMPORARY DEBUG LINE - remove once you confirm the path is correct.
        Console.WriteLine($"[DEBUG] Users.txt path: {_filePath}");
    }

  
    private static string ResolveDefaultPath()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);

        while (dir != null && dir.GetFiles("*.sln").Length == 0)
        {
            dir = dir.Parent;
        }

        var solutionRoot = dir?.FullName ?? AppContext.BaseDirectory;
        return Path.Combine(solutionRoot, "Repository", "Data", "Users.txt");
    }

    public List<User> GetAll()
    {
        var users = new List<User>();

        try
        {
            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var user = ParseLine(line);
                if (user != null)
                {
                    users.Add(user);
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"მომხმარებლების ფაილის წაკითხვისას მოხდა შეცდომა: {ex.Message}");
        }

        return users;
    }

    public User? GetUserByUsername(string username)
    {
        return GetAll().FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }

    public User? GetUserById(int id)
    {
        return GetAll().FirstOrDefault(u => u.Id == id);
    }

    public void AddUser(User user)
    {
        try
        {
            File.AppendAllLines(_filePath, new[] { ToLine(user) });
        }
        catch (IOException ex)
        {
            Console.WriteLine($"მომხმარებლის შენახვისას მოხდა შეცდომა: {ex.Message}");
            throw;
        }
    }

    public void UpdateUser(User user)
    {
        var users = GetAll();
        var index = users.FindIndex(u => u.Id == user.Id);
        if (index == -1)
        {
            throw new InvalidOperationException($"მომხმარებელი ID={user.Id} ვერ მოიძებნა.");
        }

        users[index] = user;
        SaveChanges(users);
    }

    public void DeleteUser(int id)
    {
        var users = GetAll();
        var removed = users.RemoveAll(u => u.Id == id);
        if (removed == 0)
        {
            throw new InvalidOperationException($"მომხმარებელი ID={id} ვერ მოიძებნა.");
        }

        SaveChanges(users);
    }

    private void SaveChanges(List<User> users)
    {
        try
        {
            File.WriteAllLines(_filePath, users.Select(ToLine));
        }
        catch (IOException ex)
        {
            Console.WriteLine($"ფაილში ცვლილებების შენახვისას მოხდა შეცდომა: {ex.Message}");
            throw;
        }
    }

    private static string ToLine(User user)
    {
        var roleText = user.Role == Roles.Admin ? "admin" : "client";
        return $"{user.Id}|{user.Username}|{user.PasswordHash}|{roleText}|{user.Fines.ToString("0.00", CultureInfo.InvariantCulture)}";
    }

    private static User? ParseLine(string line)
    {
        var parts = line.Split('|');
        if (parts.Length != 5)
        {
            Console.WriteLine($"ფორმატის ხაზი გამოტოვებულია: {line}");
            return null;
        }

        try
        {
            var id = int.Parse(parts[0].Trim());
            var username = parts[1].Trim();
            var passwordHash = parts[2].Trim();
            var roleText = parts[3].Trim().ToLowerInvariant();
            var fines = decimal.Parse(parts[4].Trim(), CultureInfo.InvariantCulture);

            return roleText == "admin"
                ? new AdminUser(id, username, passwordHash, fines)
                : new ClientUser(id, username, passwordHash, fines);
        }
        catch (FormatException)
        {
            Console.WriteLine($"ფორმატის ხაზი გამოტოვებულია: {line}");
            return null;
        }
    }
}
using SQLite;
using ShuleLink.Models;
using System.Security.Cryptography;
using System.Text;

namespace ShuleLink.Data
{
    public class DbInitializer
    {
        private readonly SQLiteAsyncConnection _database;

        public DbInitializer(SQLiteAsyncConnection database)
        {
            _database = database;
        }

        public async Task SeedDataAsync()
        {
            var existingUsers = await _database.Table<User>().CountAsync();
            
            if (existingUsers == 0)
            {
                var defaultUsers = new List<User>
                {
                    new User
                    {
                        Name = "Shule Student 1",
                        PhoneNumber = "254758024400",
                        Password = HashPassword("password123"),
                        AdmissionNumber = "12345",
                        Grade = "5",
                        Class = "5 East",
                        ParentName = "Shule Parent 1",
                        HomeAddress = "Olkalou, Salient",
                        Status = "active",
                        UserType = "student",
                        Graduated = false,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new User
                    {
                        Name = "Shule Student 2",
                        PhoneNumber = "254758024401",
                        Password = HashPassword("password123"),
                        AdmissionNumber = "12346",
                        Grade = "6",
                        Class = "6 West",
                        ParentName = "Shule Parent 2",
                        HomeAddress = "Nakuru, Kenya",
                        Status = "active",
                        UserType = "student",
                        Graduated = false,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new User
                    {
                        Name = "Shule Student 3",
                        PhoneNumber = "254758024402",
                        Password = HashPassword("password123"),
                        AdmissionNumber = "12347",
                        Grade = "7",
                        Class = "7 North",
                        ParentName = "Shule Parent 3",
                        HomeAddress = "Eldoret, Kenya",
                        Status = "active",
                        UserType = "student",
                        Graduated = false,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new User
                    {
                        Name = "Shule Student 4",
                        PhoneNumber = "254758024403",
                        Password = HashPassword("password123"),
                        AdmissionNumber = "12348",
                        Grade = "8",
                        Class = "8 South",
                        ParentName = "Shule Parent 4",
                        HomeAddress = "Kisumu, Kenya",
                        Status = "active",
                        UserType = "student",
                        Graduated = false,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new User
                    {
                        Name = "Shule Student 5",
                        PhoneNumber = "254758024404",
                        Password = HashPassword("password123"),
                        AdmissionNumber = "12349",
                        Grade = "4",
                        Class = "4 Central",
                        ParentName = "Shule Parent 5",
                        HomeAddress = "Mombasa, Kenya",
                        Status = "active",
                        UserType = "student",
                        Graduated = false,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    }
                };

                foreach (var user in defaultUsers)
                {
                    await _database.InsertAsync(user);
                }
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "ShuleLink_Salt"));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}

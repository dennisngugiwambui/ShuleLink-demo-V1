using SQLite;
using ShuleLink.Models;

namespace ShuleLink.Data
{
    public class ShuleLinkDbContext
    {
        private SQLiteAsyncConnection? _database;
        private readonly string _databasePath;

        public ShuleLinkDbContext()
        {
            _databasePath = Path.Combine(FileSystem.AppDataDirectory, "ShuleLink.db3");
        }

        public async Task<SQLiteAsyncConnection> GetConnectionAsync()
        {
            if (_database is not null)
                return _database;

            _database = new SQLiteAsyncConnection(_databasePath);
            await InitializeDatabaseAsync();
            return _database;
        }

        private async Task InitializeDatabaseAsync()
        {
            if (_database == null) return;

            // Create tables
            await _database.CreateTableAsync<User>();
            
            // Initialize default data
            var initializer = new DbInitializer(_database);
            await initializer.SeedDataAsync();
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var db = await GetConnectionAsync();
            return await db.Table<User>().ToListAsync();
        }

        public async Task<User?> GetUserAsync(int id)
        {
            var db = await GetConnectionAsync();
            return await db.Table<User>().Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByPhoneAsync(string phoneNumber)
        {
            var db = await GetConnectionAsync();
            return await db.Table<User>().Where(u => u.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
        }

        public async Task<int> SaveUserAsync(User user)
        {
            var db = await GetConnectionAsync();
            user.UpdatedAt = DateTime.Now;

            if (user.Id != 0)
                return await db.UpdateAsync(user);
            else
            {
                user.CreatedAt = DateTime.Now;
                return await db.InsertAsync(user);
            }
        }

        public async Task<int> DeleteUserAsync(User user)
        {
            var db = await GetConnectionAsync();
            return await db.DeleteAsync(user);
        }

        public async Task<bool> UserExistsAsync(string phoneNumber, string admissionNumber)
        {
            var db = await GetConnectionAsync();
            var existingUser = await db.Table<User>()
                .Where(u => u.PhoneNumber == phoneNumber || u.AdmissionNumber == admissionNumber)
                .FirstOrDefaultAsync();
            return existingUser != null;
        }
    }
}

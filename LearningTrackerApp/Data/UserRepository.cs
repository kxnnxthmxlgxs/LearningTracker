using Dapper;
using LearningTrackerApp.Models;
using System.Data;

namespace LearningTrackerApp.Data
{
    public class UserRepository
    {
        private readonly IDbConnection _db;

        public UserRepository(IDbConnection db)
        {
            _db = db;
        }

        // Save a new user
        public async Task CreateUser(string username, string password)
        {
            var sql = "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @Password)";
            await _db.ExecuteAsync(sql, new { Username = username, Password = password });
        }

        // Find a user by username (for Login)
        public async Task<User?> GetUserByUsername(string username)
        {
            var sql = "SELECT * FROM Users WHERE Username = @Username";
            return await _db.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
        }
    }
}

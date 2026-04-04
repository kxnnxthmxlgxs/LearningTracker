using Dapper;
using LearningTrackerApp.Models;
using System.Data;

namespace LearningTrackerApp.Data
{
    public class LearningRepository
    {
        private readonly IDbConnection _db;

        public LearningRepository(IDbConnection db)
        {
            _db = db;
        }

        // Get all items from the SQL table
        public async Task<IEnumerable<LearningItem>> GetAllItems(string search = "")
        {
            var sql = "SELECT * FROM LearningItems WHERE Topic LIKE @Search ORDER BY DateLearned DESC";
            // The '%' signs tell SQL to find the text anywhere in the string
            return await _db.QueryAsync<LearningItem>(sql, new { Search = "%" + search + "%" });
        }

        // Add a new item to the SQL table
        public async Task AddItem(string topic, string category)
        {
            var sql = "INSERT INTO LearningItems (Topic, Category, DateLearned, IsCompleted) VALUES (@Topic, @Category, @Date, 0)";
            await _db.ExecuteAsync(sql, new { Topic = topic, Category = category, Date = DateTime.Now });
        }

        public async Task DeleteItem(int id)
        {
            var sql = "DELETE FROM LearningItems WHERE Id = @Id";
            await _db.ExecuteAsync(sql, new { Id = id });
        }

        public async Task CompleteItem(int id)
        {
            var sql = "UPDATE LearningItems SET IsCompleted = 1 WHERE Id = @Id";
            await _db.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<(int Total, int Completed, string TopCategory)> GetStats()
{
    // 1. Get total count
    var total = await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM LearningItems");

    // 2. Get completed count
    var completed = await _db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM LearningItems WHERE IsCompleted = 1");

    // 3. Get the category with the most entries
    var topCatSql = "SELECT TOP 1 Category FROM LearningItems GROUP BY Category ORDER BY COUNT(*) DESC";
    var topCat = await _db.QueryFirstOrDefaultAsync<string>(topCatSql) ?? "None";

    return (total, completed, topCat);
}
    }
}

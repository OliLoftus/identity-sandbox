using Identity.Oli.Models;
using MongoDB.Driver;

namespace Identity.Oli.Data;

// Handles all database operations for the Goals collection.
public class GoalsRepository(MongoDbContext context) : IGoalsRepository
{
        private readonly IMongoCollection<GoalModel> _goals = context.Goals; // MongoDB collection for goals
        
        public async Task<List<GoalModel>> GetAllAsync()
        {
                return await _goals.Find(_ => true).ToListAsync();
        }
        
        public async Task<GoalModel> GetByIdAsync(Guid id)
        {
                return await _goals.Find(goal => goal.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(GoalModel goal)
        {
                await _goals.InsertOneAsync(goal);
        }

        public async Task UpdateAsync(Guid id, GoalModel updatedGoal)
        {
                await _goals.ReplaceOneAsync(goal => goal.Id == id, updatedGoal);
        }

        public async Task DeleteAsync(Guid id)
        {
                await _goals.DeleteOneAsync(goal => goal.Id == id);
        }
}

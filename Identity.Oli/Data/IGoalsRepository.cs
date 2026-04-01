using Identity.Oli.Models;
using Identity.Oli.Models.Requests;

namespace Identity.Oli.Data;

public interface IGoalsRepository
{
    Task<List<GoalModel>> GetAllAsync(); // Retrieve all goals
    Task<List<GoalModel>> GetAllAsync(GoalQuery query); // Retrieve goals by filters
    Task<(List<GoalModel> Items, long TotalCount)> GetAllWithCountAsync(GoalQuery query); // Retrieve goals with total count
    Task<GoalModel> GetByIdAsync(Guid id); // Retrieve a goal by its ID
    Task CreateAsync(GoalModel goal); // Create a new goal
    Task UpdateAsync(Guid id, GoalModel updatedGoal); // Update an existing goal
    Task DeleteAsync(Guid id); // Delete a goal by its ID
}

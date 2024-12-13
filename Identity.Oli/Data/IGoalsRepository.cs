using Identity.Oli.Models;

namespace Identity.Oli.Data;

public interface IGoalsRepository
{
    Task<List<GoalModel>> GetAllAsync(); // Retrieve all goals
    Task<GoalModel> GetByIdAsync(Guid id); // Retrieve a goal by its ID
    Task CreateAsync(GoalModel goal); // Create a new goal
    Task UpdateAsync(Guid id, GoalModel updatedGoal); // Update an existing goal
    Task DeleteAsync(Guid id); // Delete a goal by its ID
}
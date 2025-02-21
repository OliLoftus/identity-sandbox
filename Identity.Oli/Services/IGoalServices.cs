using Identity.Oli.Models;

namespace Identity.Oli.Services;

public interface IGoalService
{
    Task<List<GoalModel>> GetGoalsAsync();
    Task<GoalModel?> GetGoalByIdAsync(Guid id);
    Task AddGoalAsync(string title, string description, string category, DateTime dueDate, bool isCompleted);
    Task<GoalModel> UpdateGoalAsync(Guid id, string title, string description, string category, DateTime dueDate, bool isCompleted);
    Task DeleteGoalAsync(Guid id);
    Task AddProgressAsync(Guid goalId, ProgressUpdate progressUpdate);
}
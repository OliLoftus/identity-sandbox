using Identity.Oli.Models;
using Identity.Oli.Models.Requests;

namespace Identity.Oli.Services;

public interface IGoalService
{
    Task<List<GoalModel>> GetGoalsAsync();
    Task<List<GoalModel>> GetGoalsAsync(GoalQuery query);
    Task<(List<GoalModel> Items, long TotalCount)> GetGoalsWithCountAsync(GoalQuery query);
    Task<GoalModel?> GetGoalByIdAsync(Guid id);
    Task<GoalModel> AddGoalAsync(string title, string description, string category, DateTime dueDate, bool isCompleted);
    Task<GoalModel> UpdateGoalAsync(Guid id, string title, string description, string category, DateTime dueDate, bool isCompleted);
    Task DeleteGoalAsync(Guid id);
    Task AddProgressAsync(Guid goalId, ProgressUpdate progressUpdate);
}

using Identity.Oli.Data;
using Identity.Oli.Models;

namespace Identity.Oli.Services;

public class GoalService : IGoalService
{
    private readonly IGoalsRepository _goalsRepository; // Repository for data access stored in private field.
    // Constructor: Injects the IGoalRepository dependency.
    public GoalService(IGoalsRepository goalsRepository)
    {
        _goalsRepository = goalsRepository ?? throw new ArgumentNullException(nameof(goalsRepository));
    }
    // Retrieves all goals from the db.
    public async Task<List<GoalModel>> GetGoalsAsync()
    {
        var goals = await _goalsRepository.GetAllAsync();

        return goals;
    }
    
    // Get goal by Id.
    public async Task<GoalModel?> GetGoalByIdAsync(Guid id)
    {
        return await _goalsRepository.GetByIdAsync(id);
    }
    
    // Add a new goal to the db.
    public async Task AddGoalAsync(string title, string description, string category, DateTime dueDate, bool isCompleted)
    {
        var newGoal = GoalModel.Create(title, description, category, dueDate, isCompleted);
        await _goalsRepository.CreateAsync(newGoal);
    }
    
    // Update existing goal.
    public async Task<GoalModel> UpdateGoalAsync(Guid id, string title, string description, string category, DateTime dueDate, bool isCompleted)
    {
        var existingGoal = await _goalsRepository.GetByIdAsync(id); // Fetch the goal by Id.
        if (existingGoal == null)
        {
            throw new KeyNotFoundException($"Goal with id: {id} not found");
        }

        existingGoal.Update(title, description, category, dueDate, isCompleted);

        await _goalsRepository.UpdateAsync(id, existingGoal);

        return existingGoal;
    }

    // Delete a goal by its Id.
    public async Task DeleteGoalAsync(Guid id)
    {
        var existingGoal = await _goalsRepository.GetByIdAsync(id); // Fetch by Id.
        if (existingGoal == null)
        {
            throw new KeyNotFoundException($"Goal with id: {id} not found");
        }
        await _goalsRepository.DeleteAsync(id);
    }
    
    // Add progress to a goal
    public async Task AddProgressAsync(Guid goalId, ProgressUpdate progress)
    {
        var goal = await _goalsRepository.GetByIdAsync(goalId);
        if (goal == null)
        {
            throw new KeyNotFoundException($"Goal with id {goalId} not found.");
        }

        progress.Id = Guid.NewGuid();
        progress.Date = DateTime.UtcNow; // Automatically set the date
        goal.Progress.Add(progress);

        await _goalsRepository.UpdateAsync(goalId, goal);
    }
    
    
}
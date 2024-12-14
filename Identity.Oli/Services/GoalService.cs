using Identity.Oli.Data;
using Identity.Oli.Models;

namespace Identity.Oli.Services;

public class GoalService
{
    private readonly IGoalsRepository _goalsRepository; // Repository for data access stored in private field.
    // Constructor: Injects the IGoalRepository dependency.
    public GoalService(IGoalsRepository goalsRepository)
    {
        _goalsRepository = goalsRepository;
    }
    // Retrieves all goals from the db.
    public async Task<List<GoalModel>> GetAllGoalsAsync()
    {
        var goals = await _goalsRepository.GetAllAsync();
        Console.WriteLine($"Fetched {goals.Count} goals from repository.");
        return goals;
    }
    
    // Get goal by Id.
    public async Task<GoalModel?> GetGoalByIdAsync(Guid id)
    {
        return await _goalsRepository.GetByIdAsync(id);
    }
    
    // Add a new goal to the db.
    public async Task AddGoalAsync(GoalModel goal)
    {
        goal.Id = Guid.NewGuid(); // Assign unique Id
        await _goalsRepository.CreateAsync(goal);
    }
    
    // Update existing goal.
    public async Task UpdateGoalAsync(Guid id, GoalModel updatedGoal)
    {
        var existingGoal = await _goalsRepository.GetByIdAsync(id); // Fetch the goal by Id.
        if (existingGoal == null)
        {
            throw new KeyNotFoundException($"Goal with id: {id} not found");
        }
        updatedGoal.Id = existingGoal.Id; // Explicitly set new Id.
        await _goalsRepository.UpdateAsync(id, updatedGoal);
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
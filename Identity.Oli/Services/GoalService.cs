using Identity.Oli.Data;
using Identity.Oli.Models;
using MongoDB.Driver;

namespace Identity.Oli.Services;

public class GoalService(IGoalsRepository goalsRepository)
{
    private readonly IGoalsRepository _goalsRepository = goalsRepository;
    private GoalModel _existingGoal;

    public async Task<List<GoalModel>> GetAllGoalsAsync()
    {
        return await _goalsRepository.GetAllAsync();
    }

    public async Task<GoalModel?> GetGoalByIdAsync(Guid id)
    {
        return await _goalsRepository.GetByIdAsync(id);
    }

    public async Task AddGoalAsync(GoalModel goal)
    {
        goal.Id = Guid.NewGuid(); // Assign unique Id
        await _goalsRepository.CreateAsync(goal);
    }

    public async Task UpdateGoalAsync(Guid id, GoalModel updatedGoal)
    {
        _existingGoal = await _goalsRepository.GetByIdAsync(id);
        if (_existingGoal == null)
        {
            throw new KeyNotFoundException($"Goal with id: {id} not found");
        }
        await _goalsRepository.UpdateAsync(id, updatedGoal);
    }

    public async Task DeleteGoalAsync(Guid id)
    {
        _existingGoal = await _goalsRepository.GetByIdAsync(id);
        if (_existingGoal == null)
        {
            throw new KeyNotFoundException($"Goal with id: {id} not found");
        }
        await _goalsRepository.DeleteAsync(id);
    }
    
    
}
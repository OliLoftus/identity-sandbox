using Identity.Oli.Models;
using Identity.Oli.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Identity.Oli.Models.Responses;
using Identity.Oli.Services;

namespace Identity.Oli.Controllers;

// Marks this class as a Web API controller, enabling ASP.NET Core features like model validation and routing.
[ApiController]
// Sets the base route for all endpoints in this controller to /goals.
[Route("goals")]
public class GoalController : ControllerBase
{
    private readonly IGoalService _goalService; // Handles business logic for goals.
    private readonly ILogger<GoalController> _logger;

    // Constructor: Injects GoalService using dependency injection.
    public GoalController(IGoalService goalService, ILogger<GoalController> logger)
    {
        ArgumentNullException.ThrowIfNull(_goalService = goalService, (nameof(goalService)));
        ArgumentNullException.ThrowIfNull(_logger = logger, (nameof(logger)));
    }
    // GET: /goals
    // This endpoint gets all goals.
    [HttpGet]
    public async Task <IActionResult> GetAllGoals()
    {
        // Calls the service layer to fetch all goals from the database.
        var goals = await _goalService.GetGoalsAsync();

        _logger.LogInformation($"Goal count: {goals.Count}");
        // If found returns 200 OK response with customer's data
        return Ok(new ApiResponse<List<GoalModel>>("Success", "Goals retrieved successfully.", goals));
    }
    
    // GET: /goals/{id}
    // This endpoint gets a goal by id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGoal(Guid id)
    {
        // Calls the service layer to fetch a goal by its ID.
        var goal = await _goalService.GetGoalByIdAsync(id);
        // If the goal is not found, returns a 404 Not Found response.
        if (goal == null)
        {
            _logger.LogError($"Goal with id: {id} not found");

            return NotFound(new ApiResponse<GoalModel>("Error", "Goal not found.", null));
        }
        _logger.LogInformation($"Goal with id: {id} was found.");

        return Ok(new ApiResponse<GoalModel>("Success", "Goal retrieved successfully.", goal));
    }
    
    // POST: /goal
    // This endpoint is used to create a new goal.
    [HttpPost]
    public async Task<IActionResult> CreateGoal([FromBody] GoalRequest request)
    {
        var goal = GoalModel.Create(request.Title, request.Description, request.Category, request.DueDate, request.IsCompleted);

        await _goalService.AddGoalAsync(request.Title, request.Description, request.Category, request.DueDate, request.IsCompleted);

        _logger.LogInformation($"Goal created: {goal.Title} - {goal.Description}");
        // Returns a 201 Created response with a Location header pointing to the newly created goal.
        return CreatedAtAction(nameof(GetGoal), new { id = goal.Id },
            new ApiResponse<GoalModel>("Success", "Goal created successfully.", goal));
    }
    
    // PUT: /goals/{id}
    // Updates a goal.
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGoal(Guid id, [FromBody] GoalRequest request)
    {
            try
            {
                var result = await _goalService.UpdateGoalAsync(id, request.Title,
                    request.Description,
                    request.Category,
                    request.DueDate,
                    request.IsCompleted);

                _logger.LogInformation($"Goal updated: {request.Title} - {request.Description}");

                return Ok(new ApiResponse<GoalModel>("Success", "Goal updated successfully.", result));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Goal with id: {id} not found");
                // If the goal is not found, returns a 404 Not Found response with an error message.
                return NotFound(new ApiResponse<GoalModel>("Error", ex.Message, null));
            }
    }
    // DELETE: /goals/{id}
    // Deletes a goal by  id
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGoal(Guid id)
    {
        try
        {
            await _goalService.DeleteGoalAsync(id);

            _logger.LogInformation($"Goal deleted: {id}");

            return Ok(new ApiResponse<GoalModel>("Success", "Goal deleted successfully.", null));
        }
        catch (KeyNotFoundException exception)
        {
            _logger.LogWarning($"Goal not found: {exception.Message}");

            return NotFound(new ApiResponse<GoalModel>("Error", "Goal not found.", null));
        }
    }
    
    // POST: /goals/{id}/progress
    // Adds a progress update
    [HttpPost("{goalId}/progress")]
    public async Task<IActionResult> AddProgress(Guid goalId, [FromBody] ProgressUpdate progress)
    {
        try
        {
            // Calls the AddProgress method in the GoalService
            await _goalService.AddProgressAsync(goalId, progress);
            _logger.LogInformation($"Progress added to: {goalId}");
            return Ok(new ApiResponse<ProgressUpdate>("Success", "Progress added successfully.", progress)); // Returns the added progress
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning($"Goal not found: {ex.Message}");

            return NotFound(new ApiResponse<object>("Error", "Goal not found.", null));
        }
    }
    
    // GET: /goals/{id}/progress
    // Gets a goals progress updates
    [HttpGet("{id}/progress")]
    public async Task<IActionResult> GetProgressUpdates(Guid id)
    {
            var goal = await _goalService.GetGoalByIdAsync(id);
            if (goal == null)
            {
                _logger.LogInformation($"Goal with id: {id} was found.");

                return NotFound(new ApiResponse<List<ProgressUpdate>>("Error", "Goal was not found.", null));
            }

            return Ok(new ApiResponse<List<ProgressUpdate>>("Success", "Progress retrieved successfully", goal.Progress)); // Return all progress updates for the specified goal
    }
}

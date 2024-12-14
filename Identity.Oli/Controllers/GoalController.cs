using Identity.Oli.Data;
using Identity.Oli.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Identity.Oli.Services;

namespace Identity.Oli.Controllers;

// Marks this class as a Web API controller, enabling ASP.NET Core features like model validation and routing.
[ApiController]
// Sets the base route for all endpoints in this controller to /goals.
[Route("goals")]
public class GoalController : ControllerBase
{
    private readonly GoalService _goalService; // Handles business logic for goals.
    
    // Constructor: Injects GoalService using dependency injection.
    public GoalController(GoalService goalService)
    {
        _goalService = goalService;
    }
    // GET: /goals
    // This endpoint gets all goals.
    [HttpGet]
    public async Task <IActionResult> GetAllGoals()
    {
        // Calls the service layer to fetch all goals from the database.
        var goals = await _goalService.GetAllGoalsAsync();
        // If found returns 200 OK response with customer's data
        return Ok(goals);
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
            return NotFound();
        }
        return Ok(goal);
    }
    
    // POST: /goal/{id}
    // This endpoint is used to create a new goal.
    [HttpPost]
    public async Task<IActionResult> CreateGoal([FromBody] GoalModel newGoal)
    {
        await _goalService.AddGoalAsync(newGoal);
        // Returns a 201 Created response with a Location header pointing to the newly created goal.
        return CreatedAtAction(nameof(GetGoal), new { id = newGoal.Id }, newGoal);
    }
    
    // PUT: /goals/{id}
    // Updates a goal.
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGoal(Guid id, [FromBody] GoalModel updatedGoal)
    {
        try
        {
            await _goalService.UpdateGoalAsync(id, updatedGoal);
            // Returns a 204 No Content response to indicate success.
            return NoContent();
        }
        catch (KeyNotFoundException exception)
        {
            // If the goal is not found, returns a 404 Not Found response with an error message.
            return NotFound(new { message = exception.Message });
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
            return NoContent();
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
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
            return Ok(progress); // Returns the added progress
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
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
            return NotFound(new { message = "Goal not found" });
        }

        return Ok(goal.Progress); // Return all progress updates for the specified goal
    }
}
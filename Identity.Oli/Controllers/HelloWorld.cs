using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Oli.Controllers;

[ApiController]
[Route("hello")]
public class HelloWorld: ControllerBase
{
    [Authorize(Policy = "ReadPolicy")]
    [HttpGet]
    public IActionResult Index()
    {
        return Ok(new 
        {
            Message = "HelloWorld"
        });
    }
    
    [Authorize(Policy = "WritePolicy")]
    [HttpPost("write")]
    public IActionResult Write()
    {
        return Ok(new 
        {
            Message = "Write access granted!"
        });
    }
    
    [Authorize(Policy = "DeletePolicy")]
    [HttpDelete("delete")]
    public IActionResult Delete()
    {
        return Ok(new 
        {
            Message = "Delete access granted!"
        });
    }
}

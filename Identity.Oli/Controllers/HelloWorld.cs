using Microsoft.AspNetCore.Mvc;

namespace Identity.Oli.Controllers;

public class HelloWorld: Controller
{
    [HttpGet("hello")]
    public IActionResult Index()
    {
        return Ok( new 
        {
            Ping = "HelloWorld"
        });
    }
}
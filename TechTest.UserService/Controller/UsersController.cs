using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    // GET: api/users
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        return Ok("This is a static string response for all users.");
    }

    //[HttpPost]
    //public IActionResult Post([FromBody] object data)
    //{
    //    return Ok($"Received data");
    //}
}

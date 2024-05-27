using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    // GET: api/settings
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        return Ok("This is a static string response for all settings.");
    }
}

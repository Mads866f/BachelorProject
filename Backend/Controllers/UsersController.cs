using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{

    [HttpGet()]
    public IActionResult getUsers()
    {
        return Ok(new { message = "List Of Users" });
    }
}
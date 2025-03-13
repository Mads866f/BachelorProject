using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersControllers : ControllerBase
{

    [HttpGet()]
    public IActionResult getUsers()
    {
        return Ok(new { message = "List Of Users" });
    }
}
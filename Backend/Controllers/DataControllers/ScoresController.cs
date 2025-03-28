using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.DataControllers;

[ApiController]
[Route("api/[controller]")]
public class ScoresController(IScoresService service) : ControllerBase
{
    
}
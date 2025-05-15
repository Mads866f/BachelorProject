using Microsoft.AspNetCore.Mvc;

namespace Front.Controllers;

[Route("download")]
public class DownloadController : Controller
{
    [HttpGet("{*filePath}")]
    public IActionResult Download(string filePath)
    {
        var fileStream = new FileStream(filePath, FileMode.Open);
        var fileName = Path.GetFileName(filePath);
        return File(fileStream, "application/octet-stream",fileName);
    }
}
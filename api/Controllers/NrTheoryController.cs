using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NrTheoryController : ControllerBase
{
    private readonly NrTheoryService _srv;

    public NrTheoryController(NrTheoryService srv)
    {
        _srv = srv;
    }

    [HttpGet("bit-manipulation/{n:int}")]
    public IActionResult BitManipulation(int n) => Ok(_srv.BitManipulation(n));
}

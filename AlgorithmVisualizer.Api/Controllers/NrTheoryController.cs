using AlgorithmVisualizer.Api.Models;
using AlgorithmVisualizer.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlgorithmVisualizer.Api.Controllers;

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

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

    [HttpGet("sieve/{n:int}")]
    public IActionResult Sieve(int n) => Ok(_srv.Sieve(n));

    [HttpGet("gcd/{a:int}/{b:int}")]
    public IActionResult Gcd(int a, int b) => Ok(_srv.Gcd(a, b));




    [HttpGet("prime-factorization/{n:int}")]
    public IActionResult PrimeFactorization(int n) => Ok(_srv.PrimeFactorization(n));



    [HttpGet("bit-manipulation/{n:int}")]
    public IActionResult BitManipulation(int n) => Ok(_srv.BitManipulation(n));

}

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

    [HttpGet("extended-gcd/{a:int}/{b:int}")]
    public IActionResult ExtendedGcd(int a, int b) => Ok(_srv.ExtendedGcd(a, b));

    [HttpGet("fast-exp/{baseVal:int}/{exp:int}/{mod:int}")]
    public IActionResult FastExponentiation(int baseVal, int exp, int mod) =>
        Ok(_srv.FastExponentiation(baseVal, exp, mod));

    [HttpGet("modular/{a:int}/{b:int}/{mod:int}")]
    public IActionResult ModularArithmetic(int a, int b, int mod) =>
        Ok(_srv.ModularArithmetic(a, b, mod));

    [HttpGet("prime-factorization/{n:int}")]
    public IActionResult PrimeFactorization(int n) => Ok(_srv.PrimeFactorization(n));

    [HttpGet("fibonacci-matrix/{n:int}")]
    public IActionResult FibonacciMatrix(int n) => Ok(_srv.FibonacciMatrix(n));

    [HttpPost("chinese-remainder")]
    public IActionResult ChineseRemainder([FromBody] int[][] input) =>
        Ok(_srv.ChineseRemainder(input[0], input[1]));

    [HttpGet("bit-manipulation/{n:int}")]
    public IActionResult BitManipulation(int n) => Ok(_srv.BitManipulation(n));

    [HttpGet("catalan/{n:int}")]
    public IActionResult CatalanNumbers(int n) => Ok(_srv.CatalanNumbers(n));
}

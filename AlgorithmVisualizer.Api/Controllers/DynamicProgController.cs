using AlgorithmVisualizer.Api.Models;
using AlgorithmVisualizer.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlgorithmVisualizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DynamicProgController : ControllerBase
{
    private readonly DynamicProgService _srv;

    public DynamicProgController(DynamicProgService srv)
    {
        _srv = srv;
    }

    [HttpGet("fibonacci/{n:int}")]
    public IActionResult Fibonacci(int n) => Ok(_srv.Fibonacci(n));

    [HttpPost("knapsack")]
    public IActionResult Knapsack([FromBody] KnapsackRequest req) =>
        Ok(_srv.Knapsack(req.Weights, req.Values, req.Capacity));

    [HttpPost("lcs")]
    public IActionResult Lcs([FromBody] StringRequest req) => Ok(_srv.Lcs(req.Text, req.Pattern));

    [HttpPost("lis")]
    public IActionResult Lis([FromBody] int[] input) => Ok(_srv.Lis(input));

    [HttpPost("coin-change/{amount:int}")]
    public IActionResult CoinChange([FromBody] int[] coins, int amount) =>
        Ok(_srv.CoinChange(coins, amount));

    [HttpPost("levenshtein")]
    public IActionResult Levenshtein([FromBody] StringRequest req) =>
        Ok(_srv.Levenshtein(req.Text, req.Pattern));

    [HttpPost("subset-sum/{target:int}")]
    public IActionResult SubsetSum([FromBody] int[] input, int target) =>
        Ok(_srv.SubsetSum(input, target));

    [HttpGet("climbing-stairs/{n:int}")]
    public IActionResult ClimbingStairs(int n) => Ok(_srv.ClimbingStairs(n));
}

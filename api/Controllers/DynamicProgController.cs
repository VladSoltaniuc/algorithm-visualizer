using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

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

    [HttpGet("climbing-stairs/{n:int}")]
    public IActionResult ClimbingStairs(int n) => Ok(_srv.ClimbingStairs(n));
}

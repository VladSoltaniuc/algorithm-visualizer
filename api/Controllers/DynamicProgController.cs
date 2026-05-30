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
    public IActionResult Fibonacci(int n)
    {
        try { return Ok(_srv.Fibonacci(n)); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
    }

    [HttpPost("knapsack")]
    public IActionResult Knapsack([FromBody] KnapsackRequest req)
    {
        try { return Ok(_srv.Knapsack(req.Weights, req.Values, req.Capacity)); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
    }

    [HttpPost("lcs")]
    public IActionResult Lcs([FromBody] StringRequest req)
    {
        try { return Ok(_srv.Lcs(req.Text, req.Pattern)); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
    }

    [HttpPost("lis")]
    public IActionResult Lis([FromBody] int[] input)
    {
        try { return Ok(_srv.Lis(input)); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
    }

    [HttpPost("coin-change/{amount:int}")]
    public IActionResult CoinChange([FromBody] int[] coins, int amount)
    {
        try { return Ok(_srv.CoinChange(coins, amount)); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
    }

    [HttpGet("climbing-stairs/{n:int}")]
    public IActionResult ClimbingStairs(int n)
    {
        try { return Ok(_srv.ClimbingStairs(n)); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
    }
}

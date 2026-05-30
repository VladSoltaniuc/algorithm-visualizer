using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BacktrackingController : ControllerBase
{
    private readonly BacktrackingService _srv;

    public BacktrackingController(BacktrackingService srv)
    {
        _srv = srv;
    }

    [HttpPost("permutations")]
    public IActionResult Permutations([FromBody] int[] input)
    {
        try { return Ok(_srv.Permutations(input)); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
    }

    [HttpPost("subsets")]
    public IActionResult Subsets([FromBody] int[] input)
    {
        try { return Ok(_srv.Subsets(input)); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
    }

    [HttpPost("combination-sum/{target:int}")]
    public IActionResult CombinationSum([FromBody] int[] input, int target)
    {
        try
        {
            return Ok(_srv.CombinationSum(input, target));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("palindrome-partitioning")]
    public IActionResult PalindromePartitioning([FromBody] StringRequest req)
    {
        try { return Ok(_srv.PalindromePartitioning(req.Text)); }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
    }
}

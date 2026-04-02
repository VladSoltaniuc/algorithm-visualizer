using AlgorithmVisualizer.Api.Models;
using AlgorithmVisualizer.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlgorithmVisualizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BacktrackingController : ControllerBase
{
    private readonly BacktrackingService _srv;

    public BacktrackingController(BacktrackingService srv)
    {
        _srv = srv;
    }

    [HttpGet("n-queens/{n:int}")]
    public IActionResult NQueens(int n) => Ok(_srv.NQueens(n));

    [HttpPost("sudoku")]
    public IActionResult Sudoku([FromBody] int[] grid) => Ok(_srv.Sudoku(grid));

    [HttpPost("permutations")]
    public IActionResult Permutations([FromBody] int[] input) => Ok(_srv.Permutations(input));

    [HttpPost("subsets")]
    public IActionResult Subsets([FromBody] int[] input) => Ok(_srv.Subsets(input));

    [HttpPost("rat-in-maze/{n:int}")]
    public IActionResult RatInMaze([FromBody] int[] grid, int n) => Ok(_srv.RatInMaze(grid, n));


    [HttpPost("word-search")]
    public IActionResult WordSearch([FromBody] StringRequest req) =>
        Ok(_srv.WordSearch(req.Text, req.Pattern));

    [HttpPost("combination-sum/{target:int}")]
    public IActionResult CombinationSum([FromBody] int[] input, int target) =>
        Ok(_srv.CombinationSum(input, target));

    [HttpPost("palindrome-partitioning")]
    public IActionResult PalindromePartitioning([FromBody] StringRequest req) =>
        Ok(_srv.PalindromePartitioning(req.Text));

}

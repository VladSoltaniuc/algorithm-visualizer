using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FindController : ControllerBase
{
    private readonly FindService _srv;
    public FindController(FindService srv) { _srv = srv; }

    [HttpPost("binary-search/{target:int}")]
    public IActionResult BinarySearch([FromBody] int[] input, int target) => Ok(_srv.BinarySearch(input, target));

    [HttpPost("linear-search/{target:int}")]
    public IActionResult LinearSearch([FromBody] int[] input, int target) => Ok(_srv.LinearSearch(input, target));

    [HttpPost("two-pointers/{target:int}")]
    public IActionResult TwoPointers([FromBody] int[] input, int target) => Ok(_srv.TwoPointers(input, target));

    [HttpPost("sliding-window/{windowSize:int}")]
    public IActionResult SlidingWindow([FromBody] int[] input, int windowSize) => Ok(_srv.SlidingWindow(input, windowSize));

    [HttpPost("kadane")]
    public IActionResult Kadane([FromBody] int[] input) => Ok(_srv.Kadane(input));
}

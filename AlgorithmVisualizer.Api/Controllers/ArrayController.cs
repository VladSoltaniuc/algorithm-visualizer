using AlgorithmVisualizer.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlgorithmVisualizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArrayController : ControllerBase
{
    private readonly ArrayService _srv;

    public ArrayController(ArrayService srv)
    {
        _srv = srv;
    }

    [HttpPost("bubble-sort")]
    public IActionResult BubbleSort([FromBody] int[] input) => Ok(_srv.BubbleSort(input));

    [HttpPost("quick-sort")]
    public IActionResult QuickSort([FromBody] int[] input) => Ok(_srv.QuickSort(input));

    [HttpPost("merge-sort")]
    public IActionResult MergeSort([FromBody] int[] input) => Ok(_srv.MergeSort(input));

    [HttpPost("binary-search/{target:int}")]
    public IActionResult BinarySearch([FromBody] int[] input, int target) =>
        Ok(_srv.BinarySearch(input, target));

    [HttpPost("linear-search/{target:int}")]
    public IActionResult LinearSearch([FromBody] int[] input, int target) =>
        Ok(_srv.LinearSearch(input, target));

    [HttpPost("insertion-sort")]
    public IActionResult InsertionSort([FromBody] int[] input) => Ok(_srv.InsertionSort(input));

    [HttpPost("selection-sort")]
    public IActionResult SelectionSort([FromBody] int[] input) => Ok(_srv.SelectionSort(input));

    [HttpPost("two-pointers/{target:int}")]
    public IActionResult TwoPointers([FromBody] int[] input, int target) =>
        Ok(_srv.TwoPointers(input, target));

    [HttpPost("sliding-window/{windowSize:int}")]
    public IActionResult SlidingWindow([FromBody] int[] input, int windowSize) =>
        Ok(_srv.SlidingWindow(input, windowSize));

    [HttpPost("kadane")]
    public IActionResult Kadane([FromBody] int[] input) => Ok(_srv.Kadane(input));
}

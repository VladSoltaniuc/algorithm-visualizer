using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SortController : ControllerBase
{
    private readonly SortService _srv;
    public SortController(SortService srv) { _srv = srv; }

    [HttpPost("bubble-sort")]
    public IActionResult BubbleSort([FromBody] int[] input) => Ok(_srv.BubbleSort(input));

    [HttpPost("quick-sort")]
    public IActionResult QuickSort([FromBody] int[] input) => Ok(_srv.QuickSort(input));

    [HttpPost("merge-sort")]
    public IActionResult MergeSort([FromBody] int[] input) => Ok(_srv.MergeSort(input));

    [HttpPost("insertion-sort")]
    public IActionResult InsertionSort([FromBody] int[] input) => Ok(_srv.InsertionSort(input));

    [HttpPost("selection-sort")]
    public IActionResult SelectionSort([FromBody] int[] input) => Ok(_srv.SelectionSort(input));
}

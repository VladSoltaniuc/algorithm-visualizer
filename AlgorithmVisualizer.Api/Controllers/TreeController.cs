using AlgorithmVisualizer.Api.Models;
using AlgorithmVisualizer.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlgorithmVisualizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TreeController : ControllerBase
{
    private readonly TreeService _srv;

    public TreeController(TreeService srv)
    {
        _srv = srv;
    }

    [HttpPost("inorder")]
    public IActionResult Inorder([FromBody] int[] input) => Ok(_srv.Inorder(input));

    [HttpPost("preorder")]
    public IActionResult Preorder([FromBody] int[] input) => Ok(_srv.Preorder(input));

    [HttpPost("postorder")]
    public IActionResult Postorder([FromBody] int[] input) => Ok(_srv.Postorder(input));

    [HttpPost("level-order")]
    public IActionResult LevelOrder([FromBody] int[] input) => Ok(_srv.LevelOrder(input));

    [HttpPost("bst-insert-search/{target:int}")]
    public IActionResult BstInsertSearch([FromBody] int[] input, int target) =>
        Ok(_srv.BstInsertSearch(input, target));

    [HttpPost("height")]
    public IActionResult Height([FromBody] int[] input) => Ok(_srv.Height(input));

    [HttpPost("lca/{a:int}/{b:int}")]
    public IActionResult Lca([FromBody] int[] input, int a, int b) => Ok(_srv.Lca(input, a, b));

    [HttpPost("invert")]
    public IActionResult Invert([FromBody] int[] input) => Ok(_srv.Invert(input));

    [HttpPost("validate-bst")]
    public IActionResult ValidateBst([FromBody] int[] input) => Ok(_srv.ValidateBst(input));

    [HttpPost("diameter")]
    public IActionResult Diameter([FromBody] int[] input) => Ok(_srv.Diameter(input));

    [HttpPost("huffman")]
    public IActionResult Huffman([FromBody] StringRequest req) =>
        Ok(_srv.HuffmanEncoding(req.Text));
}

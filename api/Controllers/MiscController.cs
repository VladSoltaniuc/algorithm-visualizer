using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MiscController : ControllerBase
{
    private readonly MiscService _srv;
    public MiscController(MiscService srv) { _srv = srv; }

    [HttpPost("reversal")]
    public IActionResult Reversal([FromBody] StringRequest req) => Ok(_srv.Reversal(req.Text));

    [HttpPost("huffman")]
    public IActionResult Huffman([FromBody] StringRequest req) => Ok(_srv.HuffmanCoding(req.Text));

    [HttpGet("bit-manipulation/{n:int}")]
    public IActionResult BitManipulation(int n) => Ok(_srv.BitManipulation(n));
}

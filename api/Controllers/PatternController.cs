using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatternController : ControllerBase
{
    private readonly PatternService _srv;
    public PatternController(PatternService srv) { _srv = srv; }

    [HttpPost("linear-search")]
    public IActionResult LinearSearch([FromBody] StringRequest req) => Ok(_srv.LinearSearch(req.Text, req.Pattern));

    [HttpPost("kmp")]
    public IActionResult KMP([FromBody] StringRequest req) => Ok(_srv.KMP(req.Text, req.Pattern));

    [HttpPost("boyer-moore")]
    public IActionResult BoyerMoore([FromBody] StringRequest req) => Ok(_srv.BoyerMoore(req.Text, req.Pattern));

    [HttpPost("rabin-karp")]
    public IActionResult RabinKarp([FromBody] StringRequest req) => Ok(_srv.RabinKarp(req.Text, req.Pattern));

    [HttpPost("longest-palindrome")]
    public IActionResult LongestPalindrome([FromBody] StringRequest req) => Ok(_srv.LongestPalindrome(req.Text));

    [HttpPost("anagram-detection")]
    public IActionResult AnagramDetection([FromBody] StringRequest req) => Ok(_srv.AnagramDetection(req.Text, req.Pattern));
}

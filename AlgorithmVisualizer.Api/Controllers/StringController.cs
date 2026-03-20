using AlgorithmVisualizer.Api.Models;
using AlgorithmVisualizer.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlgorithmVisualizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StringController : ControllerBase
{
    private readonly StringService _srv;

    public StringController(StringService srv)
    {
        _srv = srv;
    }

    [HttpPost("linear-search")]
    public IActionResult LinearSearch([FromBody] StringRequest req) =>
        Ok(_srv.LinearSearch(req.Text, req.Pattern));

    [HttpPost("kmp")]
    public IActionResult KMP([FromBody] StringRequest req) => Ok(_srv.KMP(req.Text, req.Pattern));

    [HttpPost("boyer-moore")]
    public IActionResult BoyerMoore([FromBody] StringRequest req) =>
        Ok(_srv.BoyerMoore(req.Text, req.Pattern));

    [HttpPost("rabin-karp")]
    public IActionResult RabinKarp([FromBody] StringRequest req) =>
        Ok(_srv.RabinKarp(req.Text, req.Pattern));

    [HttpPost("lcs")]
    public IActionResult Lcs([FromBody] StringRequest req) =>
        Ok(_srv.LongestCommonSubsequence(req.Text, req.Pattern));

    [HttpPost("longest-palindrome")]
    public IActionResult LongestPalindrome([FromBody] StringRequest req) =>
        Ok(_srv.LongestPalindrome(req.Text));

    [HttpPost("anagram-detection")]
    public IActionResult AnagramDetection([FromBody] StringRequest req) =>
        Ok(_srv.AnagramDetection(req.Text, req.Pattern));

    [HttpPost("reversal")]
    public IActionResult Reversal([FromBody] StringRequest req) => Ok(_srv.Reversal(req.Text));

    [HttpPost("run-length-encoding")]
    public IActionResult RunLengthEncoding([FromBody] StringRequest req) =>
        Ok(_srv.RunLengthEncoding(req.Text));

    [HttpPost("levenshtein")]
    public IActionResult Levenshtein([FromBody] StringRequest req) =>
        Ok(_srv.Levenshtein(req.Text, req.Pattern));
}

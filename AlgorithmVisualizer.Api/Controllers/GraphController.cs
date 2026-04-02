using AlgorithmVisualizer.Api.Models;
using AlgorithmVisualizer.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlgorithmVisualizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GraphController : ControllerBase
{
    private readonly GraphService _srv;

    public GraphController(GraphService srv)
    {
        _srv = srv;
    }

    [HttpPost("bfs")]
    public IActionResult Bfs([FromBody] GraphRequest req) => Ok(_srv.Bfs(req));

    [HttpPost("dfs")]
    public IActionResult Dfs([FromBody] GraphRequest req) => Ok(_srv.Dfs(req));

    [HttpPost("dijkstra")]
    public IActionResult Dijkstra([FromBody] GraphRequest req) => Ok(_srv.Dijkstra(req));

    [HttpPost("bellman-ford")]
    public IActionResult BellmanFord([FromBody] GraphRequest req) => Ok(_srv.BellmanFord(req));


    [HttpPost("topological-sort")]
    public IActionResult TopologicalSort([FromBody] GraphRequest req) =>
        Ok(_srv.TopologicalSort(req));

    [HttpPost("cycle-detection")]
    public IActionResult CycleDetection([FromBody] GraphRequest req) =>
        Ok(_srv.CycleDetection(req));

    [HttpPost("union-find")]
    public IActionResult UnionFind([FromBody] GraphRequest req) => Ok(_srv.UnionFind(req));

    [HttpPost("kruskal")]
    public IActionResult Kruskal([FromBody] GraphRequest req) => Ok(_srv.Kruskal(req));

    [HttpPost("prim")]
    public IActionResult Prim([FromBody] GraphRequest req) => Ok(_srv.Prim(req));
}

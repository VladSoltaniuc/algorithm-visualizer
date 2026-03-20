namespace AlgorithmVisualizer.Api.Models;

public class GraphRequest
{
    public int NodeCount { get; set; }
    public int[][] Edges { get; set; } = [];
    public int StartNode { get; set; }
}

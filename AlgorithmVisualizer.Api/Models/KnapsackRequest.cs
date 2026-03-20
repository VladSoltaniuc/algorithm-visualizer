namespace AlgorithmVisualizer.Api.Models;

public class KnapsackRequest
{
    public int[] Weights { get; set; } = [];
    public int[] Values { get; set; } = [];
    public int Capacity { get; set; }
}

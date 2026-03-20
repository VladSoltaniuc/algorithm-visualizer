namespace AlgorithmVisualizer.Api.Models;

public class AlgorithmStep
{
    public int StepNumber { get; set; }
    public int[] Array { get; set; } = [];
    public string Description { get; set; } = string.Empty;
    public int[] HighlightIndices { get; set; } = [];
    public int[] SortedIndices { get; set; } = [];
}

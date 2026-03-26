namespace AlgorithmVisualizer.Api.Models;

public class AlgorithmStep
{
    public int StepNumber { get; set; }
    public int[] Array { get; set; } = [];
    public string Description { get; set; } = string.Empty;
    public int[] HighlightIndices { get; set; } = [];
    public int[] SortedIndices { get; set; } = [];
    public int[] PatternArray { get; set; } = [];
    public int PatternOffset { get; set; } = -1;
    public int PatternHighlightIndex { get; set; } = -1;
    public bool IsNumericArray { get; set; } = false;
}

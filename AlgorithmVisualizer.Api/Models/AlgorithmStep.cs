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
    public long? TextHash { get; set; }
    public long? PatternHash { get; set; }
    public int[] PArray { get; set; } = [];
    public int ManacherCenter { get; set; } = -1;
    public int ManacherRight { get; set; } = -1;

    // Per-cell labels and secondary notes (e.g. Huffman char + binary code)
    public string[] Labels { get; set; } = [];
    public string[] Notes { get; set; } = [];

    // Full level-order with null gaps so the frontend can reconstruct the tree
    // shape dynamically (e.g. Invert Binary Tree). null elements = empty slots.
    public int?[]? TreeLevelOrder { get; set; }

    // DP matrix visualization (e.g. LCS, Edit Distance)
    public int[][]? DpMatrix { get; set; }
    public string? RowHeaders { get; set; }
    public string? ColHeaders { get; set; }

    // Multi-character row/column labels (used by Knapsack, SubsetSum, LIS)
    public string[]? RowLabels { get; set; }
    public string[]? ColLabels { get; set; }
    public int HighlightRow { get; set; } = -1;
    public int HighlightCol { get; set; } = -1;
    public int[]? BacktrackPath { get; set; }
}

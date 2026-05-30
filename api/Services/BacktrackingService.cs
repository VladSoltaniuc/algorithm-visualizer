using api.Helpers;
using api.Models;

namespace api.Services;

public class BacktrackingService
{
    public List<AlgorithmStep> Permutations(int[] arr)
    {
        if (arr.Length == 0)
            throw new ArgumentException("Provide a non-empty array.");
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(new AlgorithmStep
        {
            StepNumber = step++,
            Array = (int[])arr.Clone(),
            Description = $"Generating permutations of [{string.Join(", ", arr)}]",
        });
        BacktrackingHelpers.PermuteHelper(arr, 0, steps, ref step);
        steps.Add(new AlgorithmStep
        {
            StepNumber = step,
            Array = (int[])arr.Clone(),
            Description = "All permutations generated",
        });
        return steps;
    }

    public List<AlgorithmStep> Subsets(int[] arr)
    {
        if (arr.Length == 0)
            throw new ArgumentException("Provide a non-empty array.");
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(new AlgorithmStep
        {
            StepNumber = step++,
            Array = (int[])arr.Clone(),
            Description = $"Generating subsets of [{string.Join(", ", arr)}]",
        });
        BacktrackingHelpers.SubsetHelper(arr, 0, new List<int>(), steps, ref step);
        steps.Add(new AlgorithmStep
        {
            StepNumber = step,
            Array = (int[])arr.Clone(),
            Description = "All subsets generated",
        });
        return steps;
    }

    public List<AlgorithmStep> CombinationSum(int[] candidates, int target)
    {
        if (candidates.Length == 0)
            throw new ArgumentException("Provide non-empty candidates.");
        if (candidates.Any(c => c <= 0))
            throw new ArgumentException("This implementation expects an all-positive array. Please enter only positive integers (no zeros).");
        if (target <= 0)
            throw new ArgumentException("Target must be a positive integer.");
        Array.Sort(candidates);
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(new AlgorithmStep
        {
            StepNumber = step++,
            Array = (int[])candidates.Clone(),
            Description = $"Combination sum: target={target}, candidates=[{string.Join(",", candidates)}]",
        });

        var results = new List<int[]>();
        BacktrackingHelpers.CombSumHelper(candidates, target, 0, new List<int>(), results, steps, ref step);
        steps.Add(new AlgorithmStep
        {
            StepNumber = step,
            Array = (int[])candidates.Clone(),
            Description = $"Found {results.Count} combination(s)",
        });
        return steps;
    }

    public List<AlgorithmStep> PalindromePartitioning(string text)
    {
        if (string.IsNullOrEmpty(text))
            throw new ArgumentException("Provide a non-empty string.");
        var codes = text.Select(c => (int)c).ToArray();
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(new AlgorithmStep
        {
            StepNumber = step++,
            Array = (int[])codes.Clone(),
            Description = $"Palindrome partitioning of \"{text}\"",
        });

        var results = new List<string[]>();
        BacktrackingHelpers.PalPartHelper(text, 0, new List<string>(), results, codes, steps, ref step);
        steps.Add(new AlgorithmStep
        {
            StepNumber = step,
            Array = (int[])codes.Clone(),
            Description = $"Found {results.Count} partition(s)",
        });
        return steps;
    }
}

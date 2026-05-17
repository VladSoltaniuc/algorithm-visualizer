using AlgorithmVisualizer.Api.Models;

namespace AlgorithmVisualizer.Api.Services;

public class BacktrackingService
{
    // 3. Generate All Permutations
    // Time: O(n! Â· n)
    // Space: O(n)
    public List<AlgorithmStep> Permutations(int[] arr)
    {
        if (arr.Length == 0)
            throw new ArgumentException("Provide a non-empty array.");
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])arr.Clone(),
                Description = $"Generating permutations of [{string.Join(", ", arr)}]",
            }
        );
        PermuteHelper(arr, 0, steps, ref step);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])arr.Clone(),
                Description = "All permutations generated",
            }
        );
        return steps;
    }

    private static void PermuteHelper(int[] arr, int start, List<AlgorithmStep> steps, ref int step)
    {
        if (start == arr.Length)
        {
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])arr.Clone(),
                    Description = $"Permutation: [{string.Join(", ", arr)}]",
                    SortedIndices = Enumerable.Range(0, arr.Length).ToArray(),
                }
            );
            return;
        }
        for (int i = start; i < arr.Length; i++)
        {
            (arr[start], arr[i]) = (arr[i], arr[start]);
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])arr.Clone(),
                    Description = $"Swap index {start} and {i}",
                    HighlightIndices = [start, i],
                }
            );
            PermuteHelper(arr, start + 1, steps, ref step);
            (arr[start], arr[i]) = (arr[i], arr[start]);
        }
    }

    // 4. Generate All Subsets
    // Time: O(2^n)
    // Space: O(n)
    public List<AlgorithmStep> Subsets(int[] arr)
    {
        if (arr.Length == 0)
            throw new ArgumentException("Provide a non-empty array.");
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])arr.Clone(),
                Description = $"Generating subsets of [{string.Join(", ", arr)}]",
            }
        );
        SubsetHelper(arr, 0, new List<int>(), steps, ref step);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])arr.Clone(),
                Description = "All subsets generated",
            }
        );
        return steps;
    }

    private static void SubsetHelper(
        int[] arr,
        int idx,
        List<int> current,
        List<AlgorithmStep> steps,
        ref int step
    )
    {
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = current.Count > 0 ? current.ToArray() : [0],
                Description =
                    current.Count > 0 ? $"Subset: {{{string.Join(", ", current)}}}" : "Subset: {}",
                SortedIndices = Enumerable.Range(0, current.Count).ToArray(),
            }
        );
        for (int i = idx; i < arr.Length; i++)
        {
            current.Add(arr[i]);
            SubsetHelper(arr, i + 1, current, steps, ref step);
            current.RemoveAt(current.Count - 1);
        }
    }

    // 8. Combination Sum
    // Time: O(n^(T/M)) where T = target, M = min candidate
    // Space: O(T/M)
    public List<AlgorithmStep> CombinationSum(int[] candidates, int target)
    {
        if (candidates.Length == 0)
            throw new ArgumentException("Provide non-empty candidates.");
        Array.Sort(candidates);
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])candidates.Clone(),
                Description =
                    $"Combination sum: target={target}, candidates=[{string.Join(",", candidates)}]",
            }
        );

        var results = new List<int[]>();
        CombSumHelper(candidates, target, 0, new List<int>(), results, steps, ref step);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])candidates.Clone(),
                Description = $"Found {results.Count} combination(s)",
            }
        );
        return steps;
    }

    private static void CombSumHelper(
        int[] cands,
        int remain,
        int start,
        List<int> current,
        List<int[]> results,
        List<AlgorithmStep> steps,
        ref int step
    )
    {
        if (remain == 0)
        {
            results.Add(current.ToArray());
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = current.ToArray(),
                    Description = $"Found: [{string.Join(", ", current)}]",
                    SortedIndices = Enumerable.Range(0, current.Count).ToArray(),
                }
            );
            return;
        }
        for (int i = start; i < cands.Length && cands[i] <= remain; i++)
        {
            current.Add(cands[i]);
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = current.ToArray(),
                    Description = $"Add {cands[i]}, sum={current.Sum()}, need={remain - cands[i]}",
                    HighlightIndices = [current.Count - 1],
                }
            );
            CombSumHelper(cands, remain - cands[i], i, current, results, steps, ref step);
            current.RemoveAt(current.Count - 1);
        }
    }

    // 9. Palindrome Partitioning
    // Time: O(n Â· 2^n)
    // Space: O(n)
    public List<AlgorithmStep> PalindromePartitioning(string text)
    {
        if (string.IsNullOrEmpty(text))
            throw new ArgumentException("Provide a non-empty string.");
        var codes = text.Select(c => (int)c).ToArray();
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])codes.Clone(),
                Description = $"Palindrome partitioning of \"{text}\"",
            }
        );

        var results = new List<string[]>();
        PalPartHelper(text, 0, new List<string>(), results, codes, steps, ref step);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])codes.Clone(),
                Description = $"Found {results.Count} partition(s)",
            }
        );
        return steps;
    }

    private static void PalPartHelper(
        string s,
        int start,
        List<string> current,
        List<string[]> results,
        int[] codes,
        List<AlgorithmStep> steps,
        ref int step
    )
    {
        if (start == s.Length)
        {
            results.Add(current.ToArray());
            var lens = current.Select(p => p.Length).ToArray();
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = lens,
                    Description = $"Partition: [{string.Join(" | ", current)}]",
                    SortedIndices = Enumerable.Range(0, lens.Length).ToArray(),
                }
            );
            return;
        }
        for (int end = start; end < s.Length; end++)
        {
            if (IsPalindrome(s, start, end))
            {
                var sub = s.Substring(start, end - start + 1);
                current.Add(sub);
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])codes.Clone(),
                        Description = $"Take palindrome \"{sub}\" [{start}..{end}]",
                        HighlightIndices = Enumerable.Range(start, end - start + 1).ToArray(),
                    }
                );
                PalPartHelper(s, end + 1, current, results, codes, steps, ref step);
                current.RemoveAt(current.Count - 1);
            }
        }
    }

    private static bool IsPalindrome(string s, int lo, int hi)
    {
        while (lo < hi)
        {
            if (s[lo++] != s[hi--])
                return false;
        }
        return true;
    }
}

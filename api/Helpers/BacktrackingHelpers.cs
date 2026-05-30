using api.Models;

namespace api.Helpers;

internal static class BacktrackingHelpers
{
    internal static void PermuteHelper(int[] arr, int start, List<AlgorithmStep> steps, ref int step)
    {
        if (start == arr.Length)
        {
            steps.Add(new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])arr.Clone(),
                Description = $"Permutation: [{string.Join(", ", arr)}]",
                SortedIndices = Enumerable.Range(0, arr.Length).ToArray(),
            });
            return;
        }
        for (int i = start; i < arr.Length; i++)
        {
            (arr[start], arr[i]) = (arr[i], arr[start]);
            steps.Add(new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])arr.Clone(),
                Description = $"Swap index {start} and {i}",
                HighlightIndices = [start, i],
            });
            PermuteHelper(arr, start + 1, steps, ref step);
            (arr[start], arr[i]) = (arr[i], arr[start]);
        }
    }

    internal static void SubsetHelper(int[] arr, int idx, List<int> current, List<AlgorithmStep> steps, ref int step)
    {
        steps.Add(new AlgorithmStep
        {
            StepNumber = step++,
            Array = current.Count > 0 ? current.ToArray() : [0],
            Description = current.Count > 0 ? $"Subset: {{{string.Join(", ", current)}}}" : "Subset: {}",
            SortedIndices = Enumerable.Range(0, current.Count).ToArray(),
        });
        for (int i = idx; i < arr.Length; i++)
        {
            current.Add(arr[i]);
            SubsetHelper(arr, i + 1, current, steps, ref step);
            current.RemoveAt(current.Count - 1);
        }
    }

    internal static void CombSumHelper(int[] cands, int remain, int start, List<int> current, List<int[]> results, List<AlgorithmStep> steps, ref int step)
    {
        if (remain == 0)
        {
            results.Add(current.ToArray());
            steps.Add(new AlgorithmStep
            {
                StepNumber = step++,
                Array = current.ToArray(),
                Description = $"Found: [{string.Join(", ", current)}]",
                SortedIndices = Enumerable.Range(0, current.Count).ToArray(),
            });
            return;
        }
        for (int i = start; i < cands.Length && cands[i] <= remain; i++)
        {
            if (steps.Count > 2000)
                return;
            current.Add(cands[i]);
            steps.Add(new AlgorithmStep
            {
                StepNumber = step++,
                Array = current.ToArray(),
                Description = $"Add {cands[i]}, sum={current.Sum()}, need={remain - cands[i]}",
                HighlightIndices = [current.Count - 1],
            });
            CombSumHelper(cands, remain - cands[i], i, current, results, steps, ref step);
            current.RemoveAt(current.Count - 1);
        }
    }

    internal static void PalPartHelper(string s, int start, List<string> current, List<string[]> results, int[] codes, List<AlgorithmStep> steps, ref int step)
    {
        if (start == s.Length)
        {
            results.Add(current.ToArray());
            var lens = current.Select(p => p.Length).ToArray();
            steps.Add(new AlgorithmStep
            {
                StepNumber = step++,
                Array = lens,
                Description = $"Partition: [{string.Join(" | ", current)}]",
                SortedIndices = Enumerable.Range(0, lens.Length).ToArray(),
            });
            return;
        }
        for (int end = start; end < s.Length; end++)
        {
            if (IsPalindrome(s, start, end))
            {
                var sub = s.Substring(start, end - start + 1);
                current.Add(sub);
                steps.Add(new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])codes.Clone(),
                    Description = $"Take palindrome \"{sub}\" [{start}..{end}]",
                    HighlightIndices = Enumerable.Range(start, end - start + 1).ToArray(),
                });
                PalPartHelper(s, end + 1, current, results, codes, steps, ref step);
                current.RemoveAt(current.Count - 1);
            }
        }
    }

    internal static bool IsPalindrome(string s, int lo, int hi)
    {
        while (lo < hi)
        {
            if (s[lo++] != s[hi--])
                return false;
        }
        return true;
    }
}

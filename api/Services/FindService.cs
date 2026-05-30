using api.Helpers;
using api.Models;

namespace api.Services;

public class FindService
{
    public List<AlgorithmStep> BinarySearch(int[] input, int target)
    {
        FindHelpers.ValidateInput(input);
        var steps = new List<AlgorithmStep>();
        var arr = (int[])input.Clone();
        Array.Sort(arr);
        int step = 0;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Sorted array, searching for {target}" });

        int lo = 0, hi = arr.Length - 1;
        while (lo <= hi)
        {
            int mid = lo + (hi - lo) / 2;
            steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Checking mid index {mid} (value {arr[mid]}), range [{lo}..{hi}]", HighlightIndices = [mid] });
            if (arr[mid] == target) { steps.Add(new AlgorithmStep { StepNumber = step, Array = (int[])arr.Clone(), Description = $"Found {target} at index {mid}", SortedIndices = [mid] }); return steps; }
            if (arr[mid] < target) lo = mid + 1; else hi = mid - 1;
        }
        steps.Add(new AlgorithmStep { StepNumber = step, Array = (int[])arr.Clone(), Description = $"{target} not found in array" });
        return steps;
    }

    public List<AlgorithmStep> LinearSearch(int[] input, int target)
    {
        FindHelpers.ValidateInput(input);
        var steps = new List<AlgorithmStep>();
        var arr = (int[])input.Clone();
        int step = 0;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Searching for {target}" });
        for (int i = 0; i < arr.Length; i++)
        {
            steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Checking index {i} (value {arr[i]})", HighlightIndices = [i] });
            if (arr[i] == target) { steps.Add(new AlgorithmStep { StepNumber = step, Array = (int[])arr.Clone(), Description = $"Found {target} at index {i}", SortedIndices = [i] }); return steps; }
        }
        steps.Add(new AlgorithmStep { StepNumber = step, Array = (int[])arr.Clone(), Description = $"{target} not found in array" });
        return steps;
    }

    public List<AlgorithmStep> TwoPointers(int[] input, int target)
    {
        FindHelpers.ValidateInput(input);
        var steps = new List<AlgorithmStep>();
        var arr = (int[])input.Clone();
        Array.Sort(arr);
        int step = 0;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Sorted array, finding pair that sums to {target}" });

        int left = 0, right = arr.Length - 1;
        while (left < right)
        {
            int sum = arr[left] + arr[right];
            steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Left={left} ({arr[left]}) + Right={right} ({arr[right]}) = {sum}", HighlightIndices = [left, right] });
            if (sum == target) { steps.Add(new AlgorithmStep { StepNumber = step, Array = (int[])arr.Clone(), Description = $"Found pair: {arr[left]} + {arr[right]} = {target}", SortedIndices = [left, right] }); return steps; }
            if (sum < target) left++; else right--;
        }
        steps.Add(new AlgorithmStep { StepNumber = step, Array = (int[])arr.Clone(), Description = $"No pair found that sums to {target}" });
        return steps;
    }

    public List<AlgorithmStep> SlidingWindow(int[] input, int windowSize)
    {
        FindHelpers.ValidateInput(input);
        if (windowSize <= 0 || windowSize > input.Length)
            throw new ArgumentException("Window size must be between 1 and array length.");

        var steps = new List<AlgorithmStep>();
        var arr = (int[])input.Clone();
        int step = 0;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Finding max sum subarray of size {windowSize}" });

        int windowSum = 0;
        for (int i = 0; i < windowSize; i++) windowSum += arr[i];
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Initial window [{0}..{windowSize - 1}] sum = {windowSum}", HighlightIndices = Enumerable.Range(0, windowSize).ToArray() });

        int maxSum = windowSum, maxStart = 0;
        for (int i = windowSize; i < arr.Length; i++)
        {
            windowSum += arr[i] - arr[i - windowSize];
            int start = i - windowSize + 1;
            steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Window [{start}..{i}] sum = {windowSum}", HighlightIndices = Enumerable.Range(start, windowSize).ToArray() });
            if (windowSum > maxSum) { maxSum = windowSum; maxStart = start; }
        }

        steps.Add(new AlgorithmStep { StepNumber = step, Array = (int[])arr.Clone(), Description = $"Max sum = {maxSum} at window [{maxStart}..{maxStart + windowSize - 1}]", SortedIndices = Enumerable.Range(maxStart, windowSize).ToArray() });
        return steps;
    }

    public List<AlgorithmStep> Kadane(int[] input)
    {
        FindHelpers.ValidateInput(input);
        var steps = new List<AlgorithmStep>();
        var arr = (int[])input.Clone();
        int step = 0;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = "Finding maximum subarray sum (Kadane's algorithm)" });

        int maxEndingHere = arr[0], maxSoFar = arr[0], start = 0, end = 0, tempStart = 0;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Start: maxEndingHere = {maxEndingHere}, maxSoFar = {maxSoFar}", HighlightIndices = [0] });

        for (int i = 1; i < arr.Length; i++)
        {
            if (arr[i] > maxEndingHere + arr[i]) { maxEndingHere = arr[i]; tempStart = i; steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Reset subarray at index {i}, maxEndingHere = {maxEndingHere}", HighlightIndices = [i] }); }
            else { maxEndingHere += arr[i]; steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Extended subarray to index {i}, maxEndingHere = {maxEndingHere}", HighlightIndices = Enumerable.Range(tempStart, i - tempStart + 1).ToArray() }); }
            if (maxEndingHere > maxSoFar) { maxSoFar = maxEndingHere; start = tempStart; end = i; }
        }

        steps.Add(new AlgorithmStep { StepNumber = step, Array = (int[])arr.Clone(), Description = $"Max subarray sum = {maxSoFar} from index {start} to {end}", SortedIndices = Enumerable.Range(start, end - start + 1).ToArray() });
        return steps;
    }
}

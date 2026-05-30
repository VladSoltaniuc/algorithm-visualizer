using api.Models;

namespace api.Helpers;

internal static class SortHelpers
{
    internal static void ValidateInput(int[] input)
    {
        if (input is not { Length: > 0 })
            throw new ArgumentException("Provide a non-empty array of integers.");
    }

    internal static int Partition(int[] arr, int low, int high, List<AlgorithmStep> steps, ref int step)
    {
        int pivot = arr[high];
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Pivot = {pivot} (index {high})", HighlightIndices = [high] });

        int i = low;
        for (int j = low; j < high; j++)
        {
            steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Comparing {arr[j]} with pivot {pivot}", HighlightIndices = [j, high] });
            if (arr[j] <= pivot)
            {
                (arr[i], arr[j]) = (arr[j], arr[i]);
                if (i != j) steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Swapped index {i} and {j}", HighlightIndices = [i, j] });
                i++;
            }
        }

        (arr[i], arr[high]) = (arr[high], arr[i]);
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Placed pivot {pivot} at index {i}", HighlightIndices = [i] });
        return i;
    }

    internal static void QuickSortHelper(int[] arr, int low, int high, List<AlgorithmStep> steps, ref int step)
    {
        if (low < high)
        {
            int p = Partition(arr, low, high, steps, ref step);
            QuickSortHelper(arr, low, p - 1, steps, ref step);
            QuickSortHelper(arr, p + 1, high, steps, ref step);
        }
    }

    internal static void MergeSortHelper(int[] arr, int left, int right, List<AlgorithmStep> steps, ref int step)
    {
        if (left >= right) return;
        int mid = left + (right - left) / 2;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Splitting [{left}..{mid}] and [{mid + 1}..{right}]", HighlightIndices = Enumerable.Range(left, right - left + 1).ToArray() });
        MergeSortHelper(arr, left, mid, steps, ref step);
        MergeSortHelper(arr, mid + 1, right, steps, ref step);
        Merge(arr, left, mid, right, steps, ref step);
    }

    internal static void Merge(int[] arr, int left, int mid, int right, List<AlgorithmStep> steps, ref int step)
    {
        var temp = new int[right - left + 1];
        int i = left, j = mid + 1, k = 0;
        while (i <= mid && j <= right) { if (arr[i] <= arr[j]) temp[k++] = arr[i++]; else temp[k++] = arr[j++]; }
        while (i <= mid) temp[k++] = arr[i++];
        while (j <= right) temp[k++] = arr[j++];
        Array.Copy(temp, 0, arr, left, temp.Length);
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Merged [{left}..{right}]", HighlightIndices = Enumerable.Range(left, right - left + 1).ToArray() });
    }
}

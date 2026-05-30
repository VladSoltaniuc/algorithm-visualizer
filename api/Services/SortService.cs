using api.Helpers;
using api.Models;

namespace api.Services;

public class SortService
{
    public List<AlgorithmStep> BubbleSort(int[] input)
    {
        SortHelpers.ValidateInput(input);
        var steps = new List<AlgorithmStep>();
        var arr = (int[])input.Clone();
        int step = 0;

        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = "Initial array" });

        for (int i = 0; i < arr.Length - 1; i++)
        {
            bool swapped = false;
            for (int j = 0; j < arr.Length - i - 1; j++)
            {
                steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Comparing index {j} and {j + 1}", HighlightIndices = [j, j + 1] });
                if (arr[j] > arr[j + 1])
                {
                    (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                    swapped = true;
                    steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Swapped index {j} and {j + 1}", HighlightIndices = [j, j + 1] });
                }
            }
            if (!swapped) break;
        }

        steps.Add(new AlgorithmStep { StepNumber = step, Array = (int[])arr.Clone(), Description = "Array sorted", SortedIndices = Enumerable.Range(0, arr.Length).ToArray() });
        return steps;
    }

    public List<AlgorithmStep> QuickSort(int[] input)
    {
        SortHelpers.ValidateInput(input);
        var steps = new List<AlgorithmStep>();
        var arr = (int[])input.Clone();
        int step = 0;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = "Initial array" });
        SortHelpers.QuickSortHelper(arr, 0, arr.Length - 1, steps, ref step);
        steps.Add(new AlgorithmStep { StepNumber = step, Array = (int[])arr.Clone(), Description = "Array sorted", SortedIndices = Enumerable.Range(0, arr.Length).ToArray() });
        return steps;
    }

    public List<AlgorithmStep> MergeSort(int[] input)
    {
        SortHelpers.ValidateInput(input);
        var steps = new List<AlgorithmStep>();
        var arr = (int[])input.Clone();
        int step = 0;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = "Initial array" });
        SortHelpers.MergeSortHelper(arr, 0, arr.Length - 1, steps, ref step);
        steps.Add(new AlgorithmStep { StepNumber = step, Array = (int[])arr.Clone(), Description = "Array sorted", SortedIndices = Enumerable.Range(0, arr.Length).ToArray() });
        return steps;
    }

    public List<AlgorithmStep> InsertionSort(int[] input)
    {
        SortHelpers.ValidateInput(input);
        var steps = new List<AlgorithmStep>();
        var arr = (int[])input.Clone();
        int step = 0;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = "Initial array" });

        for (int i = 1; i < arr.Length; i++)
        {
            int key = arr[i], j = i - 1;
            steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Key = {key} at index {i}", HighlightIndices = [i] });
            while (j >= 0 && arr[j] > key)
            {
                arr[j + 1] = arr[j];
                steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Shifted {arr[j + 1]} from index {j} to {j + 1}", HighlightIndices = [j, j + 1] });
                j--;
            }
            arr[j + 1] = key;
            steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Inserted {key} at index {j + 1}", HighlightIndices = [j + 1] });
        }

        steps.Add(new AlgorithmStep { StepNumber = step, Array = (int[])arr.Clone(), Description = "Array sorted", SortedIndices = Enumerable.Range(0, arr.Length).ToArray() });
        return steps;
    }

    public List<AlgorithmStep> SelectionSort(int[] input)
    {
        SortHelpers.ValidateInput(input);
        var steps = new List<AlgorithmStep>();
        var arr = (int[])input.Clone();
        int step = 0;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = "Initial array" });

        for (int i = 0; i < arr.Length - 1; i++)
        {
            int minIdx = i;
            for (int j = i + 1; j < arr.Length; j++)
            {
                steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Comparing index {j} (value {arr[j]}) with current min index {minIdx} (value {arr[minIdx]})", HighlightIndices = [j, minIdx] });
                if (arr[j] < arr[minIdx]) minIdx = j;
            }
            if (minIdx != i)
            {
                (arr[i], arr[minIdx]) = (arr[minIdx], arr[i]);
                steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Swapped index {i} and {minIdx}", HighlightIndices = [i, minIdx] });
            }
        }

        steps.Add(new AlgorithmStep { StepNumber = step, Array = (int[])arr.Clone(), Description = "Array sorted", SortedIndices = Enumerable.Range(0, arr.Length).ToArray() });
        return steps;
    }
}

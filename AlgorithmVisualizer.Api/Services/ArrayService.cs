using AlgorithmVisualizer.Api.Models;

namespace AlgorithmVisualizer.Api.Services;

// TODO: Separate code in smaller services for better code readability
// TODO: Get rid of regions
// TODO: Show specs on hove over title and little "?" icon or "info" icon
public class ArrayService
{
    private static void ValidateInput(int[] input)
    {
        if (input is not { Length: > 0 })
            throw new ArgumentException("Provide a non-empty array of integers.");
    }

    // Best:    Time O(n)    Space O(1)
    // Average: Time O(n^2)  Space O(1)
    // Worst:   Time O(n^2)  Space O(1)
    // Note: O(n) best case requires an early-exit flag (no swaps made in a pass)
    //       TODO: This implementation omits the flag, so it always runs O(n²).
    // Real world: Teaching/visualization purposes,
    //             nearly-sorted small datasets where simplicity is valued,
    //             detecting if a list is already sorted (via the O(n) best case)
    #region BubbleSort
    public List<AlgorithmStep> BubbleSort(int[] input)
    {
        ValidateInput(input);
        var steps = new List<AlgorithmStep>();
        var arr = (int[])input.Clone();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])arr.Clone(),
                Description = "Initial array",
            }
        );

        for (int i = 0; i < arr.Length - 1; i++)
        {
            for (int j = 0; j < arr.Length - i - 1; j++)
            {
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])arr.Clone(),
                        Description = $"Comparing index {j} and {j + 1}",
                        HighlightIndices = [j, j + 1],
                    }
                );

                if (arr[j] > arr[j + 1])
                {
                    (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                    steps.Add(
                        new AlgorithmStep
                        {
                            StepNumber = step++,
                            Array = (int[])arr.Clone(),
                            Description = $"Swapped index {j} and {j + 1}",
                            HighlightIndices = [j, j + 1],
                        }
                    );
                }
            }
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])arr.Clone(),
                Description = "Array sorted",
                SortedIndices = Enumerable.Range(0, arr.Length).ToArray(),
            }
        );

        return steps;
    }
    #endregion

    // Best:    Time O(n log n)  Space O(log n)
    // Average: Time O(n log n)  Space O(log n)
    // Worst:   Time O(n^2)      Space O(n)
    // Note: Worst case occurs on already-sorted input with Lomuto partition (pivot always min/max)
    // Real world: General-purpose in-memory sorting (used in most stdlib sorts),
    //             database query result sorting,
    //             language runtimes (C qsort, Java Arrays.sort for primitives)
    #region QuickSort (Lomuto)
    public List<AlgorithmStep> QuickSort(int[] input)
    {
        ValidateInput(input);
        var steps = new List<AlgorithmStep>();
        var arr = (int[])input.Clone();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])arr.Clone(),
                Description = "Initial array",
            }
        );

        QuickSortHelper(arr, 0, arr.Length - 1, steps, ref step);

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])arr.Clone(),
                Description = "Array sorted",
                SortedIndices = Enumerable.Range(0, arr.Length).ToArray(),
            }
        );

        return steps;
    }

    private static int Partition(
        int[] arr,
        int low,
        int high,
        List<AlgorithmStep> steps,
        ref int step
    )
    {
        int pivot = arr[high];

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])arr.Clone(),
                Description = $"Pivot = {pivot} (index {high})",
                HighlightIndices = [high],
            }
        );

        int i = low;
        for (int j = low; j < high; j++)
        {
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])arr.Clone(),
                    Description = $"Comparing {arr[j]} with pivot {pivot}",
                    HighlightIndices = [j, high],
                }
            );

            if (arr[j] <= pivot)
            {
                (arr[i], arr[j]) = (arr[j], arr[i]);
                if (i != j)
                {
                    steps.Add(
                        new AlgorithmStep
                        {
                            StepNumber = step++,
                            Array = (int[])arr.Clone(),
                            Description = $"Swapped index {i} and {j}",
                            HighlightIndices = [i, j],
                        }
                    );
                }
                i++;
            }
        }

        (arr[i], arr[high]) = (arr[high], arr[i]);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])arr.Clone(),
                Description = $"Placed pivot {pivot} at index {i}",
                HighlightIndices = [i],
            }
        );

        return i;
    }

    private static void QuickSortHelper(
        int[] arr,
        int low,
        int high,
        List<AlgorithmStep> steps,
        ref int step
    )
    {
        if (low < high)
        {
            int partitionIndex = Partition(arr, low, high, steps, ref step);
            QuickSortHelper(arr, low, partitionIndex - 1, steps, ref step);
            QuickSortHelper(arr, partitionIndex + 1, high, steps, ref step);
        }
    }
    #endregion

    // Best:    Time O(n log n)  Space O(n)
    // Average: Time O(n log n)  Space O(n)
    // Worst:   Time O(n log n)  Space O(n)
    // Real world: External sorting (sorting data too large for RAM),
    //             stable sort requirement (preserving equal element order),
    //             sorting linked lists efficiently
    //             maximum rainfall period analysis" or "ad click revenue burst detection
    #region MergeSort
    public List<AlgorithmStep> MergeSort(int[] input)
    {
        ValidateInput(input);
        var steps = new List<AlgorithmStep>();
        var arr = (int[])input.Clone();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])arr.Clone(),
                Description = "Initial array",
            }
        );

        MergeSortHelper(arr, 0, arr.Length - 1, steps, ref step);

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])arr.Clone(),
                Description = "Array sorted",
                SortedIndices = Enumerable.Range(0, arr.Length).ToArray(),
            }
        );

        return steps;
    }

    private static void MergeSortHelper(
        int[] arr,
        int left,
        int right,
        List<AlgorithmStep> steps,
        ref int step
    )
    {
        if (left >= right)
            return;

        int mid = left + (right - left) / 2;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])arr.Clone(),
                Description = $"Splitting [{left}..{mid}] and [{mid + 1}..{right}]",
                HighlightIndices = Enumerable.Range(left, right - left + 1).ToArray(),
            }
        );

        MergeSortHelper(arr, left, mid, steps, ref step);
        MergeSortHelper(arr, mid + 1, right, steps, ref step);
        Merge(arr, left, mid, right, steps, ref step);
    }

    private static void Merge(
        int[] arr,
        int left,
        int mid,
        int right,
        List<AlgorithmStep> steps,
        ref int step
    )
    {
        var temp = new int[right - left + 1];
        int i = left,
            j = mid + 1,
            k = 0;

        while (i <= mid && j <= right)
        {
            if (arr[i] <= arr[j])
                temp[k++] = arr[i++];
            else
                temp[k++] = arr[j++];
        }
        while (i <= mid)
            temp[k++] = arr[i++];
        while (j <= right)
            temp[k++] = arr[j++];

        Array.Copy(temp, 0, arr, left, temp.Length);

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])arr.Clone(),
                Description = $"Merged [{left}..{right}]",
                HighlightIndices = Enumerable.Range(left, right - left + 1).ToArray(),
            }
        );
    }
    #endregion

    // Best:    Time O(1)      Space O(1)
    // Average: Time O(log n)  Space O(1)
    // Worst:   Time O(log n)  Space O(1)
    // Note: Recursive implementation uses O(log n) space
    // Real world: Dictionary/autocomplete lookups,
    //             finding a version that introduced a bug (git bisect),
    //             database index range lookups (B-tree search)
    #region BinarySearch
    public List<AlgorithmStep> BinarySearch(int[] input, int target)
    {
        ValidateInput(input);
        var steps = new List<AlgorithmStep>();
        var arr = (int[])input.Clone();
        Array.Sort(arr);
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])arr.Clone(),
                Description = $"Sorted array, searching for {target}",
            }
        );

        int lo = 0,
            hi = arr.Length - 1;
        while (lo <= hi)
        {
            int mid = lo + (hi - lo) / 2;
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])arr.Clone(),
                    Description =
                        $"Checking mid index {mid} (value {arr[mid]}), range [{lo}..{hi}]",
                    HighlightIndices = [mid],
                }
            );

            if (arr[mid] == target)
            {
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step,
                        Array = (int[])arr.Clone(),
                        Description = $"Found {target} at index {mid}",
                        SortedIndices = [mid],
                    }
                );
                return steps;
            }

            if (arr[mid] < target)
                lo = mid + 1;
            else
                hi = mid - 1;
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])arr.Clone(),
                Description = $"{target} not found in array",
            }
        );

        return steps;
    }
    #endregion

    // Best:    Time O(1)  Space O(1)
    // Average: Time O(n)  Space O(1)
    // Worst:   Time O(n)  Space O(1)
    // Real world: Searching unsorted/unindexed data (logs, raw files),
    //             finding a value in a linked list,
    //             fallback search when data isn't worth sorting,
    //             feature flag lookup in a short config list
    #region LinearSearch
    public List<AlgorithmStep> LinearSearch(int[] input, int target)
    {
        ValidateInput(input);
        var steps = new List<AlgorithmStep>();
        var arr = (int[])input.Clone();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])arr.Clone(),
                Description = $"Searching for {target}",
            }
        );

        for (int i = 0; i < arr.Length; i++)
        {
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])arr.Clone(),
                    Description = $"Checking index {i} (value {arr[i]})",
                    HighlightIndices = [i],
                }
            );

            if (arr[i] == target)
            {
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step,
                        Array = (int[])arr.Clone(),
                        Description = $"Found {target} at index {i}",
                        SortedIndices = [i],
                    }
                );
                return steps;
            }
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])arr.Clone(),
                Description = $"{target} not found in array",
            }
        );

        return steps;
    }
    #endregion

    // TODO: The visualisation duplicates the number just before the swap, it should instantly swap without duplicating the value which looks weird
    // Best:    Time O(n)    Space O(1)
    // Average: Time O(n^2)  Space O(1)
    // Worst:   Time O(n^2)  Space O(1)
    // Real world: Sorting small arrays (used inside Timsort/IntroSort),
    //             inserting new records into an already-sorted list,
    //             online sorting (data arriving one item at a time)
    #region InsertionSort
    public List<AlgorithmStep> InsertionSort(int[] input)
    {
        ValidateInput(input);
        var steps = new List<AlgorithmStep>();
        var arr = (int[])input.Clone();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])arr.Clone(),
                Description = "Initial array",
            }
        );

        for (int i = 1; i < arr.Length; i++)
        {
            int key = arr[i];
            int j = i - 1;

            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])arr.Clone(),
                    Description = $"Key = {key} at index {i}",
                    HighlightIndices = [i],
                }
            );

            while (j >= 0 && arr[j] > key)
            {
                arr[j + 1] = arr[j];
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])arr.Clone(),
                        Description = $"Shifted {arr[j + 1]} from index {j} to {j + 1}",
                        HighlightIndices = [j, j + 1],
                    }
                );
                j--;
            }

            arr[j + 1] = key;
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])arr.Clone(),
                    Description = $"Inserted {key} at index {j + 1}",
                    HighlightIndices = [j + 1],
                }
            );
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])arr.Clone(),
                Description = "Array sorted",
                SortedIndices = Enumerable.Range(0, arr.Length).ToArray(),
            }
        );

        return steps;
    }
    #endregion

    // Best:    Time O(n^2)  Space O(1)
    // Average: Time O(n^2)  Space O(1)
    // Worst:   Time O(n^2)  Space O(1)
    // Real world: Embedded systems with very limited memory,
    //             sorting small fixed-size datasets,
    //             finding the k smallest elements iteratively,
    //             tournament bracket seeding (pick best N from a pool)"
    #region SelectionSort
    public List<AlgorithmStep> SelectionSort(int[] input)
    {
        ValidateInput(input);
        var steps = new List<AlgorithmStep>();
        var arr = (int[])input.Clone();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])arr.Clone(),
                Description = "Initial array",
            }
        );

        for (int i = 0; i < arr.Length - 1; i++)
        {
            int minIdx = i;
            for (int j = i + 1; j < arr.Length; j++)
            {
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])arr.Clone(),
                        Description =
                            $"Comparing index {j} (value {arr[j]}) with current min index {minIdx} (value {arr[minIdx]})",
                        HighlightIndices = [j, minIdx],
                    }
                );

                if (arr[j] < arr[minIdx])
                    minIdx = j;
            }

            if (minIdx != i)
            {
                (arr[i], arr[minIdx]) = (arr[minIdx], arr[i]);
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])arr.Clone(),
                        Description = $"Swapped index {i} and {minIdx}",
                        HighlightIndices = [i, minIdx],
                    }
                );
            }
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])arr.Clone(),
                Description = "Array sorted",
                SortedIndices = Enumerable.Range(0, arr.Length).ToArray(),
            }
        );

        return steps;
    }
    #endregion

    // Best:    Time O(n log n)  Space O(1)
    // Average: Time O(n log n)  Space O(1)
    // Worst:   Time O(n log n)  Space O(1)
    // Note: Includes an O(n log n) sort; the two-pointer scan itself is O(n)
    // Real world: Pair sum problems in financial data,
    //             removing duplicates from sorted arrays,
    //             checking if a string is a palindrome
    #region TwoPointers

    public List<AlgorithmStep> TwoPointers(int[] input, int target)
    {
        ValidateInput(input);
        var steps = new List<AlgorithmStep>();
        var arr = (int[])input.Clone();
        Array.Sort(arr);
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])arr.Clone(),
                Description = $"Sorted array, finding pair that sums to {target}",
            }
        );

        int left = 0,
            right = arr.Length - 1;
        while (left < right)
        {
            int sum = arr[left] + arr[right];
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])arr.Clone(),
                    Description =
                        $"Left={left} ({arr[left]}) + Right={right} ({arr[right]}) = {sum}",
                    HighlightIndices = [left, right],
                }
            );

            if (sum == target)
            {
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step,
                        Array = (int[])arr.Clone(),
                        Description = $"Found pair: {arr[left]} + {arr[right]} = {target}",
                        SortedIndices = [left, right],
                    }
                );
                return steps;
            }

            if (sum < target)
                left++;
            else
                right--;
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])arr.Clone(),
                Description = $"No pair found that sums to {target}",
            }
        );

        return steps;
    }
    #endregion

    // Best:    Time O(n)  Space O(1)
    // Average: Time O(n)  Space O(1)
    // Worst:   Time O(n)  Space O(1)
    // Real world: Network rate limiting (requests per time window),
    //             finding longest substring without repeating chars,
    //             real-time analytics over rolling time windows
    #region SlidingWindow
    public List<AlgorithmStep> SlidingWindow(int[] input, int windowSize)
    {
        ValidateInput(input);
        if (windowSize <= 0 || windowSize > input.Length)
            throw new ArgumentException("Window size must be between 1 and array length.");

        var steps = new List<AlgorithmStep>();
        var arr = (int[])input.Clone();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])arr.Clone(),
                Description = $"Finding max sum subarray of size {windowSize}",
            }
        );

        int windowSum = 0;
        for (int i = 0; i < windowSize; i++)
            windowSum += arr[i];

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])arr.Clone(),
                Description = $"Initial window [{0}..{windowSize - 1}] sum = {windowSum}",
                HighlightIndices = Enumerable.Range(0, windowSize).ToArray(),
            }
        );

        int maxSum = windowSum;
        int maxStart = 0;

        for (int i = windowSize; i < arr.Length; i++)
        {
            windowSum += arr[i] - arr[i - windowSize];
            int start = i - windowSize + 1;

            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])arr.Clone(),
                    Description = $"Window [{start}..{i}] sum = {windowSum}",
                    HighlightIndices = Enumerable.Range(start, windowSize).ToArray(),
                }
            );

            if (windowSum > maxSum)
            {
                maxSum = windowSum;
                maxStart = start;
            }
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])arr.Clone(),
                Description =
                    $"Max sum = {maxSum} at window [{maxStart}..{maxStart + windowSize - 1}]",
                SortedIndices = Enumerable.Range(maxStart, windowSize).ToArray(),
            }
        );

        return steps;
    }
    #endregion

    // Best:    Time O(n)  Space O(1)
    // Average: Time O(n)  Space O(1)
    // Worst:   Time O(n)  Space O(1)
    // Real world: Stock profit maximization (max gain over a period),
    //             signal processing (finding strongest continuous signal),
    //             game score streak tracking
    #region Kadane
    public List<AlgorithmStep> Kadane(int[] input)
    {
        ValidateInput(input);
        var steps = new List<AlgorithmStep>();
        var arr = (int[])input.Clone();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])arr.Clone(),
                Description = "Finding maximum subarray sum (Kadane's algorithm)",
            }
        );

        int maxEndingHere = arr[0];
        int maxSoFar = arr[0];
        int start = 0,
            end = 0,
            tempStart = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])arr.Clone(),
                Description = $"Start: maxEndingHere = {maxEndingHere}, maxSoFar = {maxSoFar}",
                HighlightIndices = [0],
            }
        );

        for (int i = 1; i < arr.Length; i++)
        {
            if (arr[i] > maxEndingHere + arr[i])
            {
                maxEndingHere = arr[i];
                tempStart = i;

                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])arr.Clone(),
                        Description =
                            $"Reset subarray at index {i}, maxEndingHere = {maxEndingHere}",
                        HighlightIndices = [i],
                    }
                );
            }
            else
            {
                maxEndingHere += arr[i];

                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])arr.Clone(),
                        Description =
                            $"Extended subarray to index {i}, maxEndingHere = {maxEndingHere}",
                        HighlightIndices = Enumerable.Range(tempStart, i - tempStart + 1).ToArray(),
                    }
                );
            }

            if (maxEndingHere > maxSoFar)
            {
                maxSoFar = maxEndingHere;
                start = tempStart;
                end = i;
            }
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])arr.Clone(),
                Description = $"Max subarray sum = {maxSoFar} from index {start} to {end}",
                SortedIndices = Enumerable.Range(start, end - start + 1).ToArray(),
            }
        );

        return steps;
    }
    #endregion
}

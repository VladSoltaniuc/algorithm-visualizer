using AlgorithmVisualizer.Api.Models;

namespace AlgorithmVisualizer.Api.Services;

public class DynamicProgService
{
    // 1. Fibonacci
    // Time: O(n)
    // Space: O(n)
    public List<AlgorithmStep> Fibonacci(int n)
    {
        if (n < 0)
            throw new ArgumentException("n must be non-negative.");
        var steps = new List<AlgorithmStep>();
        var dp = new int[Math.Max(n + 1, 2)];
        dp[0] = 0;
        dp[1] = 1;
        int step = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])dp.Clone(),
                Description = "F(0)=0, F(1)=1",
            }
        );

        for (int i = 2; i <= n; i++)
        {
            dp[i] = dp[i - 1] + dp[i - 2];
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = dp[..(n + 1)],
                    Description = $"F({i}) = F({i - 1}) + F({i - 2}) = {dp[i]}",
                    HighlightIndices = [i],
                }
            );
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = dp[..(n + 1)],
                Description = $"F({n}) = {dp[n]}",
                SortedIndices = Enumerable.Range(0, n + 1).ToArray(),
            }
        );
        return steps;
    }

    // 2. 0/1 Knapsack
    // Time: O(n · W) where W = capacity
    // Space: O(W)
    public List<AlgorithmStep> Knapsack(int[] weights, int[] values, int capacity)
    {
        if (weights.Length == 0)
            throw new ArgumentException("Provide non-empty weights.");
        int n = weights.Length;
        var dp = new int[capacity + 1];
        var steps = new List<AlgorithmStep>();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])dp.Clone(),
                Description = $"Knapsack: {n} items, capacity={capacity}",
            }
        );

        for (int i = 0; i < n; i++)
        {
            for (int w = capacity; w >= weights[i]; w--)
            {
                if (dp[w - weights[i]] + values[i] > dp[w])
                    dp[w] = dp[w - weights[i]] + values[i];
            }
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])dp.Clone(),
                    Description = $"Item {i} (w={weights[i]}, v={values[i]}): dp updated",
                    HighlightIndices = Enumerable
                        .Range(weights[i], capacity - weights[i] + 1)
                        .ToArray(),
                }
            );
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])dp.Clone(),
                Description = $"Max value = {dp[capacity]}",
                SortedIndices = Enumerable.Range(0, capacity + 1).ToArray(),
            }
        );
        return steps;
    }

    // 3. Longest Common Subsequence
    // Time: O(m · n)
    // Space: O(m · n)
    public List<AlgorithmStep> Lcs(string text1, string text2)
    {
        if (string.IsNullOrEmpty(text1) || string.IsNullOrEmpty(text2))
            throw new ArgumentException("Provide non-empty strings.");
        // Longest string on top (columns), shortest on the side (rows)
        if (text1.Length > text2.Length)
            (text1, text2) = (text2, text1);
        int m = text1.Length,
            n = text2.Length;
        var dp = new int[m + 1, n + 1];
        var steps = new List<AlgorithmStep>();
        int step = 0;

        string rowHeaders = " " + text1;
        string colHeaders = " " + text2;

        int[][] SnapshotMatrix()
        {
            var matrix = new int[m + 1][];
            for (int r = 0; r <= m; r++)
            {
                matrix[r] = new int[n + 1];
                for (int c = 0; c <= n; c++)
                    matrix[r][c] = dp[r, c];
            }
            return matrix;
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = new int[n + 1],
                Description =
                    $"LCS of \"{text1}\" and \"{text2}\" — initialise ({m + 1})x({n + 1}) matrix with zeros",
                DpMatrix = SnapshotMatrix(),
                RowHeaders = rowHeaders,
                ColHeaders = colHeaders,
            }
        );

        for (int i = 1; i <= m; i++)
        {
            for (int j = 1; j <= n; j++)
            {
                dp[i, j] =
                    text1[i - 1] == text2[j - 1]
                        ? dp[i - 1, j - 1] + 1
                        : Math.Max(dp[i - 1, j], dp[i, j - 1]);
                string desc =
                    text1[i - 1] == text2[j - 1]
                        ? $"'{text1[i - 1]}' == '{text2[j - 1]}': dp[{i}][{j}] = dp[{i - 1}][{j - 1}] + 1 = {dp[i, j]}"
                        : $"'{text1[i - 1]}' ≠ '{text2[j - 1]}': dp[{i}][{j}] = max({dp[i - 1, j]}, {dp[i, j - 1]}) = {dp[i, j]}";
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Description = desc,
                        DpMatrix = SnapshotMatrix(),
                        RowHeaders = rowHeaders,
                        ColHeaders = colHeaders,
                        HighlightRow = i,
                        HighlightCol = j,
                    }
                );
            }
        }

        // Backtrack to find the LCS string
        var lcs = new List<char>();
        var backtrackCells = new List<int>();
        int x = m,
            y = n;
        while (x > 0 && y > 0)
        {
            if (text1[x - 1] == text2[y - 1])
            {
                backtrackCells.Add(x);
                backtrackCells.Add(y);
                lcs.Add(text1[x - 1]);
                x--;
                y--;
            }
            else if (dp[x - 1, y] > dp[x, y - 1])
                x--;
            else
                y--;
        }
        lcs.Reverse();

        var finalRow = new int[n + 1];
        for (int j = 0; j <= n; j++)
            finalRow[j] = dp[m, j];
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = finalRow,
                Description = $"LCS = \"{new string(lcs.ToArray())}\" (length {lcs.Count})",
                SortedIndices = Enumerable.Range(0, n + 1).ToArray(),
                DpMatrix = SnapshotMatrix(),
                RowHeaders = rowHeaders,
                ColHeaders = colHeaders,
                BacktrackPath = backtrackCells.ToArray(),
            }
        );
        return steps;
    }

    // Time: O(n^2)
    // Space: O(n)
    public List<AlgorithmStep> Lis(int[] arr)
    {
        if (arr.Length == 0)
            throw new ArgumentException("Provide a non-empty array.");
        int n = arr.Length;
        var dp = new int[n];
        Array.Fill(dp, 1);
        var steps = new List<AlgorithmStep>();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])dp.Clone(),
                Description = $"LIS of [{string.Join(", ", arr)}]",
            }
        );

        for (int i = 1; i < n; i++)
        {
            for (int j = 0; j < i; j++)
                if (arr[j] < arr[i])
                    dp[i] = Math.Max(dp[i], dp[j] + 1);
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])dp.Clone(),
                    Description = $"dp[{i}] = {dp[i]} (element {arr[i]})",
                    HighlightIndices = [i],
                }
            );
        }

        int maxLis = dp.Max();
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])dp.Clone(),
                Description = $"LIS length = {maxLis}",
                SortedIndices = Enumerable.Range(0, n).Where(i => dp[i] == maxLis).ToArray(),
            }
        );
        return steps;
    }

    // 5. Coin Change
    // Time: O(n · amount)
    // Space: O(amount)
    public List<AlgorithmStep> CoinChange(int[] coins, int amount)
    {
        if (coins.Length == 0)
            throw new ArgumentException("Provide non-empty coins.");

        int inf = amount + 1; // sentinel for "unreachable"
        var dp = new int[amount + 1];
        Array.Fill(dp, inf);
        dp[0] = 0;

        var steps = new List<AlgorithmStep>();
        int stepNum = 0;

        // Display labels: "?" = not started, "∞" = unreachable, numeral = computed
        string[] MakeNotes(int upTo) =>
            Enumerable
                .Range(0, amount + 1)
                .Select(j =>
                {
                    if (j > upTo)
                        return "?";
                    if (dp[j] >= inf)
                        return "∞";
                    return dp[j].ToString();
                })
                .ToArray();

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = stepNum++,
                Array = (int[])dp.Clone(),
                Notes = MakeNotes(0),
                Description =
                    $"Goal: fewest coins from [{string.Join(", ", coins)}] that sum to {amount}. "
                    + "dp[0] = 0 (zero coins needed for amount 0). All other cells start as ∞ (unreachable).",
                SortedIndices = [0],
            }
        );

        for (int i = 1; i <= amount; i++)
        {
            var finishedSoFar = Enumerable.Range(0, i).ToArray();

            foreach (int coin in coins)
            {
                if (coin > i)
                    continue;

                int refVal = dp[i - coin];
                int prevBest = dp[i];
                string lookupStr = $"look up dp[{i} − {coin}] = dp[{i - coin}]";

                string outcome;
                if (refVal >= inf)
                {
                    outcome =
                        $"{lookupStr} = ∞ — that sub-amount is unreachable, so coin {coin} can't help here.";
                }
                else
                {
                    int candidate = refVal + 1;
                    bool improved = candidate < prevBest;
                    if (improved)
                        dp[i] = candidate;

                    string prevStr = prevBest >= inf ? "∞" : prevBest.ToString();
                    outcome = improved
                        ? $"{lookupStr} = {refVal}, so {refVal} + 1 = {candidate} coin(s). "
                            + $"Current dp[{i}] = {prevStr} → updated to {candidate}!"
                        : $"{lookupStr} = {refVal}, so {refVal} + 1 = {candidate} coin(s). "
                            + $"Current dp[{i}] = {prevStr} is already ≤ {candidate} — no change.";
                }

                string curBest = dp[i] >= inf ? "∞" : dp[i].ToString();
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = stepNum++,
                        Array = (int[])dp.Clone(),
                        Notes = MakeNotes(i),
                        Description =
                            $"Amount {i}, try coin {coin}: {outcome} Best so far: dp[{i}] = {curBest}.",
                        HighlightIndices = [i],
                        SortedIndices = finishedSoFar,
                    }
                );
            }

            // Finalize dp[i] — flip it from active (red) to done (green)
            string finalStr =
                dp[i] >= inf ? "∞ (cannot be formed with these coins)" : dp[i].ToString();
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = stepNum++,
                    Array = (int[])dp.Clone(),
                    Notes = MakeNotes(i),
                    Description = $"dp[{i}] = {finalStr}. All coins tried for amount {i}.",
                    SortedIndices = Enumerable.Range(0, i + 1).ToArray(),
                }
            );
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = stepNum,
                Array = (int[])dp.Clone(),
                Notes = MakeNotes(amount),
                Description =
                    dp[amount] >= inf
                        ? $"No solution — amount {amount} cannot be formed from [{string.Join(", ", coins)}]."
                        : $"Answer: {dp[amount]} coin(s) to reach amount {amount}.",
                SortedIndices = Enumerable.Range(0, amount + 1).ToArray(),
            }
        );

        return steps;
    }

    // 7. Edit Distance
    // Time: O(m · n)
    // Space: O(m · n)
    public List<AlgorithmStep> EditDistance(string text1, string text2)
    {
        if (string.IsNullOrEmpty(text1) || string.IsNullOrEmpty(text2))
            throw new ArgumentException("Provide non-empty strings.");
        int m = text1.Length,
            n = text2.Length;
        var dp = new int[m + 1, n + 1];
        for (int j = 0; j <= n; j++)
            dp[0, j] = j;
        for (int i = 0; i <= m; i++)
            dp[i, 0] = i;
        var steps = new List<AlgorithmStep>();
        int step = 0;

        var initRow = new int[n + 1];
        for (int j = 0; j <= n; j++)
            initRow[j] = dp[0, j];
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = initRow,
                Description = $"Edit distance: \"{text1}\" → \"{text2}\"",
            }
        );

        for (int i = 1; i <= m; i++)
        {
            for (int j = 1; j <= n; j++)
            {
                int cost = text1[i - 1] == text2[j - 1] ? 0 : 1;
                dp[i, j] = Math.Min(
                    Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                    dp[i - 1, j - 1] + cost
                );
            }
            var row = new int[n + 1];
            for (int j = 0; j <= n; j++)
                row[j] = dp[i, j];
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = row,
                    Description = $"Row {i} ('{text1[i - 1]}')",
                    HighlightIndices = [i],
                }
            );
        }

        var finalRow = new int[n + 1];
        for (int j = 0; j <= n; j++)
            finalRow[j] = dp[m, j];
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = finalRow,
                Description = $"Edit distance = {dp[m, n]}",
                SortedIndices = Enumerable.Range(0, n + 1).ToArray(),
            }
        );
        return steps;
    }

    // 9. Subset Sum
    // Time: O(n · target)
    // Space: O(target)
    public List<AlgorithmStep> SubsetSum(int[] arr, int target)
    {
        if (arr.Length == 0)
            throw new ArgumentException("Provide a non-empty array.");
        var dp = new int[target + 1]; // 0=false, 1=true
        dp[0] = 1;
        var steps = new List<AlgorithmStep>();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])dp.Clone(),
                Description = $"Subset sum: [{string.Join(", ", arr)}], target={target}",
            }
        );

        foreach (int num in arr)
        {
            for (int j = target; j >= num; j--)
                if (dp[j - num] == 1)
                    dp[j] = 1;
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])dp.Clone(),
                    Description = $"Include {num}: reachable sums updated",
                    HighlightIndices = Enumerable
                        .Range(0, target + 1)
                        .Where(i => dp[i] == 1)
                        .ToArray(),
                }
            );
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])dp.Clone(),
                Description =
                    dp[target] == 1
                        ? $"Target {target} is achievable ✓"
                        : $"Target {target} is not achievable ✗",
                SortedIndices = Enumerable.Range(0, target + 1).Where(i => dp[i] == 1).ToArray(),
            }
        );
        return steps;
    }

    // 10. Climbing Stairs
    // Time: O(n)
    // Space: O(n)
    public List<AlgorithmStep> ClimbingStairs(int n)
    {
        if (n < 1)
            throw new ArgumentException("n must be at least 1.");
        var dp = new int[n + 1];
        dp[0] = 1;
        if (n >= 1)
            dp[1] = 1;
        var steps = new List<AlgorithmStep>();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])dp.Clone(),
                Description = $"Climbing {n} stairs (1 or 2 steps at a time)",
            }
        );

        for (int i = 2; i <= n; i++)
        {
            dp[i] = dp[i - 1] + dp[i - 2];
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])dp.Clone(),
                    Description = $"dp[{i}] = dp[{i - 1}] + dp[{i - 2}] = {dp[i]}",
                    HighlightIndices = [i],
                }
            );
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])dp.Clone(),
                Description = $"Ways to climb {n} stairs = {dp[n]}",
                SortedIndices = Enumerable.Range(0, n + 1).ToArray(),
            }
        );
        return steps;
    }
}

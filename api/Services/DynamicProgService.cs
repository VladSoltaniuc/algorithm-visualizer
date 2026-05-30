using api.Helpers;
using api.Models;

namespace api.Services;

public class DynamicProgService
{
    // 1. Fibonacci
    // Time: O(n)
    // Space: O(n)
    public List<AlgorithmStep> Fibonacci(int n)
    {
        if (n < 0)
            throw new ArgumentException("n must be non-negative.");
        if (n > 100)
            throw new ArgumentException("Fibonacci is limited to n ≤ 100 to keep the visualization manageable.");
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
    // Time: O(n Â· W)
    // Space: O(n Â· W) full 2-D table for matrix visualisation
    public List<AlgorithmStep> Knapsack(int[] weights, int[] values, int capacity)
    {
        if (weights.Length == 0)
            throw new ArgumentException("Provide non-empty weights.");
        if (weights.Length > 20)
            throw new ArgumentException("Knapsack is limited to 20 items.");
        if (capacity < 0)
            throw new ArgumentException("Capacity must be non-negative.");
        if (capacity > 500)
            throw new ArgumentException("Knapsack capacity is limited to 500.");
        if (values.Length != weights.Length)
            throw new ArgumentException($"Values array must have the same number of entries as weights ({weights.Length}).");
        int n = weights.Length;
        var dp = new int[n + 1, capacity + 1]; // dp[i][w] = max value, first i items, capacity w

        var rowLabels = new string[n + 1];
        rowLabels[0] = "âˆ…";
        for (int i = 1; i <= n; i++)
            rowLabels[i] = $"#{i - 1}(w={weights[i - 1]},v={values[i - 1]})";
        var colLabels = Enumerable.Range(0, capacity + 1).Select(w => w.ToString()).ToArray();

        var steps = new List<AlgorithmStep>();
        int stepNum = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = stepNum++,
                Array = [],
                Description =
                    $"0/1 Knapsack {n} items, capacity {capacity}. Row 0 = 0 (no items selected).",
                DpMatrix = DynamicProgHelpers.SnapshotMatrix(dp, n + 1, capacity + 1),
                RowLabels = rowLabels,
                ColLabels = colLabels,
                HighlightRow = 0,
            }
        );

        for (int i = 1; i <= n; i++)
        {
            int wi = weights[i - 1],
                vi = values[i - 1];
            for (int w = 0; w <= capacity; w++)
            {
                dp[i, w] = w < wi ? dp[i - 1, w] : Math.Max(dp[i - 1, w], dp[i - 1, w - wi] + vi);
            }
            int rowAns = dp[i, capacity];
            bool improved = rowAns > dp[i - 1, capacity];
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = stepNum++,
                    Array = [],
                    Description =
                        $"Item {i - 1} (w={wi}, v={vi}): best value at capacity {capacity} = {rowAns}"
                        + (improved ? $" â†‘ (was {dp[i - 1, capacity]})" : " (unchanged)")
                        + ".",
                    DpMatrix = DynamicProgHelpers.SnapshotMatrix(dp, n + 1, capacity + 1),
                    RowLabels = rowLabels,
                    ColLabels = colLabels,
                    HighlightRow = i,
                    HighlightCol = capacity,
                }
            );
        }

        // Backtrack: find which items were selected
        var bt = new List<int>();
        var selected = new List<int>();
        int bi = n,
            bw = capacity;
        while (bi > 0 && bw >= 0)
        {
            if (dp[bi, bw] != dp[bi - 1, bw])
            {
                bt.Add(bi);
                bt.Add(bw);
                selected.Add(bi - 1);
                bw -= weights[bi - 1];
            }
            bi--;
        }

        string selStr =
            selected.Count > 0
                ? $" Selected: {string.Join(", ", selected.Select(i => $"#{i}(v={values[i]})"))} â†’ total value {dp[n, capacity]}."
                : " No items selected.";
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = stepNum,
                Array = [],
                Description = $"Max value = {dp[n, capacity]}.{selStr}",
                DpMatrix = DynamicProgHelpers.SnapshotMatrix(dp, n + 1, capacity + 1),
                RowLabels = rowLabels,
                ColLabels = colLabels,
                BacktrackPath = bt.ToArray(),
            }
        );
        return steps;
    }

    // 3. Longest Common Subsequence
    // Time: O(m Â· n)
    // Space: O(m Â· n)
    public List<AlgorithmStep> Lcs(string text1, string text2)
    {
        if (string.IsNullOrEmpty(text1) || string.IsNullOrEmpty(text2))
            throw new ArgumentException("Provide non-empty strings.");
        if (text1.Length > 25 || text2.Length > 25)
            throw new ArgumentException("LCS strings are each limited to 25 characters - the DP matrix grows as m×n.");
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
                    $"LCS of \"{text1}\" and \"{text2}\" initialise ({m + 1})x({n + 1}) matrix with zeros",
                DpMatrix = DynamicProgHelpers.SnapshotMatrix(dp, m + 1, n + 1),
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
                        : $"'{text1[i - 1]}' â‰  '{text2[j - 1]}': dp[{i}][{j}] = max({dp[i - 1, j]}, {dp[i, j - 1]}) = {dp[i, j]}";
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Description = desc,
                        DpMatrix = DynamicProgHelpers.SnapshotMatrix(dp, m + 1, n + 1),
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
                DpMatrix = DynamicProgHelpers.SnapshotMatrix(dp, m + 1, n + 1),
                RowHeaders = rowHeaders,
                ColHeaders = colHeaders,
                BacktrackPath = backtrackCells.ToArray(),
            }
        );
        return steps;
    }

    // Time: O(n^2)
    // Space: O(n^2) comparison matrix for visualisation
    public List<AlgorithmStep> Lis(int[] arr)
    {
        if (arr.Length == 0)
            throw new ArgumentException("Provide a non-empty array.");
        if (arr.Length > 30)
            throw new ArgumentException("LIS is limited to 30 elements - the visualization matrix grows as n².");
        int n = arr.Length;
        var dp = new int[n];
        Array.Fill(dp, 1);
        var pred = new int[n];
        Array.Fill(pred, -1);

        // matrix[i][j]:
        //  j == i  â†’ dp[i] (LIS length ending at arr[i])
        //  j < i   â†’ dp[j]+1 if arr[j] < arr[i], else 0 (invalid predecessor)
        //  j > i   â†’ -1 ("â€“", not yet computed)
        var matrix = new int[n][];
        for (int i = 0; i < n; i++)
        {
            matrix[i] = new int[n];
            Array.Fill(matrix[i], -1);
        }

        // Row and column headers = array element values
        var lbls = arr.Select(v => v.ToString()).ToArray();

        int[][] Snap()
        {
            var s = new int[n][];
            for (int r = 0; r < n; r++)
                s[r] = (int[])matrix[r].Clone();
            return s;
        }

        var steps = new List<AlgorithmStep>();
        int stepNum = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = stepNum++,
                Array = [],
                Description =
                    $"LIS of [{string.Join(", ", arr)}] each row i shows: "
                    + "candidates dp[j]+1 (green when arr[j] < arr[i]), "
                    + "0 (invalid), diagonal = dp[i].",
                DpMatrix = DynamicProgHelpers.SnapshotJaggedMatrix(matrix),
                RowLabels = lbls,
                ColLabels = lbls,
            }
        );

        for (int i = 0; i < n; i++)
        {
            // Fill comparisons for row i
            for (int j = 0; j < i; j++)
            {
                if (arr[j] < arr[i])
                {
                    int cand = dp[j] + 1;
                    matrix[i][j] = cand;
                    if (cand > dp[i])
                    {
                        dp[i] = cand;
                        pred[i] = j;
                    }
                }
                else
                {
                    matrix[i][j] = 0;
                }
            }
            matrix[i][i] = dp[i];

            string note =
                i == 0 ? "No predecessors dp[0] = 1."
                : pred[i] >= 0
                    ? $"Best predecessor: arr[{pred[i]}]={arr[pred[i]]} â†’ dp[{i}] = {dp[i]}."
                : $"No valid predecessors dp[{i}] = 1.";
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = stepNum++,
                    Array = (int[])dp.Clone(),
                    Description = $"arr[{i}] = {arr[i]}: dp[{i}] = {dp[i]}. {note}",
                    DpMatrix = DynamicProgHelpers.SnapshotJaggedMatrix(matrix),
                    RowLabels = lbls,
                    ColLabels = lbls,
                    HighlightRow = i,
                    HighlightCol = i,
                }
            );
        }

        // Find LIS and backtrack
        int maxLis = dp.Max();
        int lisEnd = Array.LastIndexOf(dp, maxLis);
        var lisIdx = new List<int>();
        for (int cur = lisEnd; cur != -1; cur = pred[cur])
            lisIdx.Add(cur);
        lisIdx.Reverse();

        // BacktrackPath: for each element in LIS, include its diagonal cell and
        // the predecessor-connection cell (i, pred[i])
        var bt = new List<int>();
        for (int k = 0; k < lisIdx.Count; k++)
        {
            int idx = lisIdx[k];
            bt.Add(idx);
            bt.Add(idx); // diagonal
            if (k > 0)
            {
                bt.Add(idx);
                bt.Add(lisIdx[k - 1]); // predecessor cell
            }
        }

        string lisStr = string.Join(", ", lisIdx.Select(i => arr[i].ToString()));
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = stepNum,
                Array = (int[])dp.Clone(),
                Description = $"LIS length = {maxLis}. Sequence: [{lisStr}].",
                DpMatrix = DynamicProgHelpers.SnapshotJaggedMatrix(matrix),
                RowLabels = lbls,
                ColLabels = lbls,
                SortedIndices = Enumerable.Range(0, n).ToArray(),
                BacktrackPath = bt.ToArray(),
            }
        );
        return steps;
    }

    // 5. Coin Change
    // Time: O(n Â· amount)
    // Space: O(amount)
    public List<AlgorithmStep> CoinChange(int[] coins, int amount)
    {
        if (coins.Length == 0)
            throw new ArgumentException("Provide non-empty coins.");
        if (amount > 500)
            throw new ArgumentException("Coin change amount is limited to 500.");

        int inf = amount + 1; // sentinel for "unreachable"
        var dp = new int[amount + 1];
        Array.Fill(dp, inf);
        dp[0] = 0;

        // coinUsed[i]: -1 = base case (amount 0), 0 = not yet set, positive = coin denomination
        var coinUsed = new int[amount + 1];
        coinUsed[0] = -1;

        var steps = new List<AlgorithmStep>();
        int stepNum = 0;

        // Display value for each cell; "?" = not yet computed
        string[] MakeNotes(int upTo) =>
            Enumerable
                .Range(0, amount + 1)
                .Select(j => j > upTo || dp[j] >= inf ? "?" : dp[j].ToString())
                .ToArray();

        // Coin-denomination label row: "" = not reached, "âˆ…" = base, "âœ-" = unreachable, "+C" = coin C
        string[] MakeCoinLabels(int upTo) =>
            Enumerable
                .Range(0, amount + 1)
                .Select(j =>
                {
                    if (j > upTo)
                        return "";
                    if (j == 0)
                        return "âˆ…";
                    if (dp[j] >= inf)
                        return "âœ-";
                    if (coinUsed[j] == 0)
                        return "";
                    return $"+{coinUsed[j]}";
                })
                .ToArray();

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = stepNum++,
                Array = (int[])dp.Clone(),
                Notes = MakeNotes(0),
                Labels = MakeCoinLabels(0),
                Description =
                    $"dp[0] = 0 zero coins needed for amount 0. "
                    + "All other cells start as âˆž (unreachable so far).",
                SortedIndices = [0],
                PatternOffset = -1,
            }
        );

        for (int i = 1; i <= amount; i++)
        {
            int[] finishedSoFar = Enumerable.Range(0, i).ToArray();

            foreach (int coin in coins)
            {
                if (coin > i)
                    continue;

                int lookupIdx = i - coin;
                int refVal = dp[lookupIdx];
                int prevBest = dp[i];

                if (refVal >= inf)
                {
                    steps.Add(
                        new AlgorithmStep
                        {
                            StepNumber = stepNum++,
                            Array = (int[])dp.Clone(),
                            Notes = MakeNotes(i),
                            Labels = MakeCoinLabels(i - 1),
                            Description = $"Coin {coin}: dp[{lookupIdx}] = âˆž (unreachable) skip.",
                            HighlightIndices = [i],
                            PatternOffset = lookupIdx,
                            SortedIndices = finishedSoFar,
                        }
                    );
                }
                else
                {
                    int candidate = refVal + 1;
                    bool improved = candidate < prevBest;
                    if (improved)
                    {
                        dp[i] = candidate;
                        coinUsed[i] = coin;
                    }

                    string prevStr = prevBest >= inf ? "âˆž" : prevBest.ToString();
                    string outcome = improved
                        ? $"dp[{i}]: {prevStr} â†’ {candidate} âœ“"
                        : $"dp[{i}] stays {prevBest} (already â‰¤ {candidate})";

                    steps.Add(
                        new AlgorithmStep
                        {
                            StepNumber = stepNum++,
                            Array = (int[])dp.Clone(),
                            Notes = MakeNotes(i),
                            Labels = MakeCoinLabels(i - 1),
                            Description =
                                $"Coin {coin}: dp[{lookupIdx}] + 1 = {refVal} + 1 = {candidate}. {outcome}.",
                            HighlightIndices = [i],
                            PatternOffset = lookupIdx,
                            SortedIndices = finishedSoFar,
                        }
                    );
                }
            }

            // Finalize dp[i]
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = stepNum++,
                    Array = (int[])dp.Clone(),
                    Notes = MakeNotes(i),
                    Labels = MakeCoinLabels(i),
                    Description =
                        dp[i] >= inf
                            ? $"dp[{i}] = âˆž amount {i} cannot be formed."
                            : $"dp[{i}] = {dp[i]}. Best coin: +{coinUsed[i]}.",
                    PatternOffset = -1,
                    SortedIndices = Enumerable.Range(0, i + 1).ToArray(),
                }
            );
        }

        // Trace back optimal path for the final summary step
        int[] tracePath = [];
        string coinsUsedStr = "";
        if (dp[amount] < inf)
        {
            var pathIndices = new List<int>();
            var usedList = new List<int>();
            int cur = amount;
            while (cur > 0)
            {
                pathIndices.Add(cur);
                usedList.Add(coinUsed[cur]);
                cur -= coinUsed[cur];
            }
            tracePath = pathIndices.ToArray();
            coinsUsedStr = $" Coins: {string.Join(" + ", usedList)} = {amount}";
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = stepNum,
                Array = (int[])dp.Clone(),
                Notes = MakeNotes(amount),
                Labels = MakeCoinLabels(amount),
                Description =
                    dp[amount] >= inf
                        ? $"No solution {amount} cannot be formed from [{string.Join(", ", coins)}]."
                        : $"Answer: {dp[amount]} coin(s) to make {amount}.{coinsUsedStr}",
                SortedIndices = Enumerable.Range(0, amount + 1).ToArray(),
                HighlightIndices = tracePath,
                PatternOffset = -1,
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
        if (n > 50)
            throw new ArgumentException("Climbing stairs is limited to n ≤ 50.");
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

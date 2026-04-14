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
    // Time: O(n · W)
    // Space: O(n · W) — full 2-D table for matrix visualisation
    public List<AlgorithmStep> Knapsack(int[] weights, int[] values, int capacity)
    {
        if (weights.Length == 0)
            throw new ArgumentException("Provide non-empty weights.");
        int n = weights.Length;
        var dp = new int[n + 1, capacity + 1]; // dp[i][w] = max value, first i items, capacity w

        var rowLabels = new string[n + 1];
        rowLabels[0] = "∅";
        for (int i = 1; i <= n; i++)
            rowLabels[i] = $"#{i - 1}(w={weights[i - 1]},v={values[i - 1]})";
        var colLabels = Enumerable.Range(0, capacity + 1).Select(w => w.ToString()).ToArray();

        int[][] Snap()
        {
            var m = new int[n + 1][];
            for (int r = 0; r <= n; r++)
            {
                m[r] = new int[capacity + 1];
                for (int c = 0; c <= capacity; c++)
                    m[r][c] = dp[r, c];
            }
            return m;
        }

        var steps = new List<AlgorithmStep>();
        int stepNum = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = stepNum++,
                Array = [],
                Description =
                    $"0/1 Knapsack — {n} items, capacity {capacity}. Row 0 = 0 (no items selected).",
                DpMatrix = Snap(),
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
                        + (improved ? $" ↑ (was {dp[i - 1, capacity]})" : " (unchanged)")
                        + ".",
                    DpMatrix = Snap(),
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
                ? $" Selected: {string.Join(", ", selected.Select(i => $"#{i}(v={values[i]})"))} → total value {dp[n, capacity]}."
                : " No items selected.";
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = stepNum,
                Array = [],
                Description = $"Max value = {dp[n, capacity]}.{selStr}",
                DpMatrix = Snap(),
                RowLabels = rowLabels,
                ColLabels = colLabels,
                BacktrackPath = bt.ToArray(),
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
    // Space: O(n^2) — comparison matrix for visualisation
    public List<AlgorithmStep> Lis(int[] arr)
    {
        if (arr.Length == 0)
            throw new ArgumentException("Provide a non-empty array.");
        int n = arr.Length;
        var dp = new int[n];
        Array.Fill(dp, 1);
        var pred = new int[n];
        Array.Fill(pred, -1);

        // matrix[i][j]:
        //  j == i  → dp[i] (LIS length ending at arr[i])
        //  j < i   → dp[j]+1 if arr[j] < arr[i], else 0 (invalid predecessor)
        //  j > i   → -1 ("–", not yet computed)
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
                    $"LIS of [{string.Join(", ", arr)}] — each row i shows: "
                    + "candidates dp[j]+1 (green when arr[j] < arr[i]), "
                    + "0 (invalid), diagonal = dp[i].",
                DpMatrix = Snap(),
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
                i == 0 ? "No predecessors — dp[0] = 1."
                : pred[i] >= 0
                    ? $"Best predecessor: arr[{pred[i]}]={arr[pred[i]]} → dp[{i}] = {dp[i]}."
                : $"No valid predecessors — dp[{i}] = 1.";
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = stepNum++,
                    Array = (int[])dp.Clone(),
                    Description = $"arr[{i}] = {arr[i]}: dp[{i}] = {dp[i]}. {note}",
                    DpMatrix = Snap(),
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
                DpMatrix = Snap(),
                RowLabels = lbls,
                ColLabels = lbls,
                SortedIndices = Enumerable.Range(0, n).ToArray(),
                BacktrackPath = bt.ToArray(),
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

        // coinUsed[i]: -1 = base case (amount 0), 0 = not yet set, positive = coin denomination
        var coinUsed = new int[amount + 1];
        coinUsed[0] = -1;

        var steps = new List<AlgorithmStep>();
        int stepNum = 0;

        // Display value for each cell; "?" = not yet computed
        string[] MakeNotes(int upTo) =>
            Enumerable
                .Range(0, amount + 1)
                .Select(j =>
                    j > upTo ? "?"
                    : dp[j] >= inf ? "∞"
                    : dp[j].ToString()
                )
                .ToArray();

        // Coin-denomination label row: "" = not reached, "∅" = base, "✗" = unreachable, "+C" = coin C
        string[] MakeCoinLabels(int upTo) =>
            Enumerable
                .Range(0, amount + 1)
                .Select(j =>
                {
                    if (j > upTo)
                        return "";
                    if (j == 0)
                        return "∅";
                    if (dp[j] >= inf)
                        return "✗";
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
                    $"dp[0] = 0 — zero coins needed for amount 0. "
                    + "All other cells start as ∞ (unreachable so far).",
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
                            Description = $"Coin {coin}: dp[{lookupIdx}] = ∞ (unreachable) — skip.",
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

                    string prevStr = prevBest >= inf ? "∞" : prevBest.ToString();
                    string outcome = improved
                        ? $"dp[{i}]: {prevStr} → {candidate} ✓"
                        : $"dp[{i}] stays {prevBest} (already ≤ {candidate})";

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
                            ? $"dp[{i}] = ∞ — amount {i} cannot be formed."
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
            pathIndices.Add(0);
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
                        ? $"No solution — {amount} cannot be formed from [{string.Join(", ", coins)}]."
                        : $"Answer: {dp[amount]} coin(s) to make {amount}.{coinsUsedStr}",
                SortedIndices = Enumerable.Range(0, amount + 1).ToArray(),
                HighlightIndices = tracePath,
                PatternOffset = -1,
            }
        );

        return steps;
    }

    // 7. Levenshtein Distance
    // Time: O(m · n)
    // Space: O(m · n)
    public List<AlgorithmStep> Levenshtein(string text1, string text2)
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

        var steps = new List<AlgorithmStep>();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = [],
                Description =
                    $"Levenshtein: \"{text1}\" → \"{text2}\" — initialise {m + 1}×{n + 1} matrix. "
                    + $"Row 0 = inserts needed to build \"{text2}\" from empty. "
                    + $"Col 0 = deletes needed to empty \"{text1}\".",
                DpMatrix = SnapshotMatrix(),
                RowHeaders = rowHeaders,
                ColHeaders = colHeaders,
            }
        );

        for (int i = 1; i <= m; i++)
        {
            for (int j = 1; j <= n; j++)
            {
                bool match = text1[i - 1] == text2[j - 1];
                int deleteCost = dp[i - 1, j] + 1;
                int insertCost = dp[i, j - 1] + 1;
                int replaceCost = dp[i - 1, j - 1] + (match ? 0 : 1);
                dp[i, j] = Math.Min(Math.Min(deleteCost, insertCost), replaceCost);

                string desc;
                if (match)
                {
                    desc =
                        $"'{text1[i - 1]}' == '{text2[j - 1]}': characters match — no edit needed. "
                        + $"dp[{i}][{j}] = dp[{i - 1}][{j - 1}] = {dp[i, j]}";
                }
                else
                {
                    string bestOp;
                    if (dp[i, j] == replaceCost)
                        bestOp =
                            $"replace '{text1[i - 1]}' with '{text2[j - 1]}' (cost {replaceCost})";
                    else if (dp[i, j] == deleteCost)
                        bestOp = $"delete '{text1[i - 1]}' (cost {deleteCost})";
                    else
                        bestOp = $"insert '{text2[j - 1]}' (cost {insertCost})";
                    desc =
                        $"'{text1[i - 1]}' ≠ '{text2[j - 1]}': best is {bestOp}. "
                        + $"min(delete={deleteCost}, insert={insertCost}, replace={replaceCost}) = {dp[i, j]}";
                }

                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = [],
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

        // Backtrack to find the optimal edit path
        var backtrackCells = new List<int>();
        int x = m,
            y = n;
        while (x > 0 || y > 0)
        {
            backtrackCells.Add(x);
            backtrackCells.Add(y);
            if (x == 0)
            {
                y--;
            }
            else if (y == 0)
            {
                x--;
            }
            else if (text1[x - 1] == text2[y - 1])
            {
                x--;
                y--;
            }
            else
            {
                int minVal = Math.Min(Math.Min(dp[x - 1, y], dp[x, y - 1]), dp[x - 1, y - 1]);
                if (minVal == dp[x - 1, y - 1])
                {
                    x--;
                    y--;
                }
                else if (minVal == dp[x - 1, y])
                {
                    x--;
                }
                else
                {
                    y--;
                }
            }
        }
        backtrackCells.Add(0);
        backtrackCells.Add(0);

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = [],
                Description =
                    $"Edit distance = {dp[m, n]}. "
                    + $"The highlighted path shows the cheapest sequence of edits to turn \"{text1}\" into \"{text2}\".",
                DpMatrix = SnapshotMatrix(),
                RowHeaders = rowHeaders,
                ColHeaders = colHeaders,
                BacktrackPath = backtrackCells.ToArray(),
            }
        );
        return steps;
    }

    // 9. Subset Sum
    // Time: O(n · target)
    // Space: O(n · target) — full 2-D table for matrix visualisation
    public List<AlgorithmStep> SubsetSum(int[] arr, int target)
    {
        if (arr.Length == 0)
            throw new ArgumentException("Provide a non-empty array.");
        int n = arr.Length;
        // dp[i][j] = 1 (true) if sum j is reachable using first i elements
        var dp = new int[n + 1, target + 1];
        dp[0, 0] = 1;

        var rowLabels = new string[n + 1];
        rowLabels[0] = "∅";
        for (int i = 1; i <= n; i++)
            rowLabels[i] = arr[i - 1].ToString();
        var colLabels = Enumerable.Range(0, target + 1).Select(j => j.ToString()).ToArray();

        int[][] Snap()
        {
            var m = new int[n + 1][];
            for (int r = 0; r <= n; r++)
            {
                m[r] = new int[target + 1];
                for (int c = 0; c <= target; c++)
                    m[r][c] = dp[r, c];
            }
            return m;
        }

        var steps = new List<AlgorithmStep>();
        int stepNum = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = stepNum++,
                Array = [],
                Description =
                    $"Subset sum: [{string.Join(", ", arr)}], target = {target}. "
                    + "dp[0][0] = 1 (empty set reaches sum 0). All others = 0.",
                DpMatrix = Snap(),
                RowLabels = rowLabels,
                ColLabels = colLabels,
                HighlightRow = 0,
                HighlightCol = 0,
            }
        );

        for (int i = 1; i <= n; i++)
        {
            int num = arr[i - 1];
            for (int j = 0; j <= target; j++)
            {
                dp[i, j] = dp[i - 1, j] == 1 ? 1 : (j >= num && dp[i - 1, j - num] == 1 ? 1 : 0);
            }

            bool reachable = dp[i, target] == 1;
            string gained = num > target ? "larger than target — no new sums." : "";
            if (string.IsNullOrEmpty(gained))
            {
                // Summarise newly reachable sums
                var newSums = Enumerable
                    .Range(0, target + 1)
                    .Where(j => dp[i, j] == 1 && dp[i - 1, j] == 0)
                    .ToList();
                gained =
                    newSums.Count > 0
                        ? $"New reachable sums: {string.Join(", ", newSums)}."
                        : "No new sums.";
            }
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = stepNum++,
                    Array = [],
                    Description =
                        $"Add {num}: {gained} dp[{i}][{target}] = {(reachable ? "1 ✓" : "0 ✗")}.",
                    DpMatrix = Snap(),
                    RowLabels = rowLabels,
                    ColLabels = colLabels,
                    HighlightRow = i,
                    HighlightCol = target,
                }
            );
        }

        // Backtrack to find one valid subset (if achievable)
        var bt = new List<int>();
        var subset = new List<int>();
        if (dp[n, target] == 1)
        {
            int bi = n,
                bj = target;
            while (bi > 0 && bj > 0)
            {
                if (dp[bi - 1, bj] == 0 && dp[bi, bj] == 1)
                {
                    bt.Add(bi);
                    bt.Add(bj);
                    subset.Add(arr[bi - 1]);
                    bj -= arr[bi - 1];
                }
                bi--;
            }
        }

        string subStr =
            subset.Count > 0 ? $" Subset: {{{string.Join(" + ", subset)}}} = {target}." : "";
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = stepNum,
                Array = [],
                Description =
                    dp[n, target] == 1
                        ? $"Target {target} is achievable ✓.{subStr}"
                        : $"Target {target} is not achievable ✗.",
                DpMatrix = Snap(),
                RowLabels = rowLabels,
                ColLabels = colLabels,
                BacktrackPath = bt.ToArray(),
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

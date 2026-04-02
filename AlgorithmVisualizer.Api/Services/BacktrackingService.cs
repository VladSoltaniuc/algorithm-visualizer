using AlgorithmVisualizer.Api.Models;

namespace AlgorithmVisualizer.Api.Services;

public class BacktrackingService
{
    // 1. N-Queens
    // Time: O(n!)
    // Space: O(n)
    public List<AlgorithmStep> NQueens(int n)
    {
        if (n < 1)
            throw new ArgumentException("n must be at least 1.");
        var board = new int[n];
        Array.Fill(board, -1);
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])board.Clone(),
                Description = $"N-Queens: placing {n} queens (-1 = empty)",
            }
        );
        NQueensHelper(board, 0, n, steps, ref step);
        return steps;
    }

    private static bool NQueensHelper(
        int[] board,
        int row,
        int n,
        List<AlgorithmStep> steps,
        ref int step
    )
    {
        if (row == n)
        {
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])board.Clone(),
                    Description =
                        $"Solution found! Queens at columns: [{string.Join(", ", board)}]",
                    SortedIndices = Enumerable.Range(0, n).ToArray(),
                }
            );
            return true;
        }
        for (int col = 0; col < n; col++)
        {
            if (IsSafeQueen(board, row, col))
            {
                board[row] = col;
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])board.Clone(),
                        Description = $"Place queen at row {row}, col {col}",
                        HighlightIndices = [row],
                    }
                );
                if (NQueensHelper(board, row + 1, n, steps, ref step))
                    return true;
                board[row] = -1;
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])board.Clone(),
                        Description = $"Backtrack row {row}",
                        HighlightIndices = [row],
                    }
                );
            }
        }
        return false;
    }

    private static bool IsSafeQueen(int[] board, int row, int col)
    {
        for (int i = 0; i < row; i++)
            if (board[i] == col || Math.Abs(board[i] - col) == Math.Abs(i - row))
                return false;
        return true;
    }

    // 2. Sudoku Solver (takes 81-element flat array, 0 = empty)
    // Time: O(9^m) where m = number of empty cells
    // Space: O(m)
    public List<AlgorithmStep> Sudoku(int[] grid)
    {
        if (grid.Length != 81)
            throw new ArgumentException("Provide exactly 81 values (0 for empty).");
        var board = (int[])grid.Clone();
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])board.Clone(),
                Description = "Sudoku: solving (0 = empty cell)",
            }
        );
        SudokuHelper(board, steps, ref step);
        return steps;
    }

    private static bool SudokuHelper(int[] board, List<AlgorithmStep> steps, ref int step)
    {
        int pos = Array.IndexOf(board, 0);
        if (pos == -1)
        {
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])board.Clone(),
                    Description = "Sudoku solved!",
                    SortedIndices = Enumerable.Range(0, 81).ToArray(),
                }
            );
            return true;
        }
        int row = pos / 9,
            col = pos % 9;
        for (int num = 1; num <= 9; num++)
        {
            if (IsSafeSudoku(board, row, col, num))
            {
                board[pos] = num;
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])board.Clone(),
                        Description = $"Try {num} at ({row},{col})",
                        HighlightIndices = [pos],
                    }
                );
                if (SudokuHelper(board, steps, ref step))
                    return true;
                board[pos] = 0;
                if (step < 500) // Limit steps for UI
                    steps.Add(
                        new AlgorithmStep
                        {
                            StepNumber = step++,
                            Array = (int[])board.Clone(),
                            Description = $"Backtrack ({row},{col})",
                            HighlightIndices = [pos],
                        }
                    );
            }
        }
        return false;
    }

    private static bool IsSafeSudoku(int[] board, int row, int col, int num)
    {
        for (int i = 0; i < 9; i++)
        {
            if (board[row * 9 + i] == num)
                return false;
            if (board[i * 9 + col] == num)
                return false;
        }
        int br = (row / 3) * 3,
            bc = (col / 3) * 3;
        for (int i = br; i < br + 3; i++)
        for (int j = bc; j < bc + 3; j++)
            if (board[i * 9 + j] == num)
                return false;
        return true;
    }

    // 3. Generate All Permutations
    // Time: O(n! · n)
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

    // 5. Rat in a Maze (flat n*n grid, 1=open, 0=blocked; route param n)
    // Time: O(2^(n^2))
    // Space: O(n^2)
    public List<AlgorithmStep> RatInMaze(int[] grid, int n)
    {
        if (grid.Length != n * n)
            throw new ArgumentException($"Grid must have {n * n} elements for {n}×{n} maze.");
        var path = new int[n * n];
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])grid.Clone(),
                Description = $"Rat in {n}×{n} maze (1=open, 0=blocked)",
            }
        );

        if (grid[0] == 1)
        {
            path[0] = 1;
            if (RatHelper(grid, path, 0, 0, n, steps, ref step))
            {
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step,
                        Array = (int[])path.Clone(),
                        Description = "Path found!",
                        SortedIndices = Enumerable
                            .Range(0, n * n)
                            .Where(i => path[i] == 1)
                            .ToArray(),
                    }
                );
            }
            else
            {
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step,
                        Array = (int[])path.Clone(),
                        Description = "No path exists",
                    }
                );
            }
        }
        else
        {
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step,
                    Array = (int[])path.Clone(),
                    Description = "Start blocked — no path",
                }
            );
        }
        return steps;
    }

    private static bool RatHelper(
        int[] grid,
        int[] path,
        int r,
        int c,
        int n,
        List<AlgorithmStep> steps,
        ref int step
    )
    {
        if (r == n - 1 && c == n - 1)
            return true;
        int[] dr = [1, 0],
            dc = [0, 1];
        for (int d = 0; d < 2; d++)
        {
            int nr = r + dr[d],
                nc = c + dc[d];
            if (nr < n && nc < n && grid[nr * n + nc] == 1 && path[nr * n + nc] == 0)
            {
                path[nr * n + nc] = 1;
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])path.Clone(),
                        Description = $"Move to ({nr},{nc})",
                        HighlightIndices = [nr * n + nc],
                    }
                );
                if (RatHelper(grid, path, nr, nc, n, steps, ref step))
                    return true;
                path[nr * n + nc] = 0;
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])path.Clone(),
                        Description = $"Backtrack from ({nr},{nc})",
                        HighlightIndices = [nr * n + nc],
                    }
                );
            }
        }
        return false;
    }
    // 7. Word Search in Grid
    // Time: O(m·n·4^L) where m·n = grid size, L = word length
    // Space: O(L)
    public List<AlgorithmStep> WordSearch(string gridStr, string word)
    {
        if (string.IsNullOrEmpty(gridStr) || string.IsNullOrEmpty(word))
            throw new ArgumentException("Provide grid and word.");
        var rows = gridStr.Split(';');
        int m = rows.Length,
            n = rows[0].Length;
        var grid = new char[m, n];
        var flat = new int[m * n];
        for (int i = 0; i < m; i++)
        for (int j = 0; j < n; j++)
        {
            grid[i, j] = j < rows[i].Length ? rows[i][j] : ' ';
            flat[i * n + j] = grid[i, j];
        }

        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])flat.Clone(),
                Description = $"Searching for \"{word}\" in grid",
            }
        );

        var visited = new bool[m, n];
        for (int i = 0; i < m; i++)
        for (int j = 0; j < n; j++)
            if (grid[i, j] == word[0])
                if (WordSearchHelper(grid, word, 0, i, j, m, n, visited, flat, steps, ref step))
                    return steps;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])flat.Clone(),
                Description = $"\"{word}\" not found",
            }
        );
        return steps;
    }

    private static bool WordSearchHelper(
        char[,] grid,
        string word,
        int idx,
        int r,
        int c,
        int m,
        int n,
        bool[,] visited,
        int[] flat,
        List<AlgorithmStep> steps,
        ref int step
    )
    {
        if (idx == word.Length)
        {
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])flat.Clone(),
                    Description = $"Found \"{word}\"!",
                    SortedIndices = Enumerable
                        .Range(0, m * n)
                        .Where(i => visited[i / n, i % n])
                        .ToArray(),
                }
            );
            return true;
        }
        if (r < 0 || r >= m || c < 0 || c >= n || visited[r, c] || grid[r, c] != word[idx])
            return false;

        visited[r, c] = true;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])flat.Clone(),
                Description = $"Match '{word[idx]}' at ({r},{c})",
                HighlightIndices = [r * n + c],
            }
        );

        int[] dr = [-1, 1, 0, 0],
            dc = [0, 0, -1, 1];
        for (int d = 0; d < 4; d++)
            if (
                WordSearchHelper(
                    grid,
                    word,
                    idx + 1,
                    r + dr[d],
                    c + dc[d],
                    m,
                    n,
                    visited,
                    flat,
                    steps,
                    ref step
                )
            )
                return true;

        visited[r, c] = false;
        return false;
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
    // Time: O(n · 2^n)
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

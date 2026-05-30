namespace api.Helpers;

internal static class DynamicProgHelpers
{
    // Snapshots a 2D DP table into a jagged array for frontend visualisation.
    internal static int[][] SnapshotMatrix(int[,] dp, int rows, int cols)
    {
        var matrix = new int[rows][];
        for (int r = 0; r < rows; r++)
        {
            matrix[r] = new int[cols];
            for (int c = 0; c < cols; c++)
                matrix[r][c] = dp[r, c];
        }
        return matrix;
    }

    // Snapshots a jagged DP matrix (deep clone).
    internal static int[][] SnapshotJaggedMatrix(int[][] matrix)
    {
        var s = new int[matrix.Length][];
        for (int r = 0; r < matrix.Length; r++)
            s[r] = (int[])matrix[r].Clone();
        return s;
    }
}

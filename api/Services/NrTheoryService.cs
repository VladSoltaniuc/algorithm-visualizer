using api.Models;

namespace api.Services;

public class NrTheoryService
{
    // 9. Bit Manipulation
    // Time: O(log n)
    // Space: O(1)
    public List<AlgorithmStep> BitManipulation(int n)
    {
        if (n < 0)
            throw new ArgumentException("n must be non-negative.");
        var steps = new List<AlgorithmStep>();
        int step = 0;
        var bits = ToBits(n);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = bits,
                Description = $"n = {n}, binary = {Convert.ToString(n, 2)}",
            }
        );

        // Count set bits
        int count = 0;
        int temp = n;
        while (temp > 0)
        {
            count += temp & 1;
            temp >>= 1;
        }
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = bits,
                Description = $"Set bits count = {count}",
                HighlightIndices = Enumerable
                    .Range(0, bits.Length)
                    .Where(i => bits[i] == 1)
                    .ToArray(),
            }
        );

        // Check power of 2
        bool isPow2 = n > 0 && (n & (n - 1)) == 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = [n, n - 1, n & (n - 1)],
                Description =
                    $"n & (n-1) = {n & (n - 1)} â†’ {(isPow2 ? "power of 2" : "not power of 2")}",
                HighlightIndices = [2],
            }
        );

        // XOR with self
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = [n, n, n ^ n],
                Description = $"n XOR n = {n ^ n} (always 0)",
                HighlightIndices = [2],
            }
        );

        // Toggle bits
        int toggled = ~n;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = ToBits(toggled & ((1 << bits.Length) - 1)),
                Description =
                    $"NOT n = {Convert.ToString(toggled & ((1 << bits.Length) - 1), 2)} (within bit width)",
                SortedIndices = Enumerable.Range(0, bits.Length).ToArray(),
            }
        );
        return steps;
    }

    private static int[] ToBits(int n)
    {
        if (n == 0)
            return [0];
        var bits = new List<int>();
        int temp = n;
        while (temp > 0)
        {
            bits.Add(temp & 1);
            temp >>= 1;
        }
        bits.Reverse();
        return bits.ToArray();
    }
}

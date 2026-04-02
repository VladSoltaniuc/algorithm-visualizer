using AlgorithmVisualizer.Api.Models;

namespace AlgorithmVisualizer.Api.Services;

public class NrTheoryService
{
    // 1. Sieve of Eratosthenes
    // Time: O(n log log n)
    // Space: O(n)
    public List<AlgorithmStep> Sieve(int n)
    {
        if (n < 2)
            throw new ArgumentException("n must be at least 2.");
        var isPrime = new int[n + 1];
        Array.Fill(isPrime, 1);
        isPrime[0] = 0;
        isPrime[1] = 0;
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])isPrime.Clone(),
                Description = $"Sieve of Eratosthenes up to {n}",
            }
        );

        for (int i = 2; i * i <= n; i++)
        {
            if (isPrime[i] == 1)
            {
                var crossed = new List<int>();
                for (int j = i * i; j <= n; j += i)
                {
                    if (isPrime[j] == 1)
                        crossed.Add(j);
                    isPrime[j] = 0;
                }
                if (crossed.Count > 0)
                    steps.Add(
                        new AlgorithmStep
                        {
                            StepNumber = step++,
                            Array = (int[])isPrime.Clone(),
                            Description = $"Cross out multiples of {i}",
                            HighlightIndices = crossed.ToArray(),
                        }
                    );
            }
        }

        var primes = Enumerable.Range(2, n - 1).Where(i => isPrime[i] == 1).ToArray();
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])isPrime.Clone(),
                Description =
                    $"Found {primes.Length} primes: [{string.Join(", ", primes.Take(20))}{(primes.Length > 20 ? "..." : "")}]",
                SortedIndices = primes,
            }
        );
        return steps;
    }

    // 2. GCD (Euclidean Algorithm)
    // Time: O(log min(a, b))
    // Space: O(1)
    public List<AlgorithmStep> Gcd(int a, int b)
    {
        if (a <= 0 || b <= 0)
            throw new ArgumentException("Both numbers must be positive.");
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = [a, b],
                Description = $"GCD({a}, {b})",
            }
        );

        int x = a,
            y = b;
        while (y != 0)
        {
            int r = x % y;
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = [x, y, r],
                    Description = $"{x} mod {y} = {r}",
                    HighlightIndices = [2],
                }
            );
            x = y;
            y = r;
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = [x],
                Description = $"GCD({a}, {b}) = {x}",
                SortedIndices = [0],
            }
        );
        return steps;
    }
    // 6. Prime Factorization
    // Time: O(sqrt(n))
    // Space: O(log n)
    public List<AlgorithmStep> PrimeFactorization(int n)
    {
        if (n < 2)
            throw new ArgumentException("n must be at least 2.");
        var steps = new List<AlgorithmStep>();
        int step = 0;
        var factors = new List<int>();
        int remaining = n;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = [n],
                Description = $"Prime factorization of {n}",
            }
        );

        for (int d = 2; (long)d * d <= remaining; d++)
        {
            while (remaining % d == 0)
            {
                factors.Add(d);
                remaining /= d;
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = factors.ToArray(),
                        Description = $"Divide by {d}, remaining = {remaining}",
                        HighlightIndices = [factors.Count - 1],
                    }
                );
            }
        }
        if (remaining > 1)
        {
            factors.Add(remaining);
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = factors.ToArray(),
                    Description = $"Last factor: {remaining}",
                    HighlightIndices = [factors.Count - 1],
                }
            );
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = factors.ToArray(),
                Description = $"{n} = {string.Join(" × ", factors)}",
                SortedIndices = Enumerable.Range(0, factors.Count).ToArray(),
            }
        );
        return steps;
    }
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
                    $"n & (n-1) = {n & (n - 1)} → {(isPow2 ? "power of 2" : "not power of 2")}",
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

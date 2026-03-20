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

    // 3. Extended Euclidean
    // Time: O(log min(a, b))
    // Space: O(1)
    public List<AlgorithmStep> ExtendedGcd(int a, int b)
    {
        if (a <= 0 || b <= 0)
            throw new ArgumentException("Both numbers must be positive.");
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = [a, b, 1, 0, 0, 1],
                Description = $"Extended GCD({a}, {b}): finding x,y s.t. ax+by=gcd",
            }
        );

        int oldR = a,
            r = b,
            oldS = 1,
            s = 0,
            oldT = 0,
            t = 1;
        while (r != 0)
        {
            int q = oldR / r;
            (oldR, r) = (r, oldR - q * r);
            (oldS, s) = (s, oldS - q * s);
            (oldT, t) = (t, oldT - q * t);
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = [oldR, r, oldS, s, oldT, t],
                    Description = $"r={oldR}, s={oldS}, t={oldT} (q={q})",
                    HighlightIndices = [0, 2, 4],
                }
            );
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = [oldR, oldS, oldT],
                Description = $"GCD={oldR}, x={oldS}, y={oldT} → {a}×{oldS}+{b}×{oldT}={oldR}",
                SortedIndices = [0, 1, 2],
            }
        );
        return steps;
    }

    // 4. Fast Exponentiation
    // Time: O(log n)
    // Space: O(1)
    public List<AlgorithmStep> FastExponentiation(int baseVal, int exp, int mod)
    {
        if (mod <= 0)
            throw new ArgumentException("Modulus must be positive.");
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = [baseVal, exp, mod],
                Description = $"{baseVal}^{exp} mod {mod}",
            }
        );

        long result = 1,
            b = baseVal % mod;
        int e = exp;
        while (e > 0)
        {
            if ((e & 1) == 1)
            {
                result = result * b % mod;
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = [(int)result, (int)b, e],
                        Description = $"Odd exp: result = {result}, base = {b}, exp = {e}",
                        HighlightIndices = [0],
                    }
                );
            }
            e >>= 1;
            b = b * b % mod;
            if (e > 0)
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = [(int)result, (int)b, e],
                        Description = $"Square base: base = {b}, exp = {e}",
                        HighlightIndices = [1],
                    }
                );
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = [(int)result],
                Description = $"{baseVal}^{exp} mod {mod} = {result}",
                SortedIndices = [0],
            }
        );
        return steps;
    }

    // 5. Modular Arithmetic demo
    // Time: O(1)
    // Space: O(1)
    public List<AlgorithmStep> ModularArithmetic(int a, int b, int mod)
    {
        if (mod <= 0)
            throw new ArgumentException("Modulus must be positive.");
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = [a, b, mod],
                Description = $"Modular arithmetic: a={a}, b={b}, mod={mod}",
            }
        );

        int addResult = ((a % mod) + (b % mod)) % mod;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = [a % mod, b % mod, addResult],
                Description = $"({a}+{b}) mod {mod} = {addResult}",
                HighlightIndices = [2],
            }
        );

        int subResult = (((a % mod) - (b % mod)) % mod + mod) % mod;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = [a % mod, b % mod, subResult],
                Description = $"({a}-{b}) mod {mod} = {subResult}",
                HighlightIndices = [2],
            }
        );

        int mulResult = (int)((long)(a % mod) * (b % mod) % mod);
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = [a % mod, b % mod, mulResult],
                Description = $"({a}×{b}) mod {mod} = {mulResult}",
                HighlightIndices = [2],
            }
        );

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = [addResult, subResult, mulResult],
                Description = $"Results: add={addResult}, sub={subResult}, mul={mulResult}",
                SortedIndices = [0, 1, 2],
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

    // 7. Fibonacci via Matrix Exponentiation
    // Time: O(log n)
    // Space: O(1)
    public List<AlgorithmStep> FibonacciMatrix(int n)
    {
        if (n < 0)
            throw new ArgumentException("n must be non-negative.");
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = [0, 1],
                Description = $"Fibonacci({n}) via matrix exponentiation",
            }
        );

        if (n == 0)
        {
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step,
                    Array = [0],
                    Description = "F(0) = 0",
                    SortedIndices = [0],
                }
            );
            return steps;
        }

        long[,] result =
        {
            { 1, 0 },
            { 0, 1 },
        };
        long[,] mat =
        {
            { 1, 1 },
            { 1, 0 },
        };
        int e = n;

        while (e > 0)
        {
            if ((e & 1) == 1)
            {
                result = MatMul(result, mat);
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array =
                        [
                            (int)result[0, 0],
                            (int)result[0, 1],
                            (int)result[1, 0],
                            (int)result[1, 1],
                        ],
                        Description = $"Multiply (exp bit=1), e={e}",
                        HighlightIndices = [0],
                    }
                );
            }
            mat = MatMul(mat, mat);
            e >>= 1;
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = [(int)result[0, 1]],
                Description = $"F({n}) = {result[0, 1]}",
                SortedIndices = [0],
            }
        );
        return steps;
    }

    private static long[,] MatMul(long[,] a, long[,] b) =>
        new long[,]
        {
            { a[0, 0] * b[0, 0] + a[0, 1] * b[1, 0], a[0, 0] * b[0, 1] + a[0, 1] * b[1, 1] },
            { a[1, 0] * b[0, 0] + a[1, 1] * b[1, 0], a[1, 0] * b[0, 1] + a[1, 1] * b[1, 1] },
        };

    // 8. Chinese Remainder Theorem
    // Time: O(n · log M) where M = max modulus
    // Space: O(n)
    public List<AlgorithmStep> ChineseRemainder(int[] remainders, int[] moduli)
    {
        if (remainders.Length == 0 || remainders.Length != moduli.Length)
            throw new ArgumentException("Provide matching non-empty remainders and moduli arrays.");
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = remainders,
                Description =
                    $"CRT: remainders=[{string.Join(",", remainders)}], moduli=[{string.Join(",", moduli)}]",
            }
        );

        long M = 1;
        foreach (int m in moduli)
            M *= m;
        long x = 0;

        for (int i = 0; i < remainders.Length; i++)
        {
            long Mi = M / moduli[i];
            long yi = ModInverse(Mi, moduli[i]);
            x += remainders[i] * Mi * yi;
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = [(int)(x % M), (int)Mi, (int)yi],
                    Description =
                        $"r={remainders[i]}, m={moduli[i]}: M_i={Mi}, y_i={yi}, x={x % M}",
                    HighlightIndices = [0],
                }
            );
        }

        x = ((x % M) + M) % M;
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = [(int)x],
                Description = $"Solution: x = {x} (mod {M})",
                SortedIndices = [0],
            }
        );
        return steps;
    }

    private static long ModInverse(long a, long m)
    {
        long g = ExtGcd(a % m, m, out long x, out _);
        if (g != 1)
            return -1;
        return (x % m + m) % m;
    }

    private static long ExtGcd(long a, long b, out long x, out long y)
    {
        if (a == 0)
        {
            x = 0;
            y = 1;
            return b;
        }
        long g = ExtGcd(b % a, a, out long x1, out long y1);
        x = y1 - (b / a) * x1;
        y = x1;
        return g;
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

    // 10. Catalan Numbers
    // Time: O(n^2)
    // Space: O(n)
    public List<AlgorithmStep> CatalanNumbers(int n)
    {
        if (n < 0)
            throw new ArgumentException("n must be non-negative.");
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
                Description = $"Catalan numbers up to C({n})",
            }
        );

        for (int i = 2; i <= n; i++)
        {
            for (int j = 0; j < i; j++)
                dp[i] += dp[j] * dp[i - 1 - j];
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])dp.Clone(),
                    Description = $"C({i}) = {dp[i]}",
                    HighlightIndices = [i],
                }
            );
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])dp.Clone(),
                Description = $"C({n}) = {dp[n]}",
                SortedIndices = Enumerable.Range(0, n + 1).ToArray(),
            }
        );
        return steps;
    }
}

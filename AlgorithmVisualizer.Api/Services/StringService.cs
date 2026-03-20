using AlgorithmVisualizer.Api.Models;

namespace AlgorithmVisualizer.Api.Services;

public class StringService
{
    private static void ValidateText(string text)
    {
        if (string.IsNullOrEmpty(text))
            throw new ArgumentException("Provide a non-empty text string.");
    }

    private static int[] ToCharCodes(string s) => s.Select(c => (int)c).ToArray();

    // 1. Linear Search in String
    // Time: O(n)
    // Space: O(1)
    public List<AlgorithmStep> LinearSearch(string text, string pattern)
    {
        ValidateText(text);
        if (string.IsNullOrEmpty(pattern))
            throw new ArgumentException("Provide a search character.");
        char target = pattern[0];
        var codes = ToCharCodes(text);
        var steps = new List<AlgorithmStep>();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])codes.Clone(),
                Description = $"Searching for '{target}' in \"{text}\"",
            }
        );

        for (int i = 0; i < text.Length; i++)
        {
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])codes.Clone(),
                    Description = $"Checking index {i}: '{text[i]}'",
                    HighlightIndices = [i],
                }
            );
            if (text[i] == target)
            {
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step,
                        Array = (int[])codes.Clone(),
                        Description = $"Found '{target}' at index {i}",
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
                Array = (int[])codes.Clone(),
                Description = $"'{target}' not found",
            }
        );
        return steps;
    }

    // 2. KMP
    // Time: O(n + m) where n = text length, m = pattern length
    // Space: O(m)
    public List<AlgorithmStep> KMP(string text, string pattern)
    {
        ValidateText(text);
        ValidateText(pattern);
        var codes = ToCharCodes(text);
        var steps = new List<AlgorithmStep>();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])codes.Clone(),
                Description = $"KMP: searching for \"{pattern}\" in \"{text}\"",
            }
        );

        // Build failure function
        int[] lps = new int[pattern.Length];
        int len = 0,
            idx = 1;
        while (idx < pattern.Length)
        {
            if (pattern[idx] == pattern[len])
            {
                lps[idx++] = ++len;
            }
            else if (len > 0)
            {
                len = lps[len - 1];
            }
            else
            {
                lps[idx++] = 0;
            }
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = lps,
                Description = $"LPS (failure) table built for \"{pattern}\"",
            }
        );

        int i = 0,
            j = 0;
        var found = new List<int>();
        while (i < text.Length)
        {
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])codes.Clone(),
                    Description =
                        $"Comparing text[{i}]='{text[i]}' with pattern[{j}]='{pattern[j]}'",
                    HighlightIndices = [i],
                }
            );

            if (text[i] == pattern[j])
            {
                i++;
                j++;
            }
            else if (j > 0)
            {
                j = lps[j - 1];
            }
            else
            {
                i++;
            }

            if (j == pattern.Length)
            {
                int matchStart = i - j;
                found.Add(matchStart);
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])codes.Clone(),
                        Description = $"Pattern found at index {matchStart}",
                        SortedIndices = Enumerable.Range(matchStart, pattern.Length).ToArray(),
                    }
                );
                j = lps[j - 1];
            }
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])codes.Clone(),
                Description =
                    found.Count > 0
                        ? $"Found {found.Count} match(es) at: {string.Join(", ", found)}"
                        : "Pattern not found",
            }
        );
        return steps;
    }

    // 3. Boyer-Moore
    // Time: O(n/m) best, O(n · m) worst
    // Space: O(m + σ) where σ = alphabet size
    public List<AlgorithmStep> BoyerMoore(string text, string pattern)
    {
        ValidateText(text);
        ValidateText(pattern);
        var codes = ToCharCodes(text);
        var steps = new List<AlgorithmStep>();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])codes.Clone(),
                Description = $"Boyer-Moore: searching for \"{pattern}\" in \"{text}\"",
            }
        );

        var badChar = new Dictionary<char, int>();
        for (int k = 0; k < pattern.Length; k++)
            badChar[pattern[k]] = k;

        int s = 0;
        var found = new List<int>();
        while (s <= text.Length - pattern.Length)
        {
            int j = pattern.Length - 1;
            while (j >= 0 && pattern[j] == text[s + j])
            {
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])codes.Clone(),
                        Description = $"Match at text[{s + j}]='{text[s + j]}'",
                        HighlightIndices = [s + j],
                    }
                );
                j--;
            }

            if (j < 0)
            {
                found.Add(s);
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])codes.Clone(),
                        Description = $"Pattern found at index {s}",
                        SortedIndices = Enumerable.Range(s, pattern.Length).ToArray(),
                    }
                );
                s +=
                    (s + pattern.Length < text.Length)
                        ? pattern.Length
                            - (
                                badChar.ContainsKey(text[s + pattern.Length])
                                    ? badChar[text[s + pattern.Length]]
                                    : -1
                            )
                        : 1;
            }
            else
            {
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])codes.Clone(),
                        Description =
                            $"Mismatch at text[{s + j}]='{text[s + j]}', pattern[{j}]='{pattern[j]}'",
                        HighlightIndices = [s + j],
                    }
                );
                int shift = badChar.ContainsKey(text[s + j]) ? badChar[text[s + j]] : -1;
                s += Math.Max(1, j - shift);
            }
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])codes.Clone(),
                Description =
                    found.Count > 0
                        ? $"Found {found.Count} match(es) at: {string.Join(", ", found)}"
                        : "Pattern not found",
            }
        );
        return steps;
    }

    // 4. Rabin-Karp
    // Time: O(n + m) avg, O(n · m) worst
    // Space: O(1)
    public List<AlgorithmStep> RabinKarp(string text, string pattern)
    {
        ValidateText(text);
        ValidateText(pattern);
        var codes = ToCharCodes(text);
        var steps = new List<AlgorithmStep>();
        int step = 0;
        int m = pattern.Length,
            n = text.Length;
        const int d = 256;
        const long q = 101;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])codes.Clone(),
                Description = $"Rabin-Karp: searching for \"{pattern}\" in \"{text}\"",
            }
        );

        if (m > n)
        {
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step,
                    Array = (int[])codes.Clone(),
                    Description = "Pattern longer than text",
                }
            );
            return steps;
        }

        long h = 1;
        for (int k = 0; k < m - 1; k++)
            h = (h * d) % q;

        long pHash = 0,
            tHash = 0;
        for (int k = 0; k < m; k++)
        {
            pHash = (d * pHash + pattern[k]) % q;
            tHash = (d * tHash + text[k]) % q;
        }

        var found = new List<int>();
        for (int i = 0; i <= n - m; i++)
        {
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])codes.Clone(),
                    Description = $"Window [{i}..{i + m - 1}] hash={tHash}, pattern hash={pHash}",
                    HighlightIndices = Enumerable.Range(i, m).ToArray(),
                }
            );

            if (pHash == tHash && text.Substring(i, m) == pattern)
            {
                found.Add(i);
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])codes.Clone(),
                        Description = $"Pattern found at index {i}",
                        SortedIndices = Enumerable.Range(i, m).ToArray(),
                    }
                );
            }

            if (i < n - m)
            {
                tHash = (d * (tHash - text[i] * h) + text[i + m]) % q;
                if (tHash < 0)
                    tHash += q;
            }
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])codes.Clone(),
                Description =
                    found.Count > 0 ? $"Found {found.Count} match(es)" : "Pattern not found",
            }
        );
        return steps;
    }

    // 5. Longest Common Subsequence
    // Time: O(m · n)
    // Space: O(m · n)
    public List<AlgorithmStep> LongestCommonSubsequence(string text1, string text2)
    {
        ValidateText(text1);
        ValidateText(text2);
        int m = text1.Length,
            n = text2.Length;
        var dp = new int[m + 1, n + 1];
        var steps = new List<AlgorithmStep>();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = new int[n + 1],
                Description = $"LCS of \"{text1}\" and \"{text2}\"",
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
            }
            var row = new int[n + 1];
            for (int j = 0; j <= n; j++)
                row[j] = dp[i, j];
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = row,
                    Description = $"Row {i} (char '{text1[i - 1]}'): [{string.Join(",", row)}]",
                    HighlightIndices = [i],
                }
            );
        }

        // Backtrack to find the LCS string
        var lcs = new List<char>();
        int x = m,
            y = n;
        while (x > 0 && y > 0)
        {
            if (text1[x - 1] == text2[y - 1])
            {
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
            }
        );
        return steps;
    }

    // 6. Longest Palindromic Substring
    // Time: O(n^2)
    // Space: O(1)
    public List<AlgorithmStep> LongestPalindrome(string text)
    {
        ValidateText(text);
        var codes = ToCharCodes(text);
        var steps = new List<AlgorithmStep>();
        int step = 0;
        int start = 0,
            maxLen = 1;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])codes.Clone(),
                Description = $"Finding longest palindromic substring in \"{text}\"",
            }
        );

        for (int center = 0; center < text.Length; center++)
        {
            // Odd length
            int lo = center,
                hi = center;
            while (lo >= 0 && hi < text.Length && text[lo] == text[hi])
            {
                if (hi - lo + 1 > maxLen)
                {
                    maxLen = hi - lo + 1;
                    start = lo;
                    steps.Add(
                        new AlgorithmStep
                        {
                            StepNumber = step++,
                            Array = (int[])codes.Clone(),
                            Description =
                                $"Palindrome \"{text.Substring(lo, hi - lo + 1)}\" at [{lo}..{hi}]",
                            HighlightIndices = Enumerable.Range(lo, hi - lo + 1).ToArray(),
                        }
                    );
                }
                lo--;
                hi++;
            }
            // Even length
            lo = center;
            hi = center + 1;
            while (lo >= 0 && hi < text.Length && text[lo] == text[hi])
            {
                if (hi - lo + 1 > maxLen)
                {
                    maxLen = hi - lo + 1;
                    start = lo;
                    steps.Add(
                        new AlgorithmStep
                        {
                            StepNumber = step++,
                            Array = (int[])codes.Clone(),
                            Description =
                                $"Palindrome \"{text.Substring(lo, hi - lo + 1)}\" at [{lo}..{hi}]",
                            HighlightIndices = Enumerable.Range(lo, hi - lo + 1).ToArray(),
                        }
                    );
                }
                lo--;
                hi++;
            }
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])codes.Clone(),
                Description =
                    $"Longest palindrome: \"{text.Substring(start, maxLen)}\" (length {maxLen})",
                SortedIndices = Enumerable.Range(start, maxLen).ToArray(),
            }
        );
        return steps;
    }

    // 7. Anagram Detection
    // Time: O(n)
    // Space: O(1)
    public List<AlgorithmStep> AnagramDetection(string text, string pattern)
    {
        ValidateText(text);
        ValidateText(pattern);
        var codes = ToCharCodes(text);
        var steps = new List<AlgorithmStep>();
        int step = 0;
        int m = pattern.Length,
            n = text.Length;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])codes.Clone(),
                Description = $"Finding anagrams of \"{pattern}\" in \"{text}\"",
            }
        );

        if (m > n)
        {
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step,
                    Array = (int[])codes.Clone(),
                    Description = "Pattern longer than text — no anagrams",
                }
            );
            return steps;
        }

        var pCount = new int[256];
        var wCount = new int[256];
        foreach (char c in pattern)
            pCount[c]++;
        var found = new List<int>();

        for (int i = 0; i < n; i++)
        {
            wCount[text[i]]++;
            if (i >= m)
                wCount[text[i - m]]--;

            if (i >= m - 1)
            {
                bool match = pCount.SequenceEqual(wCount);
                int ws = i - m + 1;
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])codes.Clone(),
                        Description = match
                            ? $"Anagram found at index {ws}: \"{text.Substring(ws, m)}\""
                            : $"Window [{ws}..{i}]: \"{text.Substring(ws, m)}\" — not anagram",
                        HighlightIndices = Enumerable.Range(ws, m).ToArray(),
                        SortedIndices = match ? Enumerable.Range(ws, m).ToArray() : [],
                    }
                );
                if (match)
                    found.Add(ws);
            }
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])codes.Clone(),
                Description =
                    found.Count > 0
                        ? $"Found {found.Count} anagram(s) at: {string.Join(", ", found)}"
                        : "No anagrams found",
            }
        );
        return steps;
    }

    // 8. String Reversal
    // Time: O(n)
    // Space: O(n)
    public List<AlgorithmStep> Reversal(string text)
    {
        ValidateText(text);
        var arr = ToCharCodes(text);
        var steps = new List<AlgorithmStep>();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])arr.Clone(),
                Description = $"Reversing \"{text}\"",
            }
        );

        int lo = 0,
            hi = arr.Length - 1;
        while (lo < hi)
        {
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])arr.Clone(),
                    Description =
                        $"Swapping index {lo} ('{(char)arr[lo]}') and {hi} ('{(char)arr[hi]}')",
                    HighlightIndices = [lo, hi],
                }
            );
            (arr[lo], arr[hi]) = (arr[hi], arr[lo]);
            lo++;
            hi--;
        }

        var reversed = new string(arr.Select(c => (char)c).ToArray());
        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])arr.Clone(),
                Description = $"Reversed: \"{reversed}\"",
                SortedIndices = Enumerable.Range(0, arr.Length).ToArray(),
            }
        );
        return steps;
    }

    // 9. Run-Length Encoding
    // Time: O(n)
    // Space: O(n)
    public List<AlgorithmStep> RunLengthEncoding(string text)
    {
        ValidateText(text);
        var codes = ToCharCodes(text);
        var steps = new List<AlgorithmStep>();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])codes.Clone(),
                Description = $"Run-Length Encoding of \"{text}\"",
            }
        );

        var result = new System.Text.StringBuilder();
        int i = 0;
        while (i < text.Length)
        {
            char c = text[i];
            int count = 0;
            int start = i;
            while (i < text.Length && text[i] == c)
            {
                count++;
                i++;
            }

            result.Append(c).Append(count);
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])codes.Clone(),
                    Description = $"Run: '{c}' × {count} at [{start}..{i - 1}]",
                    HighlightIndices = Enumerable.Range(start, count).ToArray(),
                }
            );
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step,
                Array = (int[])codes.Clone(),
                Description = $"Encoded: \"{result}\"",
                SortedIndices = Enumerable.Range(0, codes.Length).ToArray(),
            }
        );
        return steps;
    }

    // 10. Levenshtein Distance (Edit Distance)
    // Time: O(m · n)
    // Space: O(m · n)
    public List<AlgorithmStep> Levenshtein(string text1, string text2)
    {
        ValidateText(text1);
        ValidateText(text2);
        int m = text1.Length,
            n = text2.Length;
        var dp = new int[m + 1, n + 1];
        var steps = new List<AlgorithmStep>();
        int step = 0;

        for (int j = 0; j <= n; j++)
            dp[0, j] = j;
        for (int i2 = 0; i2 <= m; i2++)
            dp[i2, 0] = i2;

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

        for (int i2 = 1; i2 <= m; i2++)
        {
            for (int j = 1; j <= n; j++)
            {
                int cost = text1[i2 - 1] == text2[j - 1] ? 0 : 1;
                dp[i2, j] = Math.Min(
                    Math.Min(dp[i2 - 1, j] + 1, dp[i2, j - 1] + 1),
                    dp[i2 - 1, j - 1] + cost
                );
            }
            var row = new int[n + 1];
            for (int j = 0; j <= n; j++)
                row[j] = dp[i2, j];
            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = row,
                    Description = $"Row {i2} ('{text1[i2 - 1]}'): [{string.Join(",", row)}]",
                    HighlightIndices = [i2],
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
}

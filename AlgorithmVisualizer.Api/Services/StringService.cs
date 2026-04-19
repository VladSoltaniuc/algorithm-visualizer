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
            // TODO: Error shows code in the frontend
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
        var patternCodes = ToCharCodes(pattern);
        var steps = new List<AlgorithmStep>();
        int step = 0;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])codes.Clone(),
                Description = $"KMP: searching for \"{pattern}\" in \"{text}\"",
                PatternArray = patternCodes,
                PatternOffset = 0,
            }
        );

        int[] lps = new int[pattern.Length];
        int len = 0,
            idx = 1;
        while (idx < pattern.Length)
        {
            if (pattern[idx] == pattern[len])
                lps[idx++] = ++len;
            else if (len > 0)
                len = lps[len - 1];
            else
                lps[idx++] = 0;
        }

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])codes.Clone(),
                Description = $"LPS table built for \"{pattern}\"",
                PatternArray = patternCodes,
                PatternOffset = 0,
                PArray = (int[])lps.Clone(),
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
                    PArray = (int[])lps.Clone(),
                    Description =
                        $"Comparing text[{i}]='{text[i]}' with pattern[{j}]='{pattern[j]}'",
                    HighlightIndices = [i],
                    PatternArray = patternCodes,
                    PatternOffset = i - j,
                    PatternHighlightIndex = j,
                }
            );

            if (text[i] == pattern[j])
            {
                i++;
                j++;
            }
            else if (j > 0)
            {
                int oldOffset = i - j;
                j = lps[j - 1];
                int newOffset = i - j;
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])codes.Clone(),
                        Description =
                            $"Mismatch! LPS shifts pattern from position {oldOffset} to {newOffset}",
                        HighlightIndices = [i],
                        PatternArray = patternCodes,
                        PatternOffset = newOffset,
                        PatternHighlightIndex = j,
                        PArray = (int[])lps.Clone(),
                    }
                );
            }
            else
                i++;

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
                        PatternArray = patternCodes,
                        PatternOffset = matchStart,
                        PArray = (int[])lps.Clone(),
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
    // Time: O(n/m) best, O(n*m) worst
    // Space: O(m + alphabet_size)
    public List<AlgorithmStep> BoyerMoore(string text, string pattern)
    {
        ValidateText(text);
        ValidateText(pattern);
        var codes = ToCharCodes(text);
        var patternCodes = ToCharCodes(pattern);
        var steps = new List<AlgorithmStep>();
        int step = 0;
        int m = pattern.Length;

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])codes.Clone(),
                Description = $"Boyer-Moore: searching for \"{pattern}\" in \"{text}\"",
                PatternArray = patternCodes,
                PatternOffset = 0,
            }
        );

        // Bad character rule: last occurrence of each character in the pattern
        var badChar = new Dictionary<char, int>();
        for (int k = 0; k < m; k++)
            badChar[pattern[k]] = k;

        // Good suffix rule
        int[] goodSuffix = BuildGoodSuffixTable(pattern);

        int s = 0;
        var found = new List<int>();
        while (s <= text.Length - m)
        {
            int j = m - 1;
            while (j >= 0 && pattern[j] == text[s + j])
            {
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])codes.Clone(),
                        Description = $"Match at text[{s + j}]='{text[s + j]}'",
                        HighlightIndices = [s + j],
                        PatternArray = patternCodes,
                        PatternOffset = s,
                        PatternHighlightIndex = j,
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
                        SortedIndices = Enumerable.Range(s, m).ToArray(),
                        PatternArray = patternCodes,
                        PatternOffset = s,
                    }
                );
                s += Math.Max(1, goodSuffix[0]);
            }
            else
            {
                int bcIdx = badChar.TryGetValue(text[s + j], out int idx) ? idx : -1;
                int badCharShift = j - bcIdx;
                int goodSufShift = goodSuffix[j + 1];
                int shift = Math.Max(1, Math.Max(badCharShift, goodSufShift));
                string rule = goodSufShift > badCharShift ? "good-suffix" : "bad-character";
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])codes.Clone(),
                        Description =
                            $"Mismatch at text[{s + j}]='{text[s + j]}', pattern[{j}]='{pattern[j]}' - shift {shift} ({rule} rule)",
                        HighlightIndices = [s + j],
                        PatternArray = patternCodes,
                        PatternOffset = s,
                        PatternHighlightIndex = j,
                    }
                );
                s += shift;
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

    // Preprocesses the good-suffix shift table using Gusfield's border method.
    // shift[k] = shift when k characters from the right have matched.
    private static int[] BuildGoodSuffixTable(string pattern)
    {
        int m = pattern.Length;
        int[] shift = new int[m + 1];
        int[] border = new int[m + 1];

        // Phase 1: suffix occurs again in pattern preceded by a different char
        int i = m,
            j = m + 1;
        border[i] = j;
        while (i > 0)
        {
            while (j <= m && pattern[i - 1] != pattern[j - 1])
            {
                if (shift[j] == 0)
                    shift[j] = j - i;
                j = border[j];
            }
            border[--i] = --j;
        }

        // Phase 2: suffix of matched portion is a prefix of the pattern
        j = border[0];
        for (i = 0; i <= m; i++)
        {
            if (shift[i] == 0)
                shift[i] = j;
            if (i == j)
                j = border[j];
        }

        return shift;
    }

    // 4. Rabin-Karp
    // Time: O(n + m) avg, O(n · m) worst
    // Space: O(1)
    public List<AlgorithmStep> RabinKarp(string text, string pattern)
    {
        ValidateText(text);
        ValidateText(pattern);
        var codes = ToCharCodes(text);
        var patternCodes = ToCharCodes(pattern);
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
                Description =
                    $"Rabin-Karp: searching for \"{pattern}\" in \"{text}\" (base={d}, mod={q})",
                PatternArray = patternCodes,
                PatternOffset = 0,
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

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = step++,
                Array = (int[])codes.Clone(),
                Description =
                    $"Initial hashes computed — pattern hash={pHash}, window[0..{m - 1}] hash={tHash}",
                HighlightIndices = Enumerable.Range(0, m).ToArray(),
                PatternArray = patternCodes,
                PatternOffset = 0,
                TextHash = tHash,
                PatternHash = pHash,
            }
        );

        var found = new List<int>();
        for (int i = 0; i <= n - m; i++)
        {
            bool hashMatch = pHash == tHash;
            string window = text.Substring(i, m);

            steps.Add(
                new AlgorithmStep
                {
                    StepNumber = step++,
                    Array = (int[])codes.Clone(),
                    Description = hashMatch
                        ? $"Window \"{window}\" [{i}..{i + m - 1}]: hash={tHash} == pattern hash={pHash} — hashes match, verifying characters…"
                        : $"Window \"{window}\" [{i}..{i + m - 1}]: hash={tHash} ≠ pattern hash={pHash} — skip",
                    HighlightIndices = Enumerable.Range(i, m).ToArray(),
                    PatternArray = patternCodes,
                    PatternOffset = i,
                    TextHash = tHash,
                    PatternHash = pHash,
                }
            );

            if (hashMatch)
            {
                if (window == pattern)
                {
                    found.Add(i);
                    steps.Add(
                        new AlgorithmStep
                        {
                            StepNumber = step++,
                            Array = (int[])codes.Clone(),
                            Description = $"Characters match! Pattern found at index {i}",
                            SortedIndices = Enumerable.Range(i, m).ToArray(),
                            PatternArray = patternCodes,
                            PatternOffset = i,
                            TextHash = tHash,
                            PatternHash = pHash,
                        }
                    );
                }
                else
                {
                    steps.Add(
                        new AlgorithmStep
                        {
                            StepNumber = step++,
                            Array = (int[])codes.Clone(),
                            Description =
                                $"Hash collision! \"{window}\" ≠ \"{pattern}\" — spurious hit",
                            HighlightIndices = Enumerable.Range(i, m).ToArray(),
                            PatternArray = patternCodes,
                            PatternOffset = i,
                            TextHash = tHash,
                            PatternHash = pHash,
                        }
                    );
                }
            }

            if (i < n - m)
            {
                long oldHash = tHash;
                tHash = (d * (tHash - text[i] * h) + text[i + m]) % q;
                if (tHash < 0)
                    tHash += q;
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = step++,
                        Array = (int[])codes.Clone(),
                        Description =
                            $"Rolling hash: remove '{text[i]}', add '{text[i + m]}' → new hash={tHash}",
                        HighlightIndices = Enumerable.Range(i + 1, m).ToArray(),
                        PatternArray = patternCodes,
                        PatternOffset = i + 1,
                        TextHash = tHash,
                        PatternHash = pHash,
                    }
                );
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

    // 5. Longest Common Subsequence
    // Time: O(m · n)
    // Space: O(m · n)
    public List<AlgorithmStep> LongestCommonSubsequence(string text1, string text2)
    {
        ValidateText(text1);
        ValidateText(text2);
        // Longest string on top (columns), shortest on the side (rows)
        if (text1.Length > text2.Length)
            (text1, text2) = (text2, text1);
        int m = text1.Length,
            n = text2.Length;
        var dp = new int[m + 1, n + 1];
        var steps = new List<AlgorithmStep>();
        int step = 0;

        string rowHeaders = " " + text1; // leading space for row 0
        string colHeaders = " " + text2; // leading space for col 0

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
                        : $"'{text1[i - 1]}' \u2260 '{text2[j - 1]}': dp[{i}][{j}] = max({dp[i - 1, j]}, {dp[i, j - 1]}) = {dp[i, j]}";
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
        var backtrackCells = new List<int>(); // flat pairs [r0,c0, r1,c1, ...]
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

    // 6. Longest Palindromic Substring (Manacher’s Algorithm)
    // Time: O(n)
    // Space: O(n)
    public List<AlgorithmStep> LongestPalindrome(string text)
    {
        ValidateText(text);
        var steps = new List<AlgorithmStep>();
        int stepNum = 0;

        // Build transformed string: #c0#c1#...#cn-1#
        int tLen = 2 * text.Length + 1;
        var T = new char[tLen];
        for (int i = 0; i < tLen; i++)
            T[i] = i % 2 == 0 ? '#' : text[i / 2];

        int[] tCodes = T.Select(c => (int)c).ToArray();
        int[] P = new int[tLen];
        int C = 0,
            R = 0;

        for (int i = 0; i < tLen; i++)
        {
            int mirror = 2 * C - i;
            bool insidePalindrome = i < R && mirror >= 0;
            int distToBorder = R - i; // how far i is from the right boundary

            string rule;
            string outcome;

            if (insidePalindrome && P[mirror] < distToBorder)
            {
                // Rule 1: mirror fits entirely, not touching border
                P[i] = P[mirror];
                rule = "R1";
                outcome =
                    $"mirror P[{mirror}]={P[mirror]} < distToBorder={distToBorder} → copy P[{i}]={P[i]}";
            }
            else if (insidePalindrome && P[mirror] == distToBorder)
            {
                // Rule 2: mirror reaches exactly to border
                P[i] = P[mirror];
                rule = "R2";
                outcome =
                    $"mirror P[{mirror}]={P[mirror]} = distToBorder={distToBorder} → start at {P[i]}, explore";
                // expand below
                while (
                    i - P[i] - 1 >= 0 && i + P[i] + 1 < tLen && T[i - P[i] - 1] == T[i + P[i] + 1]
                )
                    P[i]++;
                outcome += $" → P[{i}]={P[i]}";
            }
            else if (insidePalindrome && P[mirror] > distToBorder)
            {
                // Rule 3: mirror extends beyond larger palindrome
                P[i] = distToBorder;
                rule = "R3";
                outcome =
                    $"mirror P[{mirror}]={P[mirror]} > distToBorder={distToBorder} → start at {P[i]}, explore";
                // expand below
                while (
                    i - P[i] - 1 >= 0 && i + P[i] + 1 < tLen && T[i - P[i] - 1] == T[i + P[i] + 1]
                )
                    P[i]++;
                outcome += $" → P[{i}]={P[i]}";
            }
            else
            {
                // Rule 4: outside known palindrome
                P[i] = 0;
                rule = "R4";
                outcome = "explore from scratch";
                while (
                    i - P[i] - 1 >= 0 && i + P[i] + 1 < tLen && T[i - P[i] - 1] == T[i + P[i] + 1]
                )
                    P[i]++;
                outcome += $" → P[{i}]={P[i]}";
            }

            // Update rightmost palindrome boundary
            bool updatedCR = i + P[i] > R;
            if (updatedCR)
            {
                C = i;
                R = i + P[i];
            }

            // Build description
            string desc = $"i={i} \'{T[i]}\': {rule} — {outcome}";
            if (updatedCR)
                desc += $"  [C→{C}, R→{R}]";

            // Emit step: show palindrome span for non-trivial (P[i] >= 2)
            // or show rule application for mirror-optimised steps
            if (P[i] >= 2)
            {
                int origStart = (i - P[i]) / 2;
                string pal = text.Substring(origStart, P[i]);
                desc += $"  \"{pal}\"";

                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = stepNum++,
                        Array = (int[])tCodes.Clone(),
                        PArray = (int[])P.Clone(),
                        ManacherCenter = C,
                        ManacherRight = R,
                        HighlightIndices = Enumerable.Range(i - P[i], 2 * P[i] + 1).ToArray(),
                        Description = desc,
                    }
                );
            }
            else if (rule != "R4" || P[i] > 0)
            {
                steps.Add(
                    new AlgorithmStep
                    {
                        StepNumber = stepNum++,
                        Array = (int[])tCodes.Clone(),
                        PArray = (int[])P.Clone(),
                        ManacherCenter = C,
                        ManacherRight = R,
                        HighlightIndices =
                            P[i] > 0
                                ? Enumerable.Range(i - P[i], 2 * P[i] + 1).ToArray()
                                : new[] { i },
                        Description = desc,
                    }
                );
            }
        }

        // Find the longest palindrome
        int maxRadius = 0,
            maxCenter = 0;
        for (int i = 0; i < tLen; i++)
        {
            if (P[i] > maxRadius)
            {
                maxRadius = P[i];
                maxCenter = i;
            }
        }

        int resStart = (maxCenter - maxRadius) / 2;
        int resLen = maxRadius;
        var origCodes = ToCharCodes(text);

        steps.Add(
            new AlgorithmStep
            {
                StepNumber = stepNum,
                Array = (int[])origCodes.Clone(),
                Description =
                    $"Longest palindrome: \"{text.Substring(resStart, resLen)}\" (length {resLen})",
                SortedIndices = Enumerable.Range(resStart, resLen).ToArray(),
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
}

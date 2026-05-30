using api.Helpers;
using api.Models;

namespace api.Services;

public class PatternService
{
    public List<AlgorithmStep> LinearSearch(string text, string pattern)
    {
        PatternHelpers.ValidateText(text);
        if (string.IsNullOrEmpty(pattern)) throw new ArgumentException("Provide a search character.");
        char target = pattern[0];
        var codes = PatternHelpers.ToCharCodes(text);
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])codes.Clone(), Description = $"Searching for '{target}' in \"{text}\"" });
        for (int i = 0; i < text.Length; i++)
        {
            steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])codes.Clone(), Description = $"Checking index {i}: '{text[i]}'", HighlightIndices = [i] });
            if (text[i] == target) { steps.Add(new AlgorithmStep { StepNumber = step, Array = (int[])codes.Clone(), Description = $"Found '{target}' at index {i}", SortedIndices = [i] }); return steps; }
        }
        steps.Add(new AlgorithmStep { StepNumber = step, Array = (int[])codes.Clone(), Description = $"'{target}' not found" });
        return steps;
    }

    public List<AlgorithmStep> KMP(string text, string pattern)
    {
        PatternHelpers.ValidateText(text);
        PatternHelpers.ValidateText(pattern);
        var codes = PatternHelpers.ToCharCodes(text);
        var patternCodes = PatternHelpers.ToCharCodes(pattern);
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])codes.Clone(), Description = $"KMP: searching for \"{pattern}\" in \"{text}\"", PatternArray = patternCodes, PatternOffset = 0 });

        int[] lps = new int[pattern.Length];
        int len = 0, idx = 1;
        while (idx < pattern.Length)
        {
            if (pattern[idx] == pattern[len]) lps[idx++] = ++len;
            else if (len > 0) len = lps[len - 1];
            else lps[idx++] = 0;
        }

        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])codes.Clone(), Description = $"LPS table built for \"{pattern}\"", PatternArray = patternCodes, PatternOffset = 0, PArray = (int[])lps.Clone() });

        int i = 0, j = 0;
        var found = new List<int>();
        while (i < text.Length)
        {
            steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])codes.Clone(), PArray = (int[])lps.Clone(), Description = $"Comparing text[{i}]='{text[i]}' with pattern[{j}]='{pattern[j]}'", HighlightIndices = [i], PatternArray = patternCodes, PatternOffset = i - j, PatternHighlightIndex = j });
            if (text[i] == pattern[j]) { i++; j++; }
            else if (j > 0)
            {
                int oldOffset = i - j; j = lps[j - 1]; int newOffset = i - j;
                steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])codes.Clone(), Description = $"Mismatch! LPS shifts pattern from position {oldOffset} to {newOffset}", HighlightIndices = [i], PatternArray = patternCodes, PatternOffset = newOffset, PatternHighlightIndex = j, PArray = (int[])lps.Clone() });
            }
            else i++;

            if (j == pattern.Length)
            {
                int matchStart = i - j; found.Add(matchStart);
                steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])codes.Clone(), Description = $"Pattern found at index {matchStart}", SortedIndices = Enumerable.Range(matchStart, pattern.Length).ToArray(), PatternArray = patternCodes, PatternOffset = matchStart, PArray = (int[])lps.Clone() });
                j = lps[j - 1];
            }
        }

        steps.Add(new AlgorithmStep { StepNumber = step, Array = (int[])codes.Clone(), Description = found.Count > 0 ? $"Found {found.Count} match(es) at: {string.Join(", ", found)}" : "Pattern not found" });
        return steps;
    }

    public List<AlgorithmStep> BoyerMoore(string text, string pattern)
    {
        PatternHelpers.ValidateText(text);
        PatternHelpers.ValidateText(pattern);
        var codes = PatternHelpers.ToCharCodes(text);
        var patternCodes = PatternHelpers.ToCharCodes(pattern);
        var steps = new List<AlgorithmStep>();
        int step = 0, m = pattern.Length;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])codes.Clone(), Description = $"Boyer-Moore: searching for \"{pattern}\" in \"{text}\"", PatternArray = patternCodes, PatternOffset = 0 });

        var badChar = new Dictionary<char, int>();
        for (int k = 0; k < m; k++) badChar[pattern[k]] = k;
        int[] goodSuffix = PatternHelpers.BuildGoodSuffixTable(pattern);

        int s = 0; var found = new List<int>();
        while (s <= text.Length - m)
        {
            int jj = m - 1;
            while (jj >= 0 && pattern[jj] == text[s + jj])
            {
                steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])codes.Clone(), Description = $"Match at text[{s + jj}]='{text[s + jj]}'", HighlightIndices = [s + jj], PatternArray = patternCodes, PatternOffset = s, PatternHighlightIndex = jj });
                jj--;
            }
            if (jj < 0)
            {
                found.Add(s);
                steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])codes.Clone(), Description = $"Pattern found at index {s}", SortedIndices = Enumerable.Range(s, m).ToArray(), PatternArray = patternCodes, PatternOffset = s });
                s += Math.Max(1, goodSuffix[0]);
            }
            else
            {
                int bcIdx = badChar.TryGetValue(text[s + jj], out int bci) ? bci : -1;
                int badCharShift = jj - bcIdx, goodSufShift = goodSuffix[jj + 1];
                int shift = Math.Max(1, Math.Max(badCharShift, goodSufShift));
                string rule = goodSufShift > badCharShift ? "good-suffix" : "bad-character";
                steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])codes.Clone(), Description = $"Mismatch at text[{s + jj}]='{text[s + jj]}', pattern[{jj}]='{pattern[jj]}' - shift {shift} ({rule} rule)", HighlightIndices = [s + jj], PatternArray = patternCodes, PatternOffset = s, PatternHighlightIndex = jj });
                s += shift;
            }
        }

        steps.Add(new AlgorithmStep { StepNumber = step, Array = (int[])codes.Clone(), Description = found.Count > 0 ? $"Found {found.Count} match(es) at: {string.Join(", ", found)}" : "Pattern not found" });
        return steps;
    }

    public List<AlgorithmStep> RabinKarp(string text, string pattern)
    {
        PatternHelpers.ValidateText(text);
        PatternHelpers.ValidateText(pattern);
        var codes = PatternHelpers.ToCharCodes(text);
        var patternCodes = PatternHelpers.ToCharCodes(pattern);
        var steps = new List<AlgorithmStep>();
        int step = 0, m = pattern.Length, n = text.Length;
        const int d = 256; const long q = 101;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])codes.Clone(), Description = $"Rabin-Karp: searching for \"{pattern}\" in \"{text}\" (base={d}, mod={q})", PatternArray = patternCodes, PatternOffset = 0 });

        if (m > n) { steps.Add(new AlgorithmStep { StepNumber = step, Array = (int[])codes.Clone(), Description = "Pattern longer than text" }); return steps; }

        long h = 1;
        for (int k = 0; k < m - 1; k++) h = (h * d) % q;
        long pHash = 0, tHash = 0;
        for (int k = 0; k < m; k++) { pHash = (d * pHash + pattern[k]) % q; tHash = (d * tHash + text[k]) % q; }

        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])codes.Clone(), Description = $"Initial hashes — pattern hash={pHash}, window[0..{m - 1}] hash={tHash}", HighlightIndices = Enumerable.Range(0, m).ToArray(), PatternArray = patternCodes, PatternOffset = 0, TextHash = tHash, PatternHash = pHash });

        var found = new List<int>();
        for (int i = 0; i <= n - m; i++)
        {
            bool hashMatch = pHash == tHash;
            string window = text.Substring(i, m);
            steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])codes.Clone(), Description = hashMatch ? $"Window \"{window}\" [{i}..{i + m - 1}]: hash={tHash} == pattern hash={pHash} — verifying…" : $"Window \"{window}\" [{i}..{i + m - 1}]: hash={tHash} ≠ pattern hash={pHash} — skip", HighlightIndices = Enumerable.Range(i, m).ToArray(), PatternArray = patternCodes, PatternOffset = i, TextHash = tHash, PatternHash = pHash });

            if (hashMatch)
            {
                if (window == pattern) { found.Add(i); steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])codes.Clone(), Description = $"Characters match! Pattern found at index {i}", SortedIndices = Enumerable.Range(i, m).ToArray(), PatternArray = patternCodes, PatternOffset = i, TextHash = tHash, PatternHash = pHash }); }
                else steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])codes.Clone(), Description = $"Hash collision! \"{window}\" ≠ \"{pattern}\" — spurious hit", HighlightIndices = Enumerable.Range(i, m).ToArray(), PatternArray = patternCodes, PatternOffset = i, TextHash = tHash, PatternHash = pHash });
            }

            if (i < n - m)
            {
                long oldHash = tHash;
                tHash = (d * (tHash - text[i] * h) + text[i + m]) % q;
                if (tHash < 0) tHash += q;
                steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])codes.Clone(), Description = $"Rolling hash: remove '{text[i]}', add '{text[i + m]}' → new hash={tHash}", HighlightIndices = Enumerable.Range(i + 1, m).ToArray(), PatternArray = patternCodes, PatternOffset = i + 1, TextHash = tHash, PatternHash = pHash });
            }
        }

        steps.Add(new AlgorithmStep { StepNumber = step, Array = (int[])codes.Clone(), Description = found.Count > 0 ? $"Found {found.Count} match(es) at: {string.Join(", ", found)}" : "Pattern not found" });
        return steps;
    }

    public List<AlgorithmStep> LongestPalindrome(string text)
    {
        PatternHelpers.ValidateText(text);
        var steps = new List<AlgorithmStep>();
        int stepNum = 0;

        int tLen = 2 * text.Length + 1;
        var T = new char[tLen];
        for (int i = 0; i < tLen; i++) T[i] = i % 2 == 0 ? '#' : text[i / 2];

        int[] tCodes = T.Select(c => (int)c).ToArray();
        int[] P = new int[tLen];
        int C = 0, R = 0;

        for (int i = 0; i < tLen; i++)
        {
            int mirror = 2 * C - i;
            bool inside = i < R && mirror >= 0;
            int dist = R - i;
            string rule, outcome;

            if (inside && P[mirror] < dist) { P[i] = P[mirror]; rule = "R1"; outcome = $"mirror P[{mirror}]={P[mirror]} < dist={dist} → copy P[{i}]={P[i]}"; }
            else if (inside && P[mirror] == dist) { P[i] = P[mirror]; rule = "R2"; outcome = $"mirror P[{mirror}]={P[mirror]} = dist={dist} → start at {P[i]}, explore"; while (i - P[i] - 1 >= 0 && i + P[i] + 1 < tLen && T[i - P[i] - 1] == T[i + P[i] + 1]) P[i]++; outcome += $" → P[{i}]={P[i]}"; }
            else if (inside && P[mirror] > dist) { P[i] = dist; rule = "R3"; outcome = $"mirror P[{mirror}]={P[mirror]} > dist={dist} → start at {P[i]}, explore"; while (i - P[i] - 1 >= 0 && i + P[i] + 1 < tLen && T[i - P[i] - 1] == T[i + P[i] + 1]) P[i]++; outcome += $" → P[{i}]={P[i]}"; }
            else { P[i] = 0; rule = "R4"; outcome = "explore from scratch"; while (i - P[i] - 1 >= 0 && i + P[i] + 1 < tLen && T[i - P[i] - 1] == T[i + P[i] + 1]) P[i]++; outcome += $" → P[{i}]={P[i]}"; }

            bool updatedCR = i + P[i] > R;
            if (updatedCR) { C = i; R = i + P[i]; }

            string desc = $"i={i} '{T[i]}': {rule} — {outcome}";
            if (updatedCR) desc += $"  [C←{C}, R←{R}]";

            if (P[i] >= 2)
            {
                int origStart = (i - P[i]) / 2;
                desc += $"  \"{text.Substring(origStart, P[i])}\"";
                steps.Add(new AlgorithmStep { StepNumber = stepNum++, Array = (int[])tCodes.Clone(), PArray = (int[])P.Clone(), ManacherCenter = C, ManacherRight = R, HighlightIndices = Enumerable.Range(i - P[i], 2 * P[i] + 1).ToArray(), Description = desc });
            }
            else if (rule != "R4" || P[i] > 0)
                steps.Add(new AlgorithmStep { StepNumber = stepNum++, Array = (int[])tCodes.Clone(), PArray = (int[])P.Clone(), ManacherCenter = C, ManacherRight = R, HighlightIndices = P[i] > 0 ? Enumerable.Range(i - P[i], 2 * P[i] + 1).ToArray() : new[] { i }, Description = desc });
        }

        int maxRadius = 0, maxCenter = 0;
        for (int i = 0; i < tLen; i++) { if (P[i] > maxRadius) { maxRadius = P[i]; maxCenter = i; } }
        int resStart = (maxCenter - maxRadius) / 2;
        steps.Add(new AlgorithmStep { StepNumber = stepNum, Array = PatternHelpers.ToCharCodes(text), Description = $"Longest palindrome: \"{text.Substring(resStart, maxRadius)}\" (length {maxRadius})", SortedIndices = Enumerable.Range(resStart, maxRadius).ToArray() });
        return steps;
    }

    public List<AlgorithmStep> AnagramDetection(string text, string pattern)
    {
        PatternHelpers.ValidateText(text);
        PatternHelpers.ValidateText(pattern);
        var codes = PatternHelpers.ToCharCodes(text);
        var steps = new List<AlgorithmStep>();
        int step = 0, m = pattern.Length, n = text.Length;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])codes.Clone(), Description = $"Finding anagrams of \"{pattern}\" in \"{text}\"" });

        if (m > n) { steps.Add(new AlgorithmStep { StepNumber = step, Array = (int[])codes.Clone(), Description = "Pattern longer than text — no anagrams" }); return steps; }

        var pCount = new int[256]; var wCount = new int[256];
        foreach (char c in pattern) pCount[c]++;
        var found = new List<int>();

        for (int i = 0; i < n; i++)
        {
            wCount[text[i]]++;
            if (i >= m) wCount[text[i - m]]--;
            if (i >= m - 1)
            {
                bool match = pCount.SequenceEqual(wCount);
                int ws = i - m + 1;
                steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])codes.Clone(), Description = match ? $"Anagram found at index {ws}: \"{text.Substring(ws, m)}\"" : $"Window [{ws}..{i}]: \"{text.Substring(ws, m)}\" — not anagram", HighlightIndices = Enumerable.Range(ws, m).ToArray(), SortedIndices = match ? Enumerable.Range(ws, m).ToArray() : [] });
                if (match) found.Add(ws);
            }
        }

        steps.Add(new AlgorithmStep { StepNumber = step, Array = (int[])codes.Clone(), Description = found.Count > 0 ? $"Found {found.Count} anagram(s) at: {string.Join(", ", found)}" : "No anagrams found" });
        return steps;
    }
}

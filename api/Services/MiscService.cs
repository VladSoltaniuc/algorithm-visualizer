using api.Helpers;
using api.Models;

namespace api.Services;

public class MiscService
{
    public List<AlgorithmStep> Reversal(string text)
    {
        PatternHelpers.ValidateText(text);
        var arr = PatternHelpers.ToCharCodes(text);
        var steps = new List<AlgorithmStep>();
        int step = 0;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Reversing \"{text}\"" });

        int lo = 0, hi = arr.Length - 1;
        while (lo < hi)
        {
            steps.Add(new AlgorithmStep { StepNumber = step++, Array = (int[])arr.Clone(), Description = $"Swapping index {lo} ('{(char)arr[lo]}') and {hi} ('{(char)arr[hi]}')", HighlightIndices = [lo, hi] });
            (arr[lo], arr[hi]) = (arr[hi], arr[lo]);
            lo++; hi--;
        }

        var reversed = new string(arr.Select(c => (char)c).ToArray());
        steps.Add(new AlgorithmStep { StepNumber = step, Array = (int[])arr.Clone(), Description = $"Reversed: \"{reversed}\"", SortedIndices = Enumerable.Range(0, arr.Length).ToArray() });
        return steps;
    }

    public List<AlgorithmStep> HuffmanCoding(string text)
    {
        if (string.IsNullOrEmpty(text))
            throw new ArgumentException("Provide non-empty text.");

        var steps = new List<AlgorithmStep>();
        int step = 0;

        var freq = new Dictionary<char, int>();
        foreach (var ch in text) freq[ch] = freq.TryGetValue(ch, out int v) ? v + 1 : 1;

        var sortedChars = freq.OrderByDescending(kv => kv.Value).ThenBy(kv => kv.Key).ToList();
        steps.Add(new AlgorithmStep
        {
            StepNumber = step++,
            Array = sortedChars.Select(kv => kv.Value).ToArray(),
            Labels = sortedChars.Select(kv => kv.Key == ' ' ? "·" : kv.Key.ToString()).ToArray(),
            Description = $"Count how often each character appears. Most frequent: '{sortedChars[0].Key}' ({sortedChars[0].Value}×). {freq.Count} unique character(s) in \"{text}\".",
            HighlightIndices = Enumerable.Range(0, sortedChars.Count).ToArray(),
        });

        var nodes = new List<(int weight, string label, HuffNode node)>();
        foreach (var kv in freq) nodes.Add((kv.Value, kv.Key == ' ' ? "·" : kv.Key.ToString(), new HuffNode(kv.Key, kv.Value)));
        nodes.Sort((a, b) => a.weight != b.weight ? a.weight.CompareTo(b.weight) : string.Compare(a.label, b.label, StringComparison.Ordinal));

        steps.Add(new AlgorithmStep { StepNumber = step++, Array = nodes.Select(n => n.weight).ToArray(), Labels = nodes.Select(n => n.label).ToArray(), Description = "Place every character in a priority queue sorted by frequency (lowest first). The two smallest nodes will always be merged next." });

        while (nodes.Count > 1)
        {
            var left = nodes[0]; var right = nodes[1];
            nodes.RemoveAt(0); nodes.RemoveAt(0);
            int merged = left.weight + right.weight;
            string mergedLabel = $"{left.label}+{right.label}";
            var parent = new HuffNode(null, merged) { Left = left.node, Right = right.node };
            int pos = nodes.FindIndex(n => n.weight >= merged);
            if (pos < 0) pos = nodes.Count;
            nodes.Insert(pos, (merged, mergedLabel, parent));
            steps.Add(new AlgorithmStep { StepNumber = step++, Array = nodes.Select(n => n.weight).ToArray(), Labels = nodes.Select(n => n.label).ToArray(), Description = $"Take the two lightest nodes '{left.label}' ({left.weight}) and '{right.label}' ({right.weight}). Combine them into weight {merged}. Re-insert at position {pos + 1}.", HighlightIndices = [pos] });
        }

        var codes = new Dictionary<char, string>();
        MiscHelpers.GenerateCodes(nodes[0].node, "", codes);

        var codeList = codes.OrderBy(kv => kv.Value.Length).ThenBy(kv => kv.Key).ToList();
        string encoded = string.Concat(text.Select(ch => codes[ch]));
        int originalBits = text.Length * 8, huffBits = encoded.Length;

        string codesDesc = string.Join(", ", codeList.Select(kv => $"'{(kv.Key == ' ' ? "·" : kv.Key.ToString())}'={kv.Value}"));
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = codeList.Select(kv => kv.Value.Length).ToArray(), Labels = codeList.Select(kv => kv.Key == ' ' ? "·" : kv.Key.ToString()).ToArray(), Notes = codeList.Select(kv => kv.Value).ToArray(), Description = $"Assign codes: left=0, right=1. Frequent chars get shorter codes. {codesDesc}.", SortedIndices = Enumerable.Range(0, codeList.Count).ToArray() });

        int savedBits = originalBits - huffBits;
        steps.Add(new AlgorithmStep { StepNumber = step, Array = [originalBits, huffBits], Labels = ["Original (ASCII)", "Huffman"], Notes = [$"{text.Length} chars × 8 bits", $"saved {savedBits} bits ({(double)savedBits / originalBits:P0})"], Description = $"Original: {originalBits} bits. Huffman: {huffBits} bits. Saved {savedBits} bits — {(double)savedBits / originalBits:P1} smaller.", SortedIndices = [0, 1] });
        return steps;
    }

    public List<AlgorithmStep> BitManipulation(int n)
    {
        if (n < 0) throw new ArgumentException("n must be non-negative.");
        var steps = new List<AlgorithmStep>();
        int step = 0;
        var bits = MiscHelpers.ToBits(n);
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = bits, Description = $"n = {n}, binary = {Convert.ToString(n, 2)}" });

        int count = 0, temp = n;
        while (temp > 0) { count += temp & 1; temp >>= 1; }
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = bits, Description = $"Set bits count = {count}", HighlightIndices = Enumerable.Range(0, bits.Length).Where(i => bits[i] == 1).ToArray() });

        bool isPow2 = n > 0 && (n & (n - 1)) == 0;
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = [n, n - 1, n & (n - 1)], Description = $"n & (n-1) = {n & (n - 1)} → {(isPow2 ? "power of 2" : "not power of 2")}", HighlightIndices = [2] });
        steps.Add(new AlgorithmStep { StepNumber = step++, Array = [n, n, n ^ n], Description = $"n XOR n = {n ^ n} (always 0)", HighlightIndices = [2] });

        int toggled = ~n;
        steps.Add(new AlgorithmStep { StepNumber = step, Array = MiscHelpers.ToBits(toggled & ((1 << bits.Length) - 1)), Description = $"NOT n = {Convert.ToString(toggled & ((1 << bits.Length) - 1), 2)} (within bit width)", SortedIndices = Enumerable.Range(0, bits.Length).ToArray() });
        return steps;
    }
}

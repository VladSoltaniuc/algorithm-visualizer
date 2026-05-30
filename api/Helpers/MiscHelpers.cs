namespace api.Helpers;

internal class HuffNode
{
    public char? Char;
    public int Weight;
    public HuffNode? Left, Right;
    public HuffNode(char? ch, int w) { Char = ch; Weight = w; }
}

internal static class MiscHelpers
{
    internal static int[] ToBits(int n)
    {
        if (n == 0) return [0];
        var bits = new List<int>();
        int temp = n;
        while (temp > 0) { bits.Add(temp & 1); temp >>= 1; }
        bits.Reverse();
        return bits.ToArray();
    }

    internal static void GenerateCodes(HuffNode node, string prefix, Dictionary<char, string> codes)
    {
        if (node.Left == null && node.Right == null && node.Char.HasValue)
        {
            codes[node.Char.Value] = prefix.Length > 0 ? prefix : "0";
            return;
        }
        if (node.Left != null) GenerateCodes(node.Left, prefix + "0", codes);
        if (node.Right != null) GenerateCodes(node.Right, prefix + "1", codes);
    }
}

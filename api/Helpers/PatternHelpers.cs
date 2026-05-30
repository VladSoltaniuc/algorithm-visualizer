namespace api.Helpers;

internal static class PatternHelpers
{
    internal static void ValidateText(string text)
    {
        if (string.IsNullOrEmpty(text))
            throw new ArgumentException("Provide a non-empty text string.");
    }

    internal static int[] ToCharCodes(string s) => s.Select(c => (int)c).ToArray();

    internal static int[] BuildGoodSuffixTable(string pattern)
    {
        int m = pattern.Length;
        int[] shift = new int[m + 1];
        int[] border = new int[m + 1];

        int i = m, j = m + 1;
        border[i] = j;
        while (i > 0)
        {
            while (j <= m && pattern[i - 1] != pattern[j - 1])
            {
                if (shift[j] == 0) shift[j] = j - i;
                j = border[j];
            }
            border[--i] = --j;
        }

        j = border[0];
        for (i = 0; i <= m; i++)
        {
            if (shift[i] == 0) shift[i] = j;
            if (i == j) j = border[j];
        }

        return shift;
    }
}

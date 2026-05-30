namespace api.Helpers;

internal static class FindHelpers
{
    internal static void ValidateInput(int[] input)
    {
        if (input is not { Length: > 0 })
            throw new ArgumentException("Provide a non-empty array of integers.");
    }
}

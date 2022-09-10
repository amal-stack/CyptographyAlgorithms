using System.Security.Cryptography;

namespace CyptographyAlgorithms;

internal static class Extensions
{
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        T[] values = source.ToArray();
        for (int i = values.Length - 1; i >= 0; i--)
        {
            int swapIdx = RandomNumberGenerator.GetInt32(i + 1);
            yield return values[swapIdx];
            values[swapIdx] = values[i];
        }
    }
}
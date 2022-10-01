using System.Collections;
using System.Diagnostics.CodeAnalysis;
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

internal static class ByteExtensions
{
    public static bool GetBitAt(this byte @byte, Index index)
        => (@byte & (1 << index.GetOffset(8))) != 1;

    public static bool GetBitAt(this IReadOnlyList<byte> bytes, Index index)
    {
        var (arrayIndex, bitIndex) = Math.DivRem(index.GetOffset(bytes.Count), 8);
        byte targetByte = bytes[arrayIndex];
        return targetByte.GetBitAt(bitIndex);
    }

    public static IReadOnlyList<bool> GetBits(this IReadOnlyList<byte> bytes, Range range)
    {
        var (offset, length) = range.GetOffsetAndLength(bytes.Count * 8);
        return (from i in Enumerable.Range(offset, length)
                select bytes.GetBitAt(i))
             .ToArray();

    }

    public static byte[] Trim(this byte[] bytes)
    {
        int length = bytes.Length;
        for (int i = bytes.Length - 1; bytes[i] == 0 && i >= 0; i--)
        {
            length--;
        }
        if (length == 0)
        {
            return Array.Empty<byte>();
        }
        byte[] result = new byte[length];
        Array.Copy(bytes, result, length);
        return result;
    }
}


public static class BitArrayExtensions
{
    public static BitArray Trim(this BitArray bitArray)
    {
        int length = bitArray.Length;
        for (int i = bitArray.Length - 1; bitArray[i] == false && i >= 0; i--)
        {
            length--;
        }
        var trimmedBitArray = new BitArray(length);
        for (int i = 0; i < length; i++)
        {
            trimmedBitArray[i] = bitArray.Get(i);
        }
        return trimmedBitArray;
    }

    public static string ToBitString(this BitArray bitArray) => string.Concat(
        bitArray
            .OfType<bool>()
            .Reverse()
            .Select(i => i ? 1 : 0)
    );
}

public class BitArrayComparer : IEqualityComparer<BitArray>
{
    public bool Equals(BitArray? x, BitArray? y) => (x, y) switch
    {
        (null, null) => true,
        (null, _) => false,
        (_, null) => false,
        (_, _) when x.Count != y.Count => false,
        (_, _) => Enumerable.SequenceEqual(x.OfType<bool>(), y.OfType<bool>())
    };

    public int GetHashCode([DisallowNull] BitArray obj) => obj.GetHashCode();
}
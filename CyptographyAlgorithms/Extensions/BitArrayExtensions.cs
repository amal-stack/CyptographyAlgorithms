using System.Collections;

namespace CyptographyAlgorithms.Extensions;

public static class BitArrayExtensions
{
    public static bool[] GetRange(this BitArray bitArray, Range range)
    {
        var (offset, length) = range.GetOffsetAndLength(bitArray.Count);
        return (from i in Enumerable.Range(offset, length)
                select bitArray.Get(i))
             .ToArray();
    }

    public static BitArray Trim(this BitArray bitArray)
    {
        int length = bitArray.Length;
        for (int i = bitArray.Length - 1; i >= 0 && bitArray[i] == false; i--)
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

    public static byte ToByte(this BitArray bitArray)
    {
        var trimmedBitArray = bitArray.Trim();
        if (trimmedBitArray.Length > 8)
        {
            throw new ArgumentOutOfRangeException(
                nameof(bitArray),
                "The length of the BitArray must be lesser than or equal to 8");
        }
        byte[] result = new byte[1];
        trimmedBitArray.CopyTo(result, 0);
        return result[0];
    }


}

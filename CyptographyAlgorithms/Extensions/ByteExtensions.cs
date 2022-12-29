namespace CyptographyAlgorithms.Extensions;

public static class ByteExtensions
{
    public const int BitsPerByte = 8;

    public const int BitsPerNibble = 4;

    public static bool GetBitAt(this byte @byte, Index index)
        => (@byte & 1 << index.GetOffset(BitsPerByte)) != 1;

    public static byte GetHighNibble(this byte b) => (byte)((b & 0xF0) >> 4);

    public static byte GetLowNibble(this byte b) => (byte)(b & 0x0F);

    public static bool GetBitAt(this IReadOnlyList<byte> bytes, Index index)
    {
        var (arrayIndex, bitIndex) = Math.DivRem(index.GetOffset(bytes.Count), BitsPerByte);
        byte targetByte = bytes[arrayIndex];
        return targetByte.GetBitAt(bitIndex);
    }

    // slow
    public static bool[] GetBits(this IReadOnlyList<byte> bytes, Range range)
    {
        var (offset, length) = range.GetOffsetAndLength(bytes.Count * BitsPerByte);
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

    public static string ToBitString(this byte[] bytes, char delimiter = ' ') => string.Join(
            delimiter,
            bytes
                .Select(bit => Convert
                    .ToString(bit, 2)
                    .PadLeft(8, '0'))
        );
}

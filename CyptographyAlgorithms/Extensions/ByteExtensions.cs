namespace CyptographyAlgorithms.Extensions;

public static class ByteExtensions
{
    public static bool GetBitAt(this byte @byte, Index index)
        => (@byte & (1 << index.GetOffset(BitConstants.BitsPerByte))) != 0;

    public static bool GetBitAt(this IReadOnlyList<byte> bytes, Index index)
    {
        var (arrayIndex, bitIndex) = Math.DivRem(index.GetOffset(bytes.Count * BitConstants.BitsPerByte), BitConstants.BitsPerByte);
        byte targetByte = bytes[arrayIndex];
        return targetByte.GetBitAt(bitIndex);
    }

    // slow
    public static bool[] GetBits(this IReadOnlyList<byte> bytes, Range range)
    {
        var (offset, length) = range.GetOffsetAndLength(bytes.Count * BitConstants.BitsPerByte);
        return (from i in Enumerable.Range(offset, length)
                select bytes.GetBitAt(i))
             .ToArray();
    }

    public static byte[] GetBitRange(this byte[] bytes, int start, int length)
        => GetBitRange(bytes, start, length, Endianness.BigEndian, BitNumbering.Lsb0);

    public static byte[] GetBitRange(this byte[] bytes, int start, int length, Endianness endianness)
        => GetBitRange(bytes, start, length, endianness, BitNumbering.Lsb0);

    public static byte[] GetBitRange(this byte[] bytes, int start, int length, BitNumbering bitNumbering)
        => GetBitRange(bytes, start, length, Endianness.BigEndian, bitNumbering);

    /// <summary>
    /// Extracts a range of bits from a byte array into a new byte array.
    /// </summary>
    /// <param name="bytes">The byte array to extract the range from.</param>
    /// <param name="start">The 0-based start index of the bit.</param>
    /// <param name="length">The number of bits to extract.</param>
    /// <returns>A new <see cref="byte"/> array with the extracted bits.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="start"/> or <paramref name="length"/> are out of range.</exception>
    public static byte[] GetBitRange(
        this byte[] bytes, 
        int start, 
        int length, 
        Endianness endianness, 
        BitNumbering bitNumbering)
    { 
        int bitsPerByte = 8;
        // Calculate the length of the input byte array in bits 
        int inputLength = bytes.Length * bitsPerByte;

        int end = start + length;

        // Range validations
        if (start >= inputLength || start < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(start), start, $"Start must non-negative and lesser than {inputLength}");
        }
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), length, $"Length must be non-negative");
        }
        if (end > inputLength)
        {
            throw new ArgumentOutOfRangeException(nameof(length), length, $"Range length must be less than or equal to {inputLength}");
        }

        // Calculate the length of the new byte array and allocate
        var (byteLength, remainderLength) = Math.DivRem(length, bitsPerByte);
        byte[] result = new byte[byteLength + (remainderLength == 0 ? 0 : 1)];
        // Iterate through each byte in the new byte array
        for (int i = 0; i < result.Length; i++)
        {
            // Compute each bit of the ith byte
            for (int j = 0; j < bitsPerByte && start < end; j++)
            {
                // Note the increment in start
                var (inputByteIndex, inputBitIndex) = Math.DivRem(start++, bitsPerByte);

                int outputBitIndex = j;
                if (bitNumbering is BitNumbering.Msb0)
                {
                    outputBitIndex = 7 - outputBitIndex;
                    inputBitIndex = 7 - inputBitIndex;
                }
                if (endianness is Endianness.LittleEndian)
                {
                    inputByteIndex = bytes.Length - 1 - inputByteIndex;
                }
                result[i] |= (byte)(((bytes[inputByteIndex] >> inputBitIndex) & 1) << outputBitIndex);
            }
        }
        return result;
    }

    public static byte[] Trim(this byte[] bytes)
    {
        int length = bytes.Length;
        for (int i = bytes.Length - 1; bytes[i] == 0 && i >= 0; i--, length--) ;

        if (length == 0)
        {
            return Array.Empty<byte>();
        }
        byte[] result = new byte[length];
        Array.Copy(bytes, result, length);
        return result;
    }

    
}

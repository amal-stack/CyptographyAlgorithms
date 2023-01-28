namespace CyptographyAlgorithms.Extensions;

public static class BitManipulation
{
    public static int GetMask(int bitCount)
    {
        if (bitCount > BitConstants.BitsPerByte)
        {
            throw new ArgumentOutOfRangeException(
                nameof(bitCount),
                $"The maximum number of bits per byte is {BitConstants.BitsPerByte}");
        }
        return (int)Math.Pow(2, bitCount) - 1;
    }

    public static (int ByteIndex, int BitIndex) GetBitAndByteIndexes(
        int bitIndex,
        int length,
        Endianness endianness,
        BitNumbering bitNumbering)
    {
        var (outputbyteIndex, outputBitIndex) = Math.DivRem(bitIndex, BitConstants.BitsPerByte);

        if (endianness is Endianness.LittleEndian)
        {
            outputbyteIndex = length - 1 - outputbyteIndex;
        }
        if (bitNumbering is BitNumbering.Msb0)
        {
            outputBitIndex = BitConstants.BitsPerByte - 1 - outputBitIndex;
        }
        return (outputbyteIndex, outputBitIndex);
    }
}

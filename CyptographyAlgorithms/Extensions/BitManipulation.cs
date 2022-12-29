namespace CyptographyAlgorithms.Extensions;

public static class BitManipulation
{
    public static int GetMask(int bitCount)
    {
        if (bitCount > ByteExtensions.BitsPerByte)
        {
            throw new ArgumentOutOfRangeException(
                nameof(bitCount),
                $"The maximum number of bits per byte is {ByteExtensions.BitsPerByte}");
        }
        return (int)Math.Pow(2, bitCount) - 1;
    }
}

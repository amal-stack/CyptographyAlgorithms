using CyptographyAlgorithms.Extensions;

namespace CyptographyAlgorithms.Spn;

public sealed class SpnPbox : ITransformation
{
    private readonly IReadOnlyDictionary<int, int> _table = new Dictionary<int, int>()
    {
        [1] = 1,
        [2] = 5,
        [3] = 9,
        [4] = 13,
        [5] = 2,
        [6] = 6,
        [7] = 10,
        [8] = 14,
        [9] = 3,
        [10] = 7,
        [11] = 11,
        [12] = 15,
        [13] = 4,
        [14] = 8,
        [15] = 12,
        [16] = 16
    };

    private readonly IReadOnlyDictionary<int, int> _inverseTable;

    public SpnPbox()
    {
        _inverseTable = _table.ToDictionary(p => p.Value, p => p.Key);
    }

    public byte[] Apply(byte[] value) => Apply(value, _table);

    public byte[] ApplyInverse(byte[] value) => Apply(value, _inverseTable);

    private static byte[] Apply(byte[] value, IReadOnlyDictionary<int, int> table)
    {
        byte[] result = new byte[value.Length];
        var bitsPerByte = BitConstants.BitsPerByte;
        for (int i = 0; i < result.Length; i++)
        {
            for (int j = 0; j < bitsPerByte; j++)
            {
                var inputIndex = i * bitsPerByte + j;
                var (inputByteIndex, inputBitIndex) = Math.DivRem(inputIndex, bitsPerByte);

                // + 1 and -1 to compensate for 1-based indexing
                int outputIndex = table[inputIndex + 1] - 1;
                var (outputByteIndex, outputBitIndex) = Math.DivRem(outputIndex, bitsPerByte);

                // (BitsPerByte - 1 - index) to address bits MSB first 
                int outputBit = (value[outputByteIndex] >> (bitsPerByte - 1 - outputBitIndex)) & 1;
                result[inputByteIndex] |= (byte)(outputBit << (bitsPerByte - 1 - inputBitIndex));
            }
        }
        return result;
    }
}

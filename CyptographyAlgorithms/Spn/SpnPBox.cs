namespace CyptographyAlgorithms.Spn;

public sealed class SpnPBox : ITransformation
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

    public SpnPBox()
    {
        _inverseTable = _table.ToDictionary(p => p.Value, p => p.Key);
    }

    public byte[] Apply(byte[] value) => Apply(value, _table);

    public byte[] ApplyInverse(byte[] value) => Apply(value, _inverseTable);

    private byte[] Apply(byte[] value, IReadOnlyDictionary<int, int> table)
    {
        byte[] result = new byte[value.Length];
        for (int i = 0; i < value.Length; i++)
        {
            int highPosition = table[i];
            int lowPosition = table[i + 1];
            result[i] = (byte)(value[highPosition] << 4 | value[lowPosition]);
        }
        return result;
    }
}

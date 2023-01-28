using CyptographyAlgorithms.Extensions;

namespace CyptographyAlgorithms.Spn;

public sealed class SpnSbox : ITransformation
{
    private readonly IReadOnlyDictionary<byte, byte> _table = new Dictionary<byte, byte>()
    {
        [0x00] = 0x0E,
        [0x01] = 0x04,
        [0x02] = 0x0D,
        [0x03] = 0x01,
        [0x04] = 0x02,
        [0x05] = 0x0F,
        [0x06] = 0x0B,
        [0x07] = 0x08,
        [0x08] = 0x03,
        [0x09] = 0x0A,
        [0x0A] = 0x06,
        [0x0B] = 0x0C,
        [0x0C] = 0x05,
        [0x0D] = 0x09,
        [0x0E] = 0x00,
        [0x0F] = 0x07
    };

    private readonly IReadOnlyDictionary<byte, byte> _inverseTable;

    public SpnSbox()
    {
        _inverseTable = _table.ToDictionary(p => p.Value, p => p.Key);
    }


    public byte[] Apply(byte[] value) => Apply(value, _table);

    public byte[] ApplyInverse(byte[] value) => Apply(value, _inverseTable);

    private static byte[] Apply(byte[] value, IReadOnlyDictionary<byte, byte> table)
    {
        byte[] result = new byte[value.Length];
        for (int i = 0; i < value.Length; i++)
        {
            byte b = value[i];
            byte high = table[b.GetHighNibble()];
            byte low = table[b.GetLowNibble()];
            result[i] = Nibble.ToByte(high, low);
        }
        return result;
    }
}
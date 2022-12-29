using System.Collections;

namespace CyptographyAlgorithms;


public interface ICipher
{
    string Encipher(string plaintext);
    string Decipher(string ciphertext);
}

public interface ICipher<T>
{
    IReadOnlyList<T> Encipher(IReadOnlyList<T> plaintext);
    IReadOnlyList<T> Decipher(IReadOnlyList<T> ciphertext);
}

public sealed class SBox
{
    private IReadOnlyDictionary<byte, byte> @abc = new Dictionary<byte, byte>()
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

    public int InCount { get; }

    public int OutCount { get; }

    public IDictionary<byte, BitArray> _bitArrayMap = new Dictionary<byte, BitArray>();

    private IReadOnlyDictionary<byte, byte> Table { get; }

    private IReadOnlyDictionary<byte, byte> InverseTable { get; }

    private SBox(IReadOnlyDictionary<byte, byte> table, int inCount, int outCount)
    {
        Table = table;
        InverseTable = table.ToDictionary(p => p.Value, table => table.Key);
        InCount = inCount;
        OutCount = outCount;
    }

    public static SBox FromDictionary(IReadOnlyDictionary<byte, byte> dictionary, int inCount, int outCount)
        => new(dictionary, inCount, outCount);

    public BitArray Apply(IReadOnlyList<byte> value)
    {
        var bits = new BitArray(value.ToArray()).Trim();
        var outBitList = new List<bool>();
        var count = bits.Count;
        var (blockCount, remCount) = Math.DivRem(count, InCount);
        for (int i = 0; i < blockCount; i++)
        {
            Range range = ^(i + InCount)..^i;
            var block = new BitArray(bits.GetRange(range)).ToByte();
            var substitutionBlock = new BitArray(new[] { abc[block] }).Trim();
            for (int j = 0; j < OutCount; j++)
            {
                outBitList.Add(j < substitutionBlock.Count && substitutionBlock[j]);
            }
        }
        if (remCount > 0)
        {
            var block = new BitArray(bits.GetRange(..^(blockCount * InCount))).ToByte();
            var substitutionBlock = new BitArray(abc[block]).Trim();
            for (int j = 0; j < remCount; j++)
            {
                outBitList.Add(j < substitutionBlock.Count && substitutionBlock[j]);
            }
        }
        outBitList.Reverse();
        return new BitArray(outBitList.ToArray());
    }

    public IReadOnlyList<byte> ApplyInverse(IReadOnlyList<byte> value)
    {
        throw new NotImplementedException();
    }
}


public sealed class PBox : ITransformation
{
    private IReadOnlyDictionary<byte, byte> @abc = new Dictionary<byte, byte>()
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

    private IReadOnlyDictionary<int, int> Table { get; }

    private IReadOnlyDictionary<int, int> InverseTable { get; }

    public int InSize { get; } = 8;

    public int OutSize { get; } = 8;

    private PBox(IReadOnlyDictionary<int, int> table)
    {
        Table = table;
        InverseTable = table.ToDictionary(p => p.Value, p => p.Key);
    }

    public byte[] Apply(byte[] bytes)
    {
        var array = new BitArray(bytes);
        return null!;
    }

    public byte[] ApplyInverse(byte[] bytes)
    {
        return null!;
    }

    public static PBox FromDictionary(IReadOnlyDictionary<int, int> dictionary)
        => new(dictionary);

    public static PBox FromList(IReadOnlyList<int> list) => new(list
         .Select((elem, idx) => new
         {
             Element = elem,
             Index = idx
         }).ToDictionary(x => x.Index, x => x.Element)
    );
}





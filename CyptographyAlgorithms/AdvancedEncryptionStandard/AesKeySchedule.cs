using System.Buffers;

namespace CyptographyAlgorithms.AdvancedEncryptionStandard;

public sealed class AesKeySchedule
{
    private static readonly byte[] s_roundConstants =
    {
        0x01, 0x02, 0x04, 0x08, 0x10,
        0x20, 0x40, 0x80, 0x1B, 0x36
    };

    private readonly byte[] _masterKey;
    private readonly int _keyLength;
    private readonly int _rounds;
    private const int _columns = 4;

    public int Rounds => _rounds;

    public AesKeySchedule(byte[] masterKey)
    {
        _masterKey = masterKey;
        _keyLength = _masterKey.Length / 4;
        //_columns = _masterKey.Length / _keyLength;
        _rounds = _keyLength + 6;
    }

    private static Word RotWord(Word word)
    {
        Word result = new();

        for (int i = 0; i < Word.Size - 1; i++)
        {
            result[i] = word[i + 1];
        }
        result[^1] = word[0];
        return result;
    }

    private static Word SubWord(Word word)
    {
        Word result = new();

        for (int i = 0; i < Word.Size; i++)
        {
            result[i] = AesSbox.Get(word[i]);
        }
        return result;
    }

    public byte[][] GetRoundKeys()
    {
        int numberOfRoundKeys = _columns * (_rounds + 1);
        Word[] result = new Word[numberOfRoundKeys];
        int i;
        for (i = 0; i < _keyLength; i++)
        {
            result[i] = new Word(_masterKey[(4 * i)..(4 * i + 4)]);
        }
        for (i = _keyLength; i < numberOfRoundKeys; i++)
        {
            Word temp = result[i - 1];

            if (i % _keyLength == 0)
            {
                temp = SubWord(RotWord(temp)) ^ s_roundConstants[(i / _keyLength) - 1];
            }
            else if (_keyLength > 6 && i % _keyLength == 4)
            {
                temp = SubWord(temp);
            }
            result[i] = result[i - _keyLength] ^ temp;
        }
        return result
            .Select(w => w.GetBytes())
            .ToArray();
    }
}

namespace CyptographyAlgorithms.Aes;

struct Word
{
    private readonly byte[] _bytes;

    public byte this[Index i]
    {
        get => _bytes[i];
        set => _bytes[i] = value;
    }

    public const int Size = 4;

    public Word(byte b1, byte b2, byte b3, byte b4)
    {
        _bytes = new[] { b1, b2, b3, b4 };
    }

    public Word(byte[] bytes)
    {
        if (bytes.Length != 4)
        {
            throw new ArgumentException(
            $"A {typeof(Word)} must be exactly 4 bytes", nameof(bytes));
        }
        _bytes = bytes;
    }

    public Word()
    {
        _bytes = new byte[4];
    }

    public static Word operator ^(Word word1, Word word2) => new(
            (byte)(word1[0] ^ word2[0]),
            (byte)(word1[1] ^ word2[1]),
            (byte)(word1[2] ^ word2[2]),
            (byte)(word1[3] ^ word2[3])
    );

    public static Word operator ^(Word word, byte @byte)
    {
        byte result = (byte)(word[0] ^ @byte);
        return new Word(result, word[1], word[2], word[3]);
    }


    internal byte[] GetBytes() => _bytes;

    public void Deconstruct(out byte b1, out byte b2, out byte b3, out byte b4)
    {
        (b1, b2, b3, b4) = (_bytes[0], _bytes[1], _bytes[2], _bytes[3]);
    }
}

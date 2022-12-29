namespace CyptographyAlgorithms.Spn;

public sealed class SPNetwork
{
    private byte[] Key { get; }

    public ITransformation SBox { get; }

    public ITransformation PBox { get; }

    public int Rounds { get; }
    public Func<byte[], int, byte[]> RoundkeyGenAlgorithm { get; }

    internal SPNetwork(
        byte[] key,
        Func<byte[], int, byte[]> roundkeyGenAlgorithm,
        ITransformation sBox,
        ITransformation pBox,
        int rounds)
    {
        Key = key;
        RoundkeyGenAlgorithm = roundkeyGenAlgorithm;
        SBox = sBox;
        PBox = pBox;
        Rounds = rounds;
    }

    public IReadOnlyList<byte> Decipher(byte[] ciphertext)
    {
        byte[] result = ciphertext;
        for (int i = 0; i < Rounds; i++)
        {
            byte[] roundKey = RoundkeyGenAlgorithm?.Invoke(Key, i) ?? throw new InvalidOperationException();
            result = PBox.ApplyInverse(SBox.ApplyInverse(Xor(result, roundKey)));
        }
        return result;
    }

    public IReadOnlyList<byte> Encipher(byte[] plaintext)
    {
        byte[] result = plaintext;
        for (int i = 0; i < Rounds; i++)
        {
            byte[] roundKey = RoundkeyGenAlgorithm?.Invoke(Key, i) ?? throw new InvalidOperationException();
            result = PBox.Apply(SBox.Apply(Xor(result, roundKey)));
        }
        return result;
    }

    private static byte[] Xor(byte[] first, byte[] second)
    {
        byte[] result = new byte[Math.Min(first.Length, second.Length)];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = (byte)(first[i] ^ second[i]);
        }
        return result;
    }
}

using CyptographyAlgorithms.Extensions;

namespace CyptographyAlgorithms.Spn;

public sealed class SPNetwork
{
    private byte[] Key { get; }

    public ITransformation Sbox { get; }

    public ITransformation Pbox { get; }

    public int Rounds { get; }

    public SpnKeySchedule RoundkeyGenAlgorithm { get; }

    public Action<string>? LogCallback { get; init; }

    internal SPNetwork(
        byte[] key,
        SpnKeySchedule roundkeyGenAlgorithm,
        ITransformation sBox,
        ITransformation pBox,
        int rounds)
    {
        Key = key;
        RoundkeyGenAlgorithm = roundkeyGenAlgorithm;
        Sbox = sBox;
        Pbox = pBox;
        Rounds = rounds;
    }

    public byte[] Decipher(byte[] ciphertext)
    {
        Log("Decrypting...");
        byte[] result = ciphertext;
        for (int i = 0; i < Rounds; i++)
        {
            Log($"Round {i + 1}");
            Log(result, "Input:");
            byte[] roundKey = RoundkeyGenAlgorithm?.Invoke(Key, Rounds - i) ?? throw new InvalidOperationException();
            Log(roundKey, "Round Key:");

            result = Pbox.ApplyInverse(result);
            Log(result, "Inverse PBox:");

            result = Sbox.ApplyInverse(result);
            Log(result, "Inverse SBox:");

            result = Xor(result, roundKey);
            Log(result, "Input XOR Round Key:");

        }
        Log("Decryption Complete.");
        Log(new string('-', 50));
        return result;
    }

    public byte[] Encipher(byte[] plaintext)
    {
        Log("Encrypting...");
        byte[] result = plaintext;
        for (int i = 0; i < Rounds; i++)
        {
            Log($"Round {i + 1}");
            Log(result, "Input:");
            byte[] roundKey = RoundkeyGenAlgorithm?.Invoke(Key, i + 1) ?? throw new InvalidOperationException();
            Log(roundKey, "Round Key:");

            //Console.Write("R*:  ");
            //Console.WriteLine(string.Concat(Enumerable.Range(0, 16).Select(i => roundKey.GetBitAt(i) ? 1.ToString() : 0.ToString())));

            result = Xor(result, roundKey);
            Log(result, "Input XOR Round Key:");

            result = Sbox.Apply(result);
            Log(result, "SBox:");

            result = Pbox.Apply(result);
            Log(result, "PBox:");
        }
        Log("Encryption Complete.");
        Log(new string('-', 50));
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

    private void Log(byte[] result, string message)
        => LogCallback?.Invoke($"{message,-25}{result.ToBitString()}");

    private void Log(string message)
        => LogCallback?.Invoke(message);
}

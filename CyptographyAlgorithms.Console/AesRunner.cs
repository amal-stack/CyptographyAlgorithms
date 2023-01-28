using CyptographyAlgorithms.AdvancedEncryptionStandard;

namespace CyptographyAlgorithms.Console;

using Console = System.Console;

[Demonstrates<Aes>]
public class AesRunner : ICryptoAlgorithmRunner
{

    private static readonly byte[] _key128 
        = Convert.FromHexString("000102030405060708090a0b0c0d0e0f");

    private static readonly byte[] _key192 
        = Convert.FromHexString("000102030405060708090a0b0c0d0e0f1011121314151617");

    private static readonly byte[] _key256
        = Convert.FromHexString("000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f");

    private static readonly byte[] _plaintext 
        = Convert.FromHexString("00112233445566778899aabbccddeeff");

    public static void Run()
    {
        string rule = new('=', 64);

        Console.WriteLine("AES-128");
        Console.WriteLine(rule);
        Run(_key128, _plaintext);
        Console.WriteLine();

        Console.WriteLine("AES-192");
        Console.WriteLine(rule);
        Run(_key192, _plaintext);
        Console.WriteLine();

        Console.WriteLine("AES-256");
        Console.WriteLine(rule);
        Run(_key256, _plaintext);
        Console.WriteLine();
    }

    private static void Run(byte[] key, byte[] plaintext)
    {
        var aes = new Aes(key) { LogCallback = Console.WriteLine };

        Console.WriteLine("ENCRYPTION");
        var ciphertext = aes.Encrypt(plaintext);
        Console.WriteLine(Convert.ToHexString(ciphertext).ToLowerInvariant());

        Console.WriteLine();

        Console.WriteLine("DECRYPTION");
        var decryptedPlaintext = aes.Decrypt(ciphertext);
        Console.WriteLine(Convert.ToHexString(decryptedPlaintext).ToLowerInvariant());
    }
}

using System.Buffers.Binary;
using System.Collections;
using System.Numerics;
using CyptographyAlgorithms.Extensions;

namespace CyptographyAlgorithms.Knapsack;

public class KnapsackCryptosystem
{
    const int BitsPerByte = 8;

    private KnapsackCryptosystem(PublicKeyInfo publicKey, PrivateKeyInfo privateKey)
    {
        PublicKey = publicKey;
        PrivateKey = privateKey;
    }

    public static KnapsackCryptosystem Create(
        IReadOnlyList<int> sequence, int m, int w)
    {
        if (sequence.Count == 0)
        {
            throw new ArgumentException(
                "Sequence must be non-empty.", 
                nameof(sequence));
        }
        int sum = sequence[0];
        for (int i = 1; i < sequence.Count; i++)
        {
            if (sequence[i] <= sum)
            {
                throw new ArgumentException(
                    "Not a superincreasing sequence.", 
                    nameof(sequence));
            }
            sum += sequence[i];
        }
        if (m <= sum)
        {
            throw new ArgumentOutOfRangeException(
                nameof(m), 
                "m must be greater than the sum of the sequence.");
        }

        if (MathHelpers.Gcd(m, w) != 1)
        {
            throw new ArgumentException(
                "w must be coprime to m.", 
                nameof(w));
        }

        IReadOnlyList<int> publicSequence = GeneratePublicSequence(sequence, m, w);

        return new KnapsackCryptosystem(
            new PublicKeyInfo(publicSequence),
            new PrivateKeyInfo(sequence, m, w)
        );
    }

    private static IReadOnlyList<int> GeneratePublicSequence(
        IReadOnlyList<int> sequence, int m, int w) => (
            from item in sequence
            select (w * item) % m)
            .ToList()
            .AsReadOnly();    // Multiply each item in the sequence by w and mod by m

    public PublicKeyInfo PublicKey { get; }

    private PrivateKeyInfo PrivateKey { get; }

    public byte[] Encrypt(byte[] plaintext)
    {
        var plaintextBits = new BitArray(plaintext);
        var sequence = PublicKey.Sequence;
        
        // Calculate number of ciphertext blocks:
        // Dividing the length of the sequence by the bit length of the plaintext gives
        // the number of ciphertext blocks. If the plaintext bit length is not a multiple of 
        // the sequence length (remainder != 0), an extra block is required.
        var (quotient, remainder) = Math.DivRem(plaintextBits.Count, sequence.Count);
        var ciphertextBlocks = quotient + (remainder == 0 ? 0 : 1);


        var ciphertext = new BigInteger[ciphertextBlocks];

        //Console.WriteLine($"Ciphertext Length {ciphertextBlocks}");

        Console.WriteLine("Bits");

        // Apply the sequence to each plaintext block
        for (int i = 0; i < ciphertextBlocks; i++)
        {
            for (int j = 0; j < sequence.Count; j++)
            {
                // Calculate the end index of the current block
                int endIndex = (i + 1) * sequence.Count;
                // Calculate the index of the jth bit from the end; 
                int bitIndex = endIndex - j - 1;

                try
                {
                    var bit = plaintextBits[bitIndex];
                    //Console.WriteLine($"j={j} : idx={rIndex} : bit={(bit ? 1 : 0)} | ");
                    Console.WriteLine($"j={j} : idx={bitIndex} : bit={(bit ? 1 : 0)} : ");
                    if (bit)
                    {
                        Console.WriteLine($"+{sequence[j]} .");
                        ciphertext[i] += sequence[j];
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    // thrown at the last block when the sequence length exceeds the last plaintext block
                    Console.WriteLine(nameof(ArgumentOutOfRangeException) + " " + bitIndex);
                    break;
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine("Ciphertext Elements:");
        Array.ForEach(ciphertext, x => Console.WriteLine(x));

        var ciphertextBytes = ciphertext.SelectMany(c => c.ToByteArray()).ToArray(); 
        // Convert long[] to byte[]
        //var ciphertextBytes = new byte[ciphertextBlocks * sizeof(long)];
        //Console.WriteLine($" IS_EQUAL: {ciphertextBytes.Length == Buffer.ByteLength(ciphertext)}");
        //Buffer.BlockCopy(ciphertext, 0, ciphertextBytes, 0, ciphertextBytes.Length);
        return ciphertextBytes;
    }


    public byte[] Decrypt(byte[] ciphertext)
    {
        // Convert byte[] back to long[]
        //long[] cipherTextInt = new long[ciphertext.Length / sizeof(long)];
        //Buffer.BlockCopy(ciphertext, 0, cipherTextInt, 0, ciphertext.Length);


        var sequence = PrivateKey.Sequence;
        int wInverse = ModularArithmetic.MultiplicativeInverse(PrivateKey.W, PrivateKey.M);
        Console.WriteLine($"wInverse: {wInverse}");

        var plaintextBits = new BitArray(ciphertext.Length * PrivateKey.Sequence.Count);
        //var plaintextBits = new BitArray(ciphertextBlockSize * sequence.Count);
        BigInteger[] ciphertextInts = new BigInteger[ciphertext.Length];
        for (int i = 0; i < ciphertextInts.Length; i++)
        {
            var ciphertextElement = (wInverse * ciphertextInts[i]) % PrivateKey.M;

            for (int j = PrivateKey.Sequence.Count - 1; j >= 0; j--)
            {
                if (PrivateKey.Sequence[j] <= ciphertextElement)
                {
                    // Calculate the end index of the current block
                    int endIndex = (i + 1) * sequence.Count;
                    // Calculate the index of the jth bit from the end; 
                    int bitIndex = endIndex - j - 1;

                    //Console.WriteLine($"Ciphertext element: {ciphertextElement}");
                    plaintextBits[Idx.Get(i, j, PrivateKey.Sequence.Count)] = true;
                    ciphertextElement -= PrivateKey.Sequence[j];
                    //Console.WriteLine($"    Minus {PrivateKey.Sequence[j]}. element: {ciphertextElement}");
                }
            }
        }

        // Copy BitArray data to byte[] array and return
        var (quotient1, remainder1) = Math.DivRem(plaintextBits.Length, BitsPerByte);
        byte[] bytes = new byte[quotient1 + (remainder1 == 0 ? 0 : 1)];
        plaintextBits.CopyTo(bytes, 0);
        return bytes;
    }

    public readonly record struct PublicKeyInfo(IReadOnlyList<int> Sequence);

    private readonly record struct PrivateKeyInfo(IReadOnlyList<int> Sequence, int M, int W);

    public static class RandomGenerators
    {
        public static IReadOnlyList<int> SuperincreasingSequence(int length, int toExclusive)
        {
            int sum = 0;
            List<int> sequence = new(length);
            for (int i = 0; i < length; i++)
            {
                int element = sum 
                    + System.Security.Cryptography.RandomNumberGenerator.GetInt32(1, toExclusive);
                sequence.Add(element);
                sum += element;
            }
            return sequence.AsReadOnly();
        }

        public static int IntegerGreaterThan(int sum, int toExclusive) => 
            sum 
            + System.Security.Cryptography.RandomNumberGenerator.GetInt32(1, 255);

        public static int IntegerCoprimeTo(int number, int toExclusive)
        {
            int result = System.Security.Cryptography.RandomNumberGenerator.GetInt32(1, toExclusive);
            while (MathHelpers.Gcd(number, result) != 1)
            {
                result = System.Security.Cryptography.RandomNumberGenerator.GetInt32(1, toExclusive);
            }
            return result;
        }
    }
}

file static class Idx
{
    public static Index Forward(int i, int j, int count) => (i * count + j);

    public static Index Backward(int i, int j, int count) => ^(i * count + j + 1);

    public static Index ForwardBigEndian(int i, int j, int count)
    {
        int forward = (i * count + j);
        return 8 * (forward / 8 + 1) - (forward % 8) - 1;
    }

    public static Index Test(int i, int j, int count)
    {
       int end = (i + 1) * count;
        return end - j - 1;
    }

    public static Func<int, int, int, Index> Get => Test;
}
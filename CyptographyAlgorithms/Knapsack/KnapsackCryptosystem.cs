using CyptographyAlgorithms.Extensions;
using System.Collections;
using System.Numerics;

namespace CyptographyAlgorithms.Knapsack;

public class KnapsackCryptosystem
{
    private const int _bitsPerByte = 8;

    private readonly int _blockSize;
    private readonly BigInteger _wInverse;
    private readonly BigInteger _privateSequenceSum;
    private readonly BigInteger _publicSequenceSum;
    
    public PublicKeyInfo PublicKey { get; }
    public PrivateKeyInfo PrivateKey { get; }

    private KnapsackCryptosystem(
        PublicKeyInfo publicKey,
        PrivateKeyInfo privateKey,
        BigInteger privateSequenceSum,
        BigInteger publicSequenceSum)
    {
        PublicKey = publicKey;
        PrivateKey = privateKey;
        _privateSequenceSum = privateSequenceSum;
        _publicSequenceSum = publicSequenceSum;
        _blockSize = publicSequenceSum.GetByteCount();
        _wInverse = ModularArithmetic.MultiplicativeInverse(PrivateKey.W, PrivateKey.M);

        Console.WriteLine($"Block Size {_blockSize}");
    }

    public static KnapsackCryptosystem Create(
        IReadOnlyList<BigInteger> sequence,
        BigInteger m,
        BigInteger w)
    {
        if (sequence.Count == 0)
        {
            throw new ArgumentException(
                "Sequence must be non-empty.",
                nameof(sequence));
        }

        BigInteger privateSequenceSum = sequence[0];
        for (int i = 1; i < sequence.Count; i++)
        {
            if (sequence[i] <= privateSequenceSum)
            {
                throw new ArgumentException(
                    "Not a superincreasing sequence.",
                    nameof(sequence));
            }
            privateSequenceSum += sequence[i];
        }
        if (m <= privateSequenceSum)
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

        IReadOnlyList<BigInteger> publicSequence = GeneratePublicSequence(sequence, m, w);
        var publicSequenceSum = publicSequence.Aggregate(BigInteger.Add);

        return new KnapsackCryptosystem(
            new PublicKeyInfo(publicSequence),
            new PrivateKeyInfo(sequence, m, w),
            privateSequenceSum,
            publicSequenceSum
        );
    }

    private static IReadOnlyList<BigInteger> GeneratePublicSequence(
        IReadOnlyList<BigInteger> sequence, BigInteger m, BigInteger w) => (
            from item in sequence
            select (w * item) % m)
            .ToList()
            .AsReadOnly();    // Multiply each item in the sequence by w and mod by m

    public byte[] Encrypt(byte[] plaintext)
    {
        var sequence = PublicKey.Sequence;
        var (quotient, remainder) = Math.DivRem(plaintext.Length * _bitsPerByte, sequence.Count);

        // Add an extra block to accomodate the rest of the bits
        int ciphertextBlockCount = quotient + 1;

        BigInteger[] ciphertext = new BigInteger[ciphertextBlockCount];

        byte[] ciphertextBytes = new byte[ciphertextBlockCount * _blockSize];

        #region Calculate Padding 
        // int paddingSize = (sequence.Count / _bitsPerByte) + 1;
        //var (paddingByteIndex, paddingBitIndex) = Math.DivRem(remainder, _bitsPerByte);
        #endregion

        for (int i = 0; i < ciphertextBlockCount; i++)
        {
            for (int j = 0; j < sequence.Count; j++)
            {
                // Little-endian, MSB first indexing
                int bitIndexInPlaintext = (i + 1) * sequence.Count - j - 1; // OR = ((ciphertextBlockCount - 1 - i) * sequence.Count) + (sequence.Count - 1 - j);

                var (byteIndexInPlaintext, bitIndexInByte) = BitManipulation.GetBitAndByteIndexes(
                    bitIndexInPlaintext,
                    plaintext.Length,
                    Endianness.BigEndian,
                    BitNumbering.Lsb0);

                int bit = byteIndexInPlaintext < plaintext.Length && byteIndexInPlaintext > -1
                    ? (plaintext[byteIndexInPlaintext] >> bitIndexInByte) & 1
                    : 0;

                if (bit == 1)
                {
                    ciphertext[i] += sequence[j];
                }
            }

            // Shorter blocks will be zero-padded in TryWriteBytes below
            if (ciphertext[i].GetByteCount() != _blockSize)
            {
                Console.WriteLine("Unmatching lengths. Index {2}. Block Size {0}. Byte Count {1}", _blockSize, ciphertext[i].GetByteCount(), i);
            }
            Console.WriteLine($"Ciphertext block {i}: {ciphertext[i]}");

            ciphertext[i].TryWriteBytes(
                ciphertextBytes.AsSpan(i * _blockSize, _blockSize),
                out int bytesWritten);

            Console.WriteLine($"Bytes Written: {bytesWritten}");
        }
        return ciphertextBytes;
    }

    public byte[] Decrypt(byte[] ciphertext)
    {
        var sequence = PrivateKey.Sequence;
        int blockCount = ciphertext.Length / _blockSize;

        BigInteger[] ciphertextInts = new BigInteger[blockCount];
        byte[] plaintextBytes = new byte[blockCount * _blockSize];
        for (int i = 0; i < blockCount; i++)
        {
            int startIndex = i * _blockSize;
            var ciphertextBlock = new BigInteger(ciphertext.AsSpan(startIndex, _blockSize));
            ciphertextInts[i] = (ciphertextBlock * _wInverse) % PrivateKey.M;

            for (int j = sequence.Count - 1; j >= 0; j--)
            {
                if (ciphertextInts[i] >= sequence[j])
                {
                    int bitIndexInPlaintext = (i + 1) * sequence.Count - j - 1; 

                    var (byteIndexInPlaintext, bitIndexInByte) = BitManipulation.GetBitAndByteIndexes(
                        bitIndexInPlaintext,
                        plaintextBytes.Length,
                        Endianness.BigEndian,
                        BitNumbering.Lsb0);

                    if (byteIndexInPlaintext > -1 && byteIndexInPlaintext < plaintextBytes.Length)
                    {
                        plaintextBytes[byteIndexInPlaintext] |= (byte)(1 << bitIndexInByte);
                        ciphertextInts[i] -= sequence[j];
                    }
                }
            }
        }
        return plaintextBytes;
    }

    [Obsolete]
    private byte[] EncryptBitArray(byte[] plaintext)
    {
        var plaintextBits = new BitArray(plaintext);
        var sequence = PublicKey.Sequence;

        // Calculate number of ciphertext blocks:
        // Dividing the length of the sequence by the bit length of the ciphertextInts gives
        // the number of ciphertext blocks. If the ciphertextInts bit length is not a multiple of 
        // the sequence length (remainder != 0), an extra block is required.
        var (quotient, remainder) = Math.DivRem(plaintextBits.Count, sequence.Count);
        var ciphertextBlockCount = quotient + (remainder == 0 ? 0 : 1);

        var ciphertext = new BigInteger[ciphertextBlockCount];

        // Apply the sequence to each ciphertextInts block
        for (int i = 0; i < ciphertextBlockCount; i++)
        {
            for (int j = 0; j < sequence.Count; j++)
            {
                // Calculate the end index of the current block
                int endIndex = (i + 1) * sequence.Count;
                // Calculate the index of the jth bit from the end; 
                int bitIndex = endIndex - j - 1;

                try
                {
                    var bit = plaintextBits[Idx.Get(i, j, sequence.Count)];
                    //Console.WriteLine($"j={j} : idx={bitIndex} : bit={(bit ? 1 : 0)} : ");
                    if (bit)
                    {
                        //    Console.WriteLine($"+{sequence[j]} .");
                        ciphertext[i] += sequence[j];
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    // thrown at the last block when the sequence length exceeds the last ciphertextInts block
                    Console.WriteLine(nameof(ArgumentOutOfRangeException) + " " + bitIndex);
                    break;
                }
                if (ciphertext[i].GetByteCount() != _blockSize)
                {
                    byte[] temp = new byte[_blockSize];
                    ciphertext[i].TryWriteBytes(temp, out int tempBytesWritten);
                    ciphertext[i] = new BigInteger(temp);
                }
            }
            Console.WriteLine();
        }

        //Console.WriteLine("Ciphertext Elements:");
        Array.ForEach(ciphertext, x => Console.WriteLine(x));

        var ciphertextBytes = ciphertext.SelectMany(c => c.ToByteArray()).ToArray();
        // Convert long[] to byte[]
        //var ciphertextBytes = new byte[ciphertextBlockCount * sizeof(long)];
        //Console.WriteLine($" IS_EQUAL: {ciphertextBytes.Length == Buffer.ByteLength(ciphertext)}");
        //Buffer.BlockCopy(ciphertext, 0, ciphertextBytes, 0, ciphertextBytes.Length);
        return ciphertextBytes;
    }


    [Obsolete]
    private byte[] DecryptBitArray(byte[] ciphertext)
    {
        // Convert byte[] back to long[]
        //long[] cipherTextInt = new long[ciphertext.Length / sizeof(long)];
        //Buffer.BlockCopy(ciphertext, 0, cipherTextInt, 0, ciphertext.Length);
        var sequence = PrivateKey.Sequence;

        //Console.WriteLine($"wInverse: {_wInverse}");

        int blockLength = ciphertext.Length / _blockSize;
        var plaintextBits = new BitArray(blockLength * _blockSize * _bitsPerByte);
        //var plaintextBits = new BitArray(ciphertextBlockSize * sequence.Count);
        BigInteger[] ciphertextInts = new BigInteger[blockLength];
        for (int i = 0; i < ciphertextInts.Length; i++)
        {
            int startIndex = i * _blockSize;
            ciphertextInts[i] = new BigInteger(ciphertext.AsSpan(startIndex, _blockSize));
        }
        for (int i = 0; i < ciphertextInts.Length; i++)
        {
            var ciphertextElement = (_wInverse * ciphertextInts[i]) % PrivateKey.M;

            for (int j = PrivateKey.Sequence.Count - 1; j >= 0; j--)
            {
                if (PrivateKey.Sequence[j] <= ciphertextElement)
                {
                    // Calculate the end index of the current block
                    int endIndex = (i + 1) * sequence.Count;
                    // Calculate the index of the jth bit from the end; 
                    int bitIndex = endIndex - j - 1;

                    //Console.WriteLine($"Ciphertext element: {ciphertextElement}");
                    plaintextBits[bitIndex] = true;
                    ciphertextElement -= PrivateKey.Sequence[j];
                    //Console.WriteLine($"    Minus {PrivateKey.Sequence[j]}. element: {ciphertextElement}");
                }
            }
        }

        // Copy BitArray data to byte[] array and return
        var (quotient1, remainder1) = Math.DivRem(plaintextBits.Length, _bitsPerByte);
        byte[] bytes = new byte[quotient1 + (remainder1 == 0 ? 0 : 1)];
        plaintextBits.CopyTo(bytes, 0);
        return bytes;
    }

    public readonly record struct PublicKeyInfo(IReadOnlyList<BigInteger> Sequence);

    public readonly record struct PrivateKeyInfo(
        IReadOnlyList<BigInteger> Sequence,
        BigInteger M,
        BigInteger W);
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

    public static Func<int, int, int, Index> Get => ForwardBigEndian;
}


//int blockIndex = (_endianness is Endianness.BigEndian
//                        ? i
//                        : ciphertextBlockCount - 1 - i)
//                    * sequence.Count;

//int bitIndexInBlock = _bitNumbering is BitNumbering.Lsb0
//   ? blockIndex + j
//   : blockIndex + (sequence.Count - 1 - j);
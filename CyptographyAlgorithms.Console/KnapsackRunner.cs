using CyptographyAlgorithms.Extensions;
using CyptographyAlgorithms.Knapsack;
using System.Buffers.Binary;
using System.Collections;
using System.Numerics;
using System.Text;

namespace CyptographyAlgorithms.Console;

public class KnapsackRunner : ICryptoAlgorithmRunner
{
    private record class KnapsackInput(
        IReadOnlyList<int> Seq,
        int M,
        int W,
        byte[] Plaintext,
        Func<byte[], string> BytesToString);

    public static void Run()
    {
        var inputWikipedia = new KnapsackInput(
            new[] { 2, 7, 11, 21, 42, 89, 180, 354 },
            M: 881,
            W: 588,
            Plaintext: BitConverter.GetBytes(97L),
            BytesToString: x => new BigInteger(x).ToString());

        var inputNBK = new KnapsackInput(
            Seq: new[] { 2, 5, 9, 21, 45, 103, 215, 450, 946 },
            M: 2003,
            W: 1289,
            Plaintext: new BigInteger(359).ToByteArray() /*new byte[] { 0b10110011, 0b10000000 }*/,
            BytesToString: x => new BigInteger(x).ToString() /*BitConverter.ToInt64(x).ToString()*/);

        var input1 = new KnapsackInput(
            Seq: new[] { 1, 2, 4, 10, 20, 40 },
            M: 120,
            W: 53,
            Plaintext: new byte[] { 0b11101010, 0b11011110, 0b01 },
            BytesToString: x => BitConverter.ToInt64(x).ToString());

        var inputTxt = new KnapsackInput(
            Seq: inputNBK.Seq,
            M: 2003,
            W: 1289,
            Plaintext: Encoding.Default.GetBytes("Hello"),
            BytesToString: Encoding.Default.GetString
            );

        RunKnapsack(inputNBK);
    }


    static void RunKnapsack(KnapsackInput input)
    {

        var knapsack = KnapsackCryptosystem.Create(
            sequence: input.Seq,
            m: input.M,
            w: input.W);

        System.Console.WriteLine("Public Sequence: ");
        Array.ForEach(knapsack.PublicKey.Sequence.ToArray(), x => System.Console.Write($"{x} "));
        System.Console.WriteLine();

        var plaintext = input.Plaintext;
        System.Console.WriteLine($"Plaintext Bits: {plaintext.ToBitString()}");
        var temp = new BitArray(plaintext);
        System.Console.WriteLine($"Plaintext Bits (BitArray access order): {temp.ToBitString()}");
        System.Console.Write("Plaintext Bits: ");
        for (int i = 0; i < temp.Count; i++)
        {
            System.Console.Write((temp[i] ? 1 : 0) + ((i + 1) % 8 == 0 ? " " : string.Empty));
        }
        System.Console.WriteLine();
        var enc = knapsack.Encrypt(plaintext);
        System.Console.WriteLine($"Ciphertext Bits: {enc.ToBitString()}");
        //System.Console.WriteLine($"Ciphertext: {BinaryPrimitives.ReadInt64LittleEndian(enc)}");

        var dec = knapsack.Decrypt(enc);
        System.Console.WriteLine($"Decrypted Plaintext Bits: {dec.ToBitString()}");

        System.Console.WriteLine($"Decrypted Plaintext: {input.BytesToString(dec)}");
    }

    static void RunKnapsackRandom()
    {
        var encoding = System.Console.OutputEncoding;
        System.Console.OutputEncoding = Encoding.UTF8;
        var superincreasingSequence = KnapsackCryptosystem.RandomGenerators.SuperincreasingSequence(length: 2, 10);
        var m = KnapsackCryptosystem.RandomGenerators.IntegerGreaterThan(superincreasingSequence.Sum(), 100);
        var w = KnapsackCryptosystem.RandomGenerators.IntegerCoprimeTo(m, 1000);
    }

}

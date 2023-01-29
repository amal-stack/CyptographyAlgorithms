using CyptographyAlgorithms.Extensions;
using CyptographyAlgorithms.Knapsack;
using System.Numerics;
using System.Text;

namespace CyptographyAlgorithms.Console;

using Console = System.Console;

[Demonstrates<KnapsackCryptosystem>]
public class KnapsackRunner : ICryptoAlgorithmRunner
{
    private record class KnapsackInput(
        IReadOnlyList<BigInteger> Seq,
        BigInteger M,
        BigInteger W,
        byte[] Plaintext,
        Func<byte[], string> BytesToString);

    public static void Run()
    {
        var inputWikipedia = new KnapsackInput(
            new BigInteger[] { 2, 7, 11, 21, 42, 89, 180, 354 },
            M: 881,
            W: 588,
            Plaintext: BitConverter.GetBytes(97),
            BytesToString: x => new BigInteger(x).ToString());

        var inputNBK = new KnapsackInput(
            Seq: new BigInteger[] { 2, 5, 9, 21, 45, 103, 215, 450, 946 },
            M: 2003,
            W: 1289,
            Plaintext: BitConverter.GetBytes(359), /*new byte[] { 0b10110011, 0b10000000 }*/
            BytesToString: x => new BigInteger(x).ToString() /*BitConverter.ToInt64(x).ToString()*/);

        var input1 = new KnapsackInput(
            Seq: new BigInteger[] { 1, 2, 4, 10, 20, 40 },
            M: 120,
            W: 53,
            Plaintext: new byte[] { 0b11101010, 0b11011110, 0b01 }, //122602
            BytesToString: x => new BigInteger(x).ToString());

        var inputTxt = new KnapsackInput(
            Seq: inputNBK.Seq,
            M: 2003,
            W: 1289,
            Plaintext: Encoding.Default.GetBytes("Hello! How are you?"),
            BytesToString: Encoding.Default.GetString
            );

        var inputBK = new KnapsackInput(
            Seq: new BigInteger[] { 1, 2, 4, 9 },
            M: 17,
            W: 15,
            Plaintext: new byte[] { 0b0100_1011, 0b1010_0101 },
            BytesToString: x => new BigInteger(x).ToString()
        );

        var input2 = new KnapsackInput(
            Seq: new BigInteger[] { 2, 3, 7, 14, 30, 57, 120, 251 },
            M: 491,
            W: 41,
            Plaintext: new BigInteger(150).ToByteArray(),
            BytesToString: x => new BigInteger(x).ToString()
        ); 

        var inputs = new[] { inputNBK, inputWikipedia, input1, inputTxt, inputBK, input2 };

        Array.ForEach(inputs, input => { 
            RunKnapsack(input); 
            Console.WriteLine(new string('-', 128)); 
        });
    }


    static void RunKnapsack(KnapsackInput input)
    {
        var knapsack = KnapsackCryptosystem.Create(
            sequence: input.Seq,
            m: input.M,
            w: input.W);

        Console.WriteLine("Public Sequence: ");
        Array.ForEach(knapsack.PublicKey.Sequence.ToArray(), x => Console.Write($"{x} "));
        Console.WriteLine();

        var plaintext = input.Plaintext;
        Console.WriteLine($"Plaintext Bits: {plaintext.ToBitString()}");

        //var temp = new BitArray(plaintext);
        //Console.WriteLine($"Plaintext Bits (BitArray access order): {temp.ToBitString()}");
        //Console.Write("Plaintext Bits: ");
        //for (int i = 0; i < temp.Count; i++)
        //{
        //    Console.Write((temp[i] ? 1 : 0) + ((i + 1) % 8 == 0 ? " " : string.Empty));
        //}

        Console.WriteLine();
        var enc = knapsack.Encrypt(plaintext);
        Console.WriteLine($"Ciphertext Bits: {enc.ToBitString()}");

        var dec = knapsack.Decrypt(enc);
        Console.WriteLine($"Decrypted Plaintext Bits: {dec.ToBitString()}");

        Console.WriteLine($"Decrypted Plaintext: {input.BytesToString(dec)}");
    }

    static KnapsackCryptosystem RunKnapsackRandom()
    {
        var encoding = Console.OutputEncoding;
        Console.OutputEncoding = Encoding.UTF8;
        var superincreasingSequence = RandomGenerators.SuperincreasingSequence(length: 100, 1000);
        var m = RandomGenerators.IntegerGreaterThan(superincreasingSequence.Aggregate(BigInteger.Add), 100);
        var w = RandomGenerators.IntegerCoprimeTo(m, 1000);
        return KnapsackCryptosystem.Create(superincreasingSequence, m, w);
    }

}

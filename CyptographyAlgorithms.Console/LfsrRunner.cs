using CyptographyAlgorithms.Extensions;
using CyptographyAlgorithms.Stream;

namespace CyptographyAlgorithms.Console;

[Demonstrates<Lfsr>]
public class LfsrRunner : ICryptoAlgorithmRunner
{
    public static void RunLfsr()
    {

        System.Console.WriteLine("Linear-feedback shift register (LFSR) implementation");
        System.Console.Write("Enter the size of LFSR> ");
        int size = int.Parse(System.Console.ReadLine()!);

        System.Console.Write("Enter taps (positions of XOR bits) as space-separated integers> ");
        int[] taps = System.Console.ReadLine()!
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        System.Console.WriteLine("Enter initialization vector base");
        System.Console.WriteLine("1. Binary");
        System.Console.WriteLine("2. Decimal");
        System.Console.WriteLine("3. Hex");
        System.Console.Write("> ");
        Radix radix = int.Parse(System.Console.ReadLine()!) switch
        {
            3 => Radix.Hex,
            2 => Radix.Decimal,
            _ => Radix.Binary,
        };

        System.Console.Write("Enter initialization vector/seed> ");
        InitializationVector iv = InitializationVector.FromBytes(
            radix switch
            {
                Radix.Hex => Convert.FromHexString(System.Console.ReadLine()!),
                Radix.Binary => BitConverter.GetBytes(Convert.ToInt32(System.Console.ReadLine()!, 2)),
                _ => BitConverter.GetBytes(int.Parse(System.Console.ReadLine()!)),
            });

        var lfsr = new Lfsr(size, taps);
        System.Console.WriteLine(lfsr);

        System.Console.WriteLine($"Initialization Vector: {iv.Value.ToBitString()}.");

        lfsr.Initialize(iv);

        bool @continue = true;
        while (@continue)
        {
            System.Console.WriteLine("Enter a choice");
            System.Console.WriteLine("1. Display first n iterations.");
            System.Console.WriteLine("2. Generate first n bits of the key stream.");
            System.Console.WriteLine("3. Calculate period.");
            System.Console.Write("> ");
            int choice = int.Parse(System.Console.ReadLine()!);
            switch (choice)
            {
                case 3:
                    int period = lfsr.CalculatePeriod();
                    System.Console.WriteLine($"The period is {period}.");
                    break;
                case 2:
                    System.Console.Write("Enter number of bits (n)> ");
                    int bitCount = int.Parse(System.Console.ReadLine()!);
                    lfsr.Reset();
                    var keyStream = lfsr.GetEnumerator();
                    for (int i = 0; i < bitCount; i++)
                    {
                        keyStream.MoveNext();
                        System.Console.Write(keyStream.Current ? 1 : 0);
                    }
                    System.Console.WriteLine();
                    lfsr.Reset();
                    break;
                case 1:
                    System.Console.Write("Enter number of iterations (n)> ");
                    int iters = int.Parse(System.Console.ReadLine()!);
                    lfsr.Reset();
                    System.Console.WriteLine(lfsr);
                    for (int i = 0; i < iters; i++)
                    {
                        System.Console.WriteLine($"# {i + 1}");
                        lfsr.Next();
                        System.Console.WriteLine(lfsr);
                    }
                    break;
                default:
                    @continue = false;
                    break;
            }
            System.Console.WriteLine();
        }
    }
}



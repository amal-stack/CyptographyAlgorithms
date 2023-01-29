using CyptographyAlgorithms.Shift;

namespace CyptographyAlgorithms.Console;

using Console = System.Console;

[Demonstrates<ShiftCipher>]
public class ShiftCipherRunner : ICryptoAlgorithmRunner
{
    public static void Run()
    {
        //Console.WriteLine("1. Encipher using Caesar cipher");
        //Console.WriteLine("2. Decipher using Caesar cipher");
        //Console.WriteLine("3. Brute Force Caesar cipher");
        //Console.Write("Enter a choice: ");
        //int input = int.Parse(Console.ReadLine()!);


        Console.Write("Enter plaintext: ");
        string plaintext = Console.ReadLine()!;

        Console.Write("Enter shift value: ");
        int shiftValue = int.Parse(Console.ReadLine()!);

        ICipher cipher = new ShiftCipher(shift: shiftValue);
        string ciphertext = cipher.Encipher(plaintext);
        Console.WriteLine($"Ciphertext: {ciphertext}");
        Console.WriteLine($"Plaintext: {cipher.Decipher(ciphertext)}");
    }
    static void RunShiftCipherBruteForce()
    {

        Console.Write("Enter ciphertext: ");
        string ciphertext = Console.ReadLine()!;
        IBruteForceCipherBreaker<ShiftCipher, BruteForceResult> cipherBreaker
            = new BruteForceShiftCipherBreaker();

        Console.WriteLine();
        Console.WriteLine("All possible combinations:");
        Console.WriteLine(new string('=', 50));
        Console.WriteLine();

        foreach (var result in cipherBreaker.Generate(ciphertext))
        {
            Console.WriteLine($"Shift: {result.Shift,2} | Plaintext: {result.Plaintext}");
        }

    }
}

using CyptographyAlgorithms.Shift;

namespace CyptographyAlgorithms.Console;

[Demonstrates<ShiftCipher>]
public class ShiftCipherRunner : ICryptoAlgorithmRunner
{
    static void RunShiftCipher()
    {
        //Console.WriteLine("1. Encipher using Caesar cipher");
        //Console.WriteLine("2. Decipher using Caesar cipher");
        //Console.WriteLine("3. Brute Force Caesar cipher");
        //Console.Write("Enter a choice: ");
        //int input = int.Parse(Console.ReadLine()!);


        System.Console.Write("Enter plaintext: ");
        string plaintext = System.Console.ReadLine()!;

        System.Console.Write("Enter shift value: ");
        int shiftValue = int.Parse(System.Console.ReadLine()!);

        ICipher cipher = new ShiftCipher(shift: shiftValue);
        string ciphertext = cipher.Encipher(plaintext);
        System.Console.WriteLine($"Ciphertext: {ciphertext}");
        System.Console.WriteLine($"Plaintext: {cipher.Decipher(ciphertext)}");
    }
    static void RunShiftCipherBruteForce()
    {

        System.Console.Write("Enter ciphertext: ");
        string ciphertext = System.Console.ReadLine()!;
        IBruteForceCipherBreaker<ShiftCipher, BruteForceResult> cipherBreaker
            = new BruteForceShiftCipherBreaker();

        System.Console.WriteLine();
        System.Console.WriteLine("All possible combinations:");
        System.Console.WriteLine(new string('=', 50));
        System.Console.WriteLine();

        foreach (var result in cipherBreaker.Generate(ciphertext))
        {
            System.Console.WriteLine($"Shift: {result.Shift,2} | Plaintext: {result.Plaintext}");
        }

    }
}

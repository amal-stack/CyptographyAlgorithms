using CyptographyAlgorithms.Substitution;

namespace CyptographyAlgorithms.Console;

public class MonoalphabeticSubtitutionCipherRunner : ICryptoAlgorithmRunner
{
    static void RunMonoalphabeticSubstitutionCipher()
    {
        System.Console.Write("Generate random permutation table? [y/n]> ");
        bool random = System.Console.ReadLine()!.Trim().ToLower() == "y";

        PermutationTable table;
        if (random)
        {
            table = PermutationTable.GenerateRandom();
        }
        else
        {
            System.Console.WriteLine("Enter a permutation of the alphabet:");
            string permutation = System.Console.ReadLine()!;
            table = PermutationTable.FromPermutationString(permutation);
        }

        System.Console.Write("Enter plaintext: ");
        string plaintext = System.Console.ReadLine()!;
        System.Console.WriteLine("Using the following permutation table:");
        System.Console.WriteLine(table);

        ICipher cipher = new MonoalphabeticSubstitutionCipher(table);
        string ciphertext = cipher.Encipher(plaintext);
        System.Console.WriteLine($"Ciphertext: {ciphertext}");

        System.Console.WriteLine("Deciphered text:");
        System.Console.WriteLine($"Plaintext: {cipher.Decipher(ciphertext)}");
    }
}

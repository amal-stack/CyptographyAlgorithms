using CyptographyAlgorithms;


//Console.WriteLine("1. Encipher using Caesar cipher");
//Console.WriteLine("2. Decipher using Caesar cipher");
//Console.WriteLine("2. Brute Force Caesar cipher");
//Console.Write("Enter a choice: ");
//int input = int.Parse(Console.ReadLine()!);
//switch (input)
//{
//    case 1:
//        Console.WriteLine("Enter plaintext:");
//        string plaintext = Console.ReadLine();
//}


//ICipherBreaker<ShiftCipher> bruteForceBreaker = new BruteForceCipherBreaker();
//bruteForceBreaker.BreakCipher(ciphertext, Console.WriteLine);


//{
//    Console.Write("Enter plaintext: ");
//    string plaintext = Console.ReadLine()!;

//    Console.Write("Enter shift value: ");
//    int shiftValue = int.Parse(Console.ReadLine()!);

//    ICipher cipher = new ShiftCipher(shift: shiftValue);
//    string ciphertext = cipher.Encipher(plaintext);
//    Console.WriteLine($"Ciphertext: {ciphertext}");
//    Console.WriteLine($"Plaintext: {cipher.Decipher(ciphertext)}");
//}
{

    Console.Write("Enter ciphertext: ");
    string ciphertext = Console.ReadLine()!;
    IBruteForceCipherBreaker<ShiftCipher, BruteForceResult> cipherBreaker 
        = new BruteForceCipherBreaker();

    Console.WriteLine();
    Console.WriteLine("All possible combinations:");
    Console.WriteLine(new string('=', 50));
    Console.WriteLine();

    foreach (var result in cipherBreaker.Generate(ciphertext))
    {
        Console.WriteLine($"Shift: {result.Shift, 2} | Plaintext: {result.Plaintext}");
    }

}



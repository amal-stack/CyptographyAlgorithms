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
//{

//    Console.Write("Enter ciphertext: ");
//    string ciphertext = Console.ReadLine()!;
//    IBruteForceCipherBreaker<ShiftCipher, BruteForceResult> cipherBreaker 
//        = new BruteForceCipherBreaker();

//    Console.WriteLine();
//    Console.WriteLine("All possible combinations:");
//    Console.WriteLine(new string('=', 50));
//    Console.WriteLine();

//    foreach (var result in cipherBreaker.Generate(ciphertext))
//    {
//        Console.WriteLine($"Shift: {result.Shift, 2} | Plaintext: {result.Plaintext}");
//    }

//}

RunLfsr();

void RunLfsr()
{
    Console.WriteLine("Linear-feedback shift register (LFSR) implementation");
    Console.Write("Enter the size of LFSR> ");
    int size = int.Parse(Console.ReadLine()!);

    Console.Write("Enter taps (positions of XOR bits) as space-separated integers> ");
    int[] taps = Console.ReadLine()!
        .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToArray();

    Console.WriteLine("Enter initialization vector base");
    Console.WriteLine("1. Binary");
    Console.WriteLine("2. Decimal");
    Console.WriteLine("3. Hex");
    Console.Write("> ");
    Radix radix = int.Parse(Console.ReadLine()!) switch
    {
        3 => Radix.Hex,
        2 => Radix.Decimal,
        _ => Radix.Binary,
    };

    Console.Write("Enter initialization vector/seed> ");
    InitializationVector iv = InitializationVector.FromBytes(
        radix switch
        {
            Radix.Hex => Convert.FromHexString(Console.ReadLine()!),
            Radix.Binary => BitConverter.GetBytes(Convert.ToInt32(Console.ReadLine()!, 2)),
            _ => BitConverter.GetBytes(int.Parse(Console.ReadLine()!)),
        });

    var lfsr = new Lfsr(size, taps);
    Console.WriteLine(lfsr);

    Console.WriteLine($"Initialization Vector: {iv.Value.ToBitString()}.");

    lfsr.Initialize(iv);

    bool @continue = true;
    while (@continue)
    {
        Console.WriteLine("Enter a choice");
        Console.WriteLine("1. Display first n iterations.");
        Console.WriteLine("2. Generate first n bits of the key stream.");
        Console.WriteLine("3. Calculate period.");
        Console.Write("> ");
        int choice = int.Parse(Console.ReadLine()!);
        switch (choice)
        {
            case 3:
                int period = lfsr.CalculatePeriod();
                Console.WriteLine($"The period is {period}.");
                break;
            case 2:
                Console.Write("Enter number of bits (n)> ");
                int bitCount = int.Parse(Console.ReadLine()!);
                lfsr.Reset();
                var keyStream = lfsr.GetEnumerator();
                for (int i = 0; i < bitCount; i++)
                {
                    keyStream.MoveNext();
                    Console.Write(keyStream.Current ? 1 : 0);
                }
                Console.WriteLine();
                lfsr.Reset();
                break;
            case 1:
                Console.Write("Enter number of iterations (n)> ");
                int iters = int.Parse(Console.ReadLine()!);
                lfsr.Reset();
                Console.WriteLine(lfsr);
                for (int i = 0; i < iters; i++)
                {
                    Console.WriteLine($"# {i + 1}");
                    lfsr.Next();
                    Console.WriteLine(lfsr);
                }
                break;
            default:
                @continue = false;
                break;
        }
        Console.WriteLine();
    }
}

void RunMonoalphabeticSubstitutionCipher()
{
    Console.Write("Generate random permutation table? [y/n]> ");
    bool random = Console.ReadLine()!.Trim().ToLower() == "y";

    PermutationTable table;
    if (random)
    {
        table = PermutationTable.GenerateRandom();
    }
    else
    {
        Console.WriteLine("Enter a permutation of the alphabet:");
        string permutation = Console.ReadLine()!;
        table = PermutationTable.FromPermutationString(permutation);
    }

    Console.Write("Enter plaintext: ");
    string plaintext = Console.ReadLine()!;
    Console.WriteLine("Using the following permutation table:");
    Console.WriteLine(table);

    ICipher cipher = new MonoalphabeticSubstitutionCipher(table);
    string ciphertext = cipher.Encipher(plaintext);
    Console.WriteLine($"Ciphertext: {ciphertext}");

    Console.WriteLine("Deciphered text:");
    Console.WriteLine($"Plaintext: {cipher.Decipher(ciphertext)}");
}


enum Radix
{
    Binary = 2,
    Decimal = 10,
    Hex = 16
}
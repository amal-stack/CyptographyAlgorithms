using CyptographyAlgorithms.AdvancedEncryptionStandard;

namespace CyptographyAlgorithms.Console;

public class AesKeyScheduleRunner : ICryptoAlgorithmRunner
{
    public static void Run()
    {
        var input128 = new byte[]
        {
            0x2b, 0x7e, 0x15, 0x16,
            0x28, 0xae, 0xd2, 0xa6,
            0xab, 0xf7, 0x15, 0x88,
            0x09, 0xcf, 0x4f, 0x3c
        };

        var input192 = new byte[]
        {
            0x8e, 0x73, 0xb0, 0xf7,
            0xda, 0x0e, 0x64, 0x52,
            0xc8, 0x10, 0xf3, 0x2b,
            0x80, 0x90, 0x79, 0xe5,
            0x62, 0xf8, 0xea, 0xd2,
            0x52, 0x2c, 0x6b, 0x7b
        };

        var input256 = new byte[]
        {
            0x60, 0x3d, 0xeb, 0x10,
            0x15, 0xca, 0x71, 0xbe,
            0x2b, 0x73, 0xae, 0xf0,
            0x85, 0x7d, 0x77, 0x81,
            0x1f, 0x35, 0x2c, 0x07,
            0x3b, 0x61, 0x08, 0xd7,
            0x2d, 0x98, 0x10, 0xa3,
            0x09, 0x14, 0xdf, 0xf4
        };


        System.Console.WriteLine($"128 bits Example. Key: {Convert.ToHexString(input128).ToLowerInvariant()}");
        PrintRoundKeys(input128);

        System.Console.WriteLine($"192 bits Example. Key: {Convert.ToHexString(input192).ToLowerInvariant()}");
        PrintRoundKeys(input192);

        System.Console.WriteLine($"256 bits Example. Key: {Convert.ToHexString(input256).ToLowerInvariant()}");
        PrintRoundKeys(input256);

        System.Console.WriteLine("Custom");
        PrintRoundKeys(Convert.FromHexString("000102030405060708090a0b0c0d0e0f"));
    }

    static void PrintRoundKeys(byte[] input)
    {
        var aks = new AesKeySchedule(masterKey: input);
        int i = 0;
        foreach (var roundKey in aks.GetRoundKeys())
        {
            System.Console.WriteLine($"{++i}: {Convert.ToHexString(roundKey).ToLowerInvariant()}");
        }
        System.Console.WriteLine();
    }
}

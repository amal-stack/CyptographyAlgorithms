using CyptographyAlgorithms.Extensions;
using CyptographyAlgorithms.Spn;

namespace CyptographyAlgorithms.Console;

[Demonstrates<SPNetwork>]
public class SpnRunner : ICryptoAlgorithmRunner
{
    public static void Run()
    {
        SPNetwork spn = new SPNetworkBuilder()
            .OfRounds(5)
            .AddSBox(new SpnSbox())
            .AddPBox(new SpnPbox())
            .WithKey(new byte[] { 0b0011_1010, 0b1001_0100, 0b1101_0110, 0b0011_1111 })
            .GenerateRoundKeysUsing(new SubsetSpnKeySchedule
            {
                Start = i => 4 * i - 3 - 1,
                Length = 16
            }.GetKeySchedule())
            .LogTo(System.Console.WriteLine)
            .Build();

        var plaintext = new byte[] { 0b0010_0110, 0b1011_0111 };

        var ciphertext = spn.Encipher(plaintext);
        System.Console.WriteLine("Encrypted Ciphertext: {0}", ciphertext.ToBitString());

        var decryptedPlaintext = spn.Decipher(ciphertext);
        System.Console.WriteLine("Decrypted Plaintext: {0}", decryptedPlaintext.ToBitString());
    }
}

/*(k, i) =>
            {
                const int BitsPerByte = 8;
                int startBitIndex = 4 * i - 3;
                int endBitIndex = (startBitIndex + 16) % (k.Length * BitsPerByte);

                var (startByteIndex, startByteOffset) = Math.DivRem(startBitIndex, BitsPerByte);
                var (endByteIndex, endByteOffset) = Math.DivRem(endBitIndex, BitsPerByte);
                var (startBit, endBit) = (k[startByteIndex], k[endByteIndex]);
                byte[] roundKey = new byte[2]; // 16 bit round key

                roundKey[0] = (byte)(startBit >> startByteOffset & BitManipulation.GetMask(startByteOffset));
                roundKey[1] = (byte)(endBit >> endByteOffset & BitManipulation.GetMask(endByteOffset));
                return roundKey;
            }
*/
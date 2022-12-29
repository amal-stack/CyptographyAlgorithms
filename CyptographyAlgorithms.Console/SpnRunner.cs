using CyptographyAlgorithms.Extensions;
using CyptographyAlgorithms.Spn;

namespace CyptographyAlgorithms.Console;

public class SpnRunner : ICryptoAlgorithmRunner
{
    static void RunSpn()
    {
        new SPNetworkBuilder()
            .OfRounds(1)
            .WithKey(new byte[] { 0b0011_1010, 0b1001_0100, 0b1101_0110, 0b0011_1111 })
            .GenerateRoundKeysUsing((k, i) =>
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
            })
            ;
    }
}

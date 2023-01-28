using CyptographyAlgorithms.Extensions;

namespace CyptographyAlgorithms.Spn;

public class SubsetSpnKeySchedule
{
    public required int Length { get; init; }

    public required Func<int, int> Start { get; init; }

    public SpnKeySchedule GetKeySchedule() => GetRoundKey;

    private byte[] GetRoundKey(byte[] key, int roundNumber)
    {
        int startBit = Start.Invoke(roundNumber);
        int endBit = startBit + Length;
        var (byteLength, remainderLength) = Math.DivRem(Length, BitConstants.BitsPerByte);
        byte[] roundKey = new byte[byteLength + (remainderLength == 0 ? 0 : 1)];

        for (int i = 0; i < roundKey.Length && startBit < endBit; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var (byteIndex, bitOffset) = Math.DivRem(startBit, BitConstants.BitsPerByte);
                roundKey[i] |= (byte)(((key[byteIndex % roundKey.Length] >> bitOffset) & 1) << j);
                startBit++;
            }
        }
        
        return roundKey;
    }
}

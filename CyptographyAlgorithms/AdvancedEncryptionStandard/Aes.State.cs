using CyptographyAlgorithms.Extensions;

namespace CyptographyAlgorithms.AdvancedEncryptionStandard;



public sealed partial class Aes
{
    private static class State
    {
        internal static byte[,] Create(byte[] bytes)
        {
            byte[,] state = new byte[4, _columns];

            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < _columns; c++)
                {
                    state[r, c] = bytes[r + 4 * c];
                }
            }

            return state;
        }

        internal static byte[] Flatten(byte[,] state)
        {
            byte[] bytes = new byte[128 / BitConstants.BitsPerByte];
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < _columns; c++)
                {
                    bytes[r + 4 * c] = state[r, c];
                }
            }

            return bytes;
        }
    }
}
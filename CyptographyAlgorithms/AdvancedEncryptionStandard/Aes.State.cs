using CyptographyAlgorithms.Extensions;

namespace CyptographyAlgorithms.AdvancedEncryptionStandard;



public sealed partial class Aes
{
    private static class State
    {
        internal static byte[,] Create(byte[] plaintext)
        {
            byte[,] state = new byte[4, _blockSize];

            // Copy input into state
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < _blockSize; c++)
                {
                    state[r, c] = plaintext[r + 4 * c];
                }
            }

            return state;
        }

        internal static byte[] Flatten(byte[,] state)
        {
            byte[] ciphertext = new byte[128 / BitConstants.BitsPerByte];
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < _blockSize; c++)
                {
                    ciphertext[r + 4 * c] = state[r, c];
                }
            }

            return ciphertext;
        }
    }
}
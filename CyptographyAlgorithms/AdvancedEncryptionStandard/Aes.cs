using CyptographyAlgorithms.Extensions;

namespace CyptographyAlgorithms.AdvancedEncryptionStandard;

public sealed partial class Aes
{
    private const int _columns = 4;
    private const byte _irreduciblePolynomial = 0x1b;

    private readonly byte[] _key;

    public byte[] Key => _key;

    public Aes(byte[] key)
    {
        if (!IsValidKey(key))
        {
            throw new ArgumentException("Valid key lengths are 128, 192 and 256 bits", nameof(key));
        }
        _key = key;
    }

    private bool IsValidKey(byte[] key) => key is { Length: 16 or 24 or 32 };

    private void AddRoundKey(byte[,] state, Span<byte[]> roundKey)
    {
        for (int c = 0; c < _columns; c++)
        {
            for (int r = 0; r < 4; r++)
            {
                state[r, c] ^= roundKey[c][r];
            }
        }
    }

    private byte GF2Pow8Multiply(byte b1, byte b2)
    {
        byte product = 0;
        for (int i = 0; i < BitConstants.BitsPerByte; i++)
        {
            if (b2.GetBitAt(0))
            {
                product ^= b1;
            }
            bool msb = b1.GetBitAt(7);
            b1 <<= 1;
            if (msb)
            {
                b1 ^= _irreduciblePolynomial;
            }
            b2 >>= 1;
        }

        return product;
    }
}
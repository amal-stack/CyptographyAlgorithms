namespace CyptographyAlgorithms.AdvancedEncryptionStandard;

public sealed partial class Aes
{
    public byte[] Encrypt(byte[] plaintext)
    {
        byte[,] state = State.Create(plaintext);

        Log(0, state, "Input");

        // Generate Round Keys
        var keySchedule = new AesKeySchedule(Key);
        byte[][] roundKeys = keySchedule.GetRoundKeys();


        var roundKey = roundKeys.AsSpan(0.._blockSize);
        AddRoundKey(state, roundKey);
        Log(0, state, roundKey);

        for (int round = 1; round < keySchedule.Rounds; round++)
        {
            SubBytes(state);
            Log(round, state, nameof(SubBytes));

            ShiftRows(state);
            Log(round, state, nameof(ShiftRows));

            MixColumns(state);
            Log(round, state, nameof(MixColumns));

            roundKey = roundKeys.AsSpan(round * _blockSize, _blockSize);
            AddRoundKey(state, roundKey);
            Log(round, state, roundKey);

        }

        SubBytes(state);
        Log(keySchedule.Rounds, state, nameof(SubBytes));

        ShiftRows(state);
        Log(keySchedule.Rounds, state, nameof(ShiftRows));

        roundKey = roundKeys.AsSpan(^4..);
        AddRoundKey(state, roundKey);
        Log(keySchedule.Rounds, state, roundKey);

        // Copy state to 1D array
        byte[] ciphertext = State.Flatten(state);
        return ciphertext;
    }


    private void SubBytes(byte[,] state)
    {
        for (int r = 0; r < 4; r++)
        {
            for (int c = 0; c < _blockSize; c++)
            {
                state[r, c] = AesSbox.Get(state[r, c]);
            }
        }
    }

    private void ShiftRows(byte[,] state)
    {
        (state[1, 0], state[1, 1], state[1, 2], state[1, 3])
            = (state[1, 1], state[1, 2], state[1, 3], state[1, 0]);

        (state[2, 0], state[2, 1], state[2, 2], state[2, 3])
            = (state[2, 2], state[2, 3], state[2, 0], state[2, 1]);

        (state[3, 0], state[3, 1], state[3, 2], state[3, 3])
            = (state[3, 3], state[3, 0], state[3, 1], state[3, 2]);
    }

    private void MixColumns(byte[,] state)
    {

        byte[,] temp = new byte[4, _blockSize];
        for (int c = 0; c < _blockSize; c++)
        {
            temp[0, c] = (byte)(GF2Pow8Multiply(state[0, c], 0x02) ^ GF2Pow8Multiply(state[1, c], 0x03) ^ state[2, c] ^ state[3, c]);
            temp[1, c] = (byte)(state[0, c] ^ GF2Pow8Multiply(state[1, c], 0x02) ^ GF2Pow8Multiply(state[2, c], 0x03) ^ state[3, c]);
            temp[2, c] = (byte)(state[0, c] ^ state[1, c] ^ GF2Pow8Multiply(state[2, c], 0x02) ^ GF2Pow8Multiply(state[3, c], 0x03));
            temp[3, c] = (byte)(GF2Pow8Multiply(state[0, c], 0x03) ^ state[1, c] ^ state[2, c] ^ GF2Pow8Multiply(state[3, c], 0x02));
        }
        Array.Copy(temp, state, state.Length);
    }

}

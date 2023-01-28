namespace CyptographyAlgorithms.AdvancedEncryptionStandard;

public sealed partial class Aes
{
    public byte[] Decrypt(byte[] ciphertext)
    {
        byte[,] state = State.Create(ciphertext);
        // Generate Round Keys
        var keySchedule = new AesKeySchedule(Key);
        byte[][] roundKeys = keySchedule.GetRoundKeys();

        Log(0, state, "Input");

        var roundKey = roundKeys.AsSpan(^4..);
        AddRoundKey(state, roundKey);
        Log(0, state, roundKey);

        for (int round = keySchedule.Rounds - 1; round > 0; round--)
        {
            int increasingRound = keySchedule.Rounds - round;

            InverseShiftRows(state);
            Log(increasingRound, state, nameof(InverseShiftRows));

            InverseSubBytes(state);
            Log(increasingRound, state, nameof(InverseSubBytes));

            roundKey = roundKeys.AsSpan(round * _columns, _columns);
            AddRoundKey(state, roundKey);
            Log(increasingRound, state, roundKey);

            InverseMixColumns(state);
            Log(increasingRound, state, nameof(InverseMixColumns));

        }

        InverseShiftRows(state);
        Log(keySchedule.Rounds, state, nameof(InverseShiftRows));

        InverseSubBytes(state);
        Log(keySchedule.Rounds, state, nameof(InverseSubBytes));

        roundKey = roundKeys.AsSpan(.._columns);
        AddRoundKey(state, roundKey);
        Log(keySchedule.Rounds, state, roundKey);

        // Copy state to 1D array
        byte[] plaintext = State.Flatten(state);
        return plaintext;
    }


    private void InverseSubBytes(byte[,] state)
    {
        for (int r = 0; r < 4; r++)
        {
            for (int c = 0; c < _columns; c++)
            {
                state[r, c] = AesSbox.Inverse.Get(state[r, c]);
            }
        }
    }

    private void InverseShiftRows(byte[,] state)
    {
        (state[1, 1], state[1, 2], state[1, 3], state[1, 0])
            = (state[1, 0], state[1, 1], state[1, 2], state[1, 3]);

        (state[2, 2], state[2, 3], state[2, 0], state[2, 1])
            = (state[2, 0], state[2, 1], state[2, 2], state[2, 3]);

        (state[3, 3], state[3, 0], state[3, 1], state[3, 2])
            = (state[3, 0], state[3, 1], state[3, 2], state[3, 3]);
    }

    private void InverseMixColumns(byte[,] state)
    {
        byte[,] temp = new byte[4, _columns];
        for (int c = 0; c < _columns; c++)
        {
            temp[0, c] = (byte)(GF2Pow8Multiply(state[0, c], 0x0e) ^ GF2Pow8Multiply(state[1, c], 0x0b) ^ GF2Pow8Multiply(state[2, c], 0x0d) ^ GF2Pow8Multiply(state[3, c], 0x09));
            temp[1, c] = (byte)(GF2Pow8Multiply(state[0, c], 0x09) ^ GF2Pow8Multiply(state[1, c], 0x0e) ^ GF2Pow8Multiply(state[2, c], 0x0b) ^ GF2Pow8Multiply(state[3, c], 0x0d));
            temp[2, c] = (byte)(GF2Pow8Multiply(state[0, c], 0x0d) ^ GF2Pow8Multiply(state[1, c], 0x09) ^ GF2Pow8Multiply(state[2, c], 0x0e) ^ GF2Pow8Multiply(state[3, c], 0x0b));
            temp[3, c] = (byte)(GF2Pow8Multiply(state[0, c], 0x0b) ^ GF2Pow8Multiply(state[1, c], 0x0d) ^ GF2Pow8Multiply(state[2, c], 0x09) ^ GF2Pow8Multiply(state[3, c], 0x0e));
        }
        Array.Copy(temp, state, state.Length);
    }
}

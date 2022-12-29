using CyptographyAlgorithms.Extensions;

namespace CyptographyAlgorithms.Shift;

public class ShiftCipher : ICipher
{
    public ShiftCipher(int shift) => Shift = shift;

    public int Shift { get; }

    public string Encipher(string plaintext) => ShiftChars(plaintext, Shift);

    public string Decipher(string ciphertext) => ShiftChars(ciphertext, -Shift);

    private static string ShiftChars(IEnumerable<char> chars, int shift) => string.Concat(
        from c in chars
        select ShiftChar(c, shift)
    );

    private static char ShiftChar(char c, int shift) => c switch
    {
        _ when char.IsUpper(c) => Convert.ToChar(ModularArithmetic.Mod(c - 'A' + shift, 26) + 'A'),
        _ when char.IsLower(c) => Convert.ToChar(ModularArithmetic.Mod(c - 'a' + shift, 26) + 'a'),
        _ => c
    };
}

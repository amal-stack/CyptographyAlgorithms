namespace CyptographyAlgorithms.Shift;

public class BruteForceShiftCipherBreaker : IBruteForceCipherBreaker<ShiftCipher, BruteForceResult>
{
    public IEnumerable<BruteForceResult> Generate(string ciphertext)
    {
        foreach (var shiftValue in Enumerable.Range(0, 26))
        {
            yield return new BruteForceResult
            {
                Plaintext = new ShiftCipher(shift: shiftValue).Decipher(ciphertext),
                Shift = shiftValue
            };
        }
    }
}

public record struct BruteForceResult(
        string Plaintext,
        int Shift);

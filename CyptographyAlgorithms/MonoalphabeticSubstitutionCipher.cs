namespace CyptographyAlgorithms;

public class MonoalphabeticSubstitutionCipher : ICipher
{
    public MonoalphabeticSubstitutionCipher(PermutationTable table) => Table = table;

    public PermutationTable Table { get; }

    public string Encipher(string plaintext) => string.Concat(
        from c in plaintext
        select Table[c]
    );

    public string Decipher(string ciphertext) => string.Concat(
        from c in ciphertext
        select Table[c, inverse: true]
    );

}



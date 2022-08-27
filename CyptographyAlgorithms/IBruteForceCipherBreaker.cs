namespace CyptographyAlgorithms;

public interface IBruteForceCipherBreaker<TCipher, TResult> where TCipher : ICipher
{
    IEnumerable<TResult> Generate(string ciphertext);
}

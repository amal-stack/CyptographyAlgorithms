using CyptographyAlgorithms.Extensions;
using System.Collections;

namespace CyptographyAlgorithms;


public interface ICipher
{
    string Encipher(string plaintext);
    string Decipher(string ciphertext);
}

public interface ICipher<T>
{
    IReadOnlyList<T> Encipher(IReadOnlyList<T> plaintext);
    IReadOnlyList<T> Decipher(IReadOnlyList<T> ciphertext);
}

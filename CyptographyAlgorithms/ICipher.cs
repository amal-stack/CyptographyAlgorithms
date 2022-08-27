using System.Text;

namespace CyptographyAlgorithms;


public interface ICipher
{
    string Encipher(string plaintext);
    string Decipher(string ciphertext);
}


using System.Security.Cryptography;

namespace CyptographyAlgorithms.Console;

public class Sha256HashAlgorithm : IHashAlgorithm
{
    public byte[] Hash(byte[] bytes) => SHA256.HashData(bytes);
}

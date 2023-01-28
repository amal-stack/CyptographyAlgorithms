using System.Numerics;

namespace CyptographyAlgorithms.Rsa;

public readonly record struct RsaPrivateKey(
    BigInteger D,
    BigInteger P,
    BigInteger Q);
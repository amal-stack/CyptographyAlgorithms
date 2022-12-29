using System.Numerics;

namespace CyptographyAlgorithms.Rsa;

public readonly record struct RsaPublicKey(
    BigInteger E, 
    BigInteger N);

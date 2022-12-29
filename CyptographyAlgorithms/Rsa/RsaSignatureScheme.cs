using System.Numerics;
using CyptographyAlgorithms.Extensions;

namespace CyptographyAlgorithms.Rsa;


public class RsaSignatureScheme : ISignatureScheme
{
    public RsaPublicKey PublicKey { get; }
    public RsaPrivateKey PrivateKey { get; }
    public IHashAlgorithm HashAlgorithm { get; }

    public byte[] Sign(byte[] message)
    {
        byte[] hash = HashAlgorithm.Hash(message);
        byte[] signature = Transform(hash, PrivateKey.D);
        return signature;
    }

    public bool Verify(byte[] message, byte[] signature)
    {
        byte[] hash = ModularArithmetic.Mod(new BigInteger(HashAlgorithm.Hash(message)), PublicKey.N)
            .ToByteArray();
        byte[] plaintext = Transform(signature, PublicKey.E);
        
        return Enumerable.SequenceEqual(hash, plaintext);
    }

    private byte[] Transform(byte[] plaintext, BigInteger exponent)
        => BigInteger.ModPow(
                new BigInteger(plaintext),
                exponent,
                PublicKey.N)
           .ToByteArray();

    public static RsaSignatureScheme Create(
        BigInteger p,
        BigInteger q,
        BigInteger e,
        BigInteger d,
        IHashAlgorithm hashAlgorithm)
    {
        BigInteger phi = (p - 1) * (q - 1);
        if (MathHelpers.Gcd(e, phi) != BigInteger.One)
        {
            throw new ArgumentException("e must be coprime to (p - 1) * (q - 1)", nameof(e));
        }
        if (ModularArithmetic.MultiplicativeInverse(d, phi) != e)
        {
            throw new ArgumentException("d is not the inverse of e", nameof(d));
        }
        RsaPublicKey publicKey = new(e, p * q);
        RsaPrivateKey privateKey = new(d, p, q);
        return new RsaSignatureScheme(publicKey, privateKey, hashAlgorithm);
    }


    private RsaSignatureScheme(
        RsaPublicKey publicKey,
        RsaPrivateKey privateKey,
        IHashAlgorithm hashAlgorithm)
    {
        PublicKey = publicKey;
        PrivateKey = privateKey;
        HashAlgorithm = hashAlgorithm;
    }
}

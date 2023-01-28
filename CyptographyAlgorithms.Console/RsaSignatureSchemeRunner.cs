using CyptographyAlgorithms.Extensions;
using CyptographyAlgorithms.Rsa;
using System.Numerics;
using System.Text;

namespace CyptographyAlgorithms.Console;

[Demonstrates<RsaSignatureScheme>]
public class RsaSignatureSchemeRunner
{
    public static void Run()
    {
        System.Console.WriteLine("RSA Digital Signature Scheme");

        // Input RSA parameters
        System.Console.Write("Enter a large prime p: ");
        var p = BigInteger.Parse(System.Console.ReadLine()!);

        System.Console.Write("Enter a large prime q: ");
        var q = BigInteger.Parse(System.Console.ReadLine()!);

        System.Console.Write("Enter the public exponent e: ");
        var e = BigInteger.Parse(System.Console.ReadLine()!);

        System.Console.Write("Enter the private exponent d: ");
        var d = BigInteger.Parse(System.Console.ReadLine()!);

        // Input message
        System.Console.Write("Enter a message: ");
        string message = System.Console.ReadLine()!;

        // Convert message to bytes using UTF8 encoding
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);

        // Create instance of RsaSignatureScheme
        var rsaSignature = RsaSignatureScheme.Create(
            p: p,
            q: q,
            e: e,
            d: d,
            hashAlgorithm: new Sha256HashAlgorithm());

        // Print Public Key
        System.Console.WriteLine(rsaSignature.PublicKey);

        //Print Private Key
        System.Console.WriteLine(rsaSignature.PrivateKey);

        // Hash
        byte[] hash = rsaSignature.HashAlgorithm.Hash(messageBytes);
        System.Console.WriteLine($"Message hash: {Convert.ToBase64String(hash)}");
        System.Console.WriteLine($"Message hash (mod n): {Convert.ToBase64String(ModularArithmetic.Mod(new BigInteger(hash), rsaSignature.PublicKey.N).ToByteArray())}");

        // Sign
        byte[] signature = rsaSignature.Sign(messageBytes);
        System.Console.WriteLine($"Signature: {Convert.ToBase64String(signature)}");

        // Verify
        System.Console.WriteLine($"Verified: {rsaSignature.Verify(messageBytes, signature)}");
    }
}

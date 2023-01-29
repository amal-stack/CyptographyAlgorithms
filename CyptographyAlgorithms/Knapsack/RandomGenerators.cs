using CyptographyAlgorithms.Extensions;
using System.Numerics;

namespace CyptographyAlgorithms.Knapsack;

public static class RandomGenerators
{
    public static IReadOnlyList<BigInteger> SuperincreasingSequence(int length, int toExclusive)
    {
        BigInteger sum = 0;
        List<BigInteger> sequence = new(length);
        for (int i = 0; i < length; i++)
        {
            BigInteger element = sum
                + System.Security.Cryptography.RandomNumberGenerator.GetInt32(1, toExclusive);
            sequence.Add(element);
            sum += element;
        }
        return sequence.AsReadOnly();
    }

    public static BigInteger IntegerGreaterThan(BigInteger sum, int toExclusive) =>
        sum
        + System.Security.Cryptography.RandomNumberGenerator.GetInt32(1, 255);

    public static BigInteger IntegerCoprimeTo(BigInteger number, int toExclusive)
    {
        BigInteger result = System.Security.Cryptography.RandomNumberGenerator.GetInt32(1, toExclusive);
        while (MathHelpers.Gcd(number, result) != 1)
        {
            result = System.Security.Cryptography.RandomNumberGenerator.GetInt32(1, toExclusive);
        }
        return result;
    }
}

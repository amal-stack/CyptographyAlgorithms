using System.Numerics;

namespace CyptographyAlgorithms.Extensions;

public static class MathHelpers
{
    public static TNumber Gcd<TNumber>(TNumber a, TNumber b)
        where TNumber : IBinaryInteger<TNumber>
    {
        while (b != TNumber.Zero)
        {
            (a, b) = (b, a % b);
        }
        return TNumber.Abs(a);
    }
}

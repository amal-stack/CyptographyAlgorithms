using System.Numerics;

namespace CyptographyAlgorithms.Extensions;

public static class ModularArithmetic
{
    public static TNumber Mod<TNumber>(TNumber a, TNumber b)
        where TNumber : IBinaryInteger<TNumber>
        => (a % b) switch
        {
            < 0 and TNumber mod => mod + b,
            TNumber mod => mod
        };
    public static TInteger MultiplicativeInverse<TInteger>(
        TInteger a,
        TInteger b)
        where TInteger : IBinaryInteger<TInteger>
    {
        var (x, y) = (TInteger.One, TInteger.Zero);
        //Console.WriteLine($"a = {a}. b = {b}. x = {x}, y = {y}");
        TInteger mod = b;
        while (b > TInteger.Zero)
        {
            (x, y) = (y, x - a / b * y);
            (a, b) = (b, a % b);
            //Console.WriteLine($"a = {a}. b = {b}. x = {x}, y = {y}");
        }
        return x < TInteger.Zero ? x + mod : x;
    }
}

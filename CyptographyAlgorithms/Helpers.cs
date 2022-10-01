namespace CyptographyAlgorithms;

internal static class Helpers
{
    public static int Mod(int a, int b) => (a % b) switch
    {
        < 0 and var mod => mod + b,
        var mod => mod
    };
}

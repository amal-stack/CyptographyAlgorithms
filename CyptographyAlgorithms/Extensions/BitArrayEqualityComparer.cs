using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace CyptographyAlgorithms.Extensions;

internal class BitArrayEqualityComparer : IEqualityComparer<BitArray>
{
    public bool Equals(BitArray? x, BitArray? y) => (x, y) switch
    {
        (null, null) => true,
        (null, _) => false,
        (_, null) => false,
        (_, _) when x.Count != y.Count => false,
        (_, _) => x.OfType<bool>().SequenceEqual(y.OfType<bool>())
    };

    public int GetHashCode([DisallowNull] BitArray obj) => obj.GetHashCode();
}
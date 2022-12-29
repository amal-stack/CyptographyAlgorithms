using CyptographyAlgorithms.Extensions;
using System.Collections;

namespace CyptographyAlgorithms;

public readonly struct InitializationVector
{
    public BitArray Value { get; }

    internal InitializationVector(BitArray value)
        => Value = value;

    public static InitializationVector FromBytes(byte[] value)
        => new(new BitArray(value).Trim());
}

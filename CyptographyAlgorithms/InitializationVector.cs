using System.Collections;

namespace CyptographyAlgorithms;

public struct InitializationVector
{
    public BitArray Value { get; }

    internal InitializationVector(BitArray value)
        => Value = value;

    public static InitializationVector FromBytes(byte[] value)
        => new(new BitArray(value).Trim());
}


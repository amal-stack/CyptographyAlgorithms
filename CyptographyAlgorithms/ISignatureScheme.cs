using System.Numerics;

namespace CyptographyAlgorithms;

public interface ISignatureScheme
{
    byte[] Sign(byte[] message);

    bool Verify(byte[] message, byte[] signature);
}



namespace CyptographyAlgorithms;

public interface ITransformation
{
    public byte[] Apply(byte[] value);
    public byte[] ApplyInverse(byte[] value);
}

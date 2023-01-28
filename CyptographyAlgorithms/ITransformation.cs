namespace CyptographyAlgorithms;

public interface ITransformation
{
    byte[] Apply(byte[] value);
    byte[] ApplyInverse(byte[] value);
}

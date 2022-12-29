namespace CyptographyAlgorithms.Spn;

public class SPNetworkBuilder
{
    private byte[] _key = default!;
    private ITransformation _sBox = default!;
    private ITransformation _pBox = default!;
    private Func<byte[], int, byte[]> _algorithm = default!;
    private int _rounds;

    public SPNetworkBuilder AddSBox(ITransformation sBox)
    {
        _sBox = sBox;
        return this;
    }

    public SPNetworkBuilder AddPBox(ITransformation pBox)
    {
        _pBox = pBox;
        return this;
    }

    public SPNetworkBuilder WithKey(byte[] key)
    {
        _key = key;
        return this;
    }

    public SPNetworkBuilder OfRounds(int rounds)
    {
        _rounds = rounds;
        return this;
    }

    public SPNetworkBuilder GenerateRoundKeysUsing(Func<byte[], int, byte[]> algorithm)
    {
        _algorithm = algorithm;
        return this;
    }

    public SPNetwork Build() => new(
            _key,
            _algorithm,
            _sBox,
            _pBox,
            _rounds);
}

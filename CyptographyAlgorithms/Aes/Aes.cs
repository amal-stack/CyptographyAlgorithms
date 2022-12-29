using System.Collections;

namespace CyptographyAlgorithms.Aes;

public sealed class Aes
{
    public byte[,] MixColumns(byte[,] state)
    {
        byte[,] newState = new byte[state.GetLength(0), state.GetLength(1)];
        for (int c = 0; c < newState.GetLength(1); c++)
        {
            //newState[0, c] = 0b1011;
            //newState[1, c]
            //newState[2, c]
            //newState[3, c]
        }
        return default!;
    }
}

file class SuperincreasingSequence : IReadOnlyList<int>
{
    private IReadOnlyList<int> _sequence;

    public SuperincreasingSequence(IEnumerable<int> sequence)
    {
        _sequence = sequence.ToList().AsReadOnly();
        int sum = _sequence[0];
        for (int i = 1; i < _sequence.Count; i++)
        {
            if (_sequence[i] <= sum)
            {
                throw new ArgumentException("Not a superincreasing sequence.", nameof(sequence));
            }
            sum += _sequence[i];
        }
        Sum = sum;
    }

    public int Sum { get; }

    public int this[int index] => _sequence[index];

    public int Count => _sequence.Count;

    public IEnumerator<int> GetEnumerator() => _sequence.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
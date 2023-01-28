using CyptographyAlgorithms.Extensions;
using System.Collections;
using System.Text;

namespace CyptographyAlgorithms.Stream;

public sealed class Lfsr : IEnumerable<bool>
{
    public Lfsr(int size, IReadOnlyList<int> taps)
    {
        if (!taps.Any())
        {
            throw new ArgumentException("Taps cannot be empty");
        }
        if (taps.Any(tap => tap > size))
        {
            throw new ArgumentException("All tap values must be lesser than size.");
        }
        Size = size;
        Taps = taps;
    }

    public int Size { get; }

    public IReadOnlyList<int> Taps { get; }

    public InitializationVector Iv { get; private set; }

    public BitArray? CurrentValue { get; private set; }

    public bool? OutBit { get; private set; }

    public bool Initialized => CurrentValue is not null;

    public void Initialize(InitializationVector iv)
    {
        if (iv.Value.Count > Size)
        {
            throw new ArgumentException("Initialization Vector is larger than the size of the LFSR.");
        }

        Iv = iv;
        CurrentValue = new BitArray(iv.Value);
    }

    public bool Next()
    {
        ThrowIfUninitialized();
        bool? nextBit = null;
        for (int i = Taps.Count - 1; i >= 0; i--)
        {
            var tapIdx = ^Taps[i];
            nextBit = nextBit is null
                ? CurrentValue![tapIdx]
                : nextBit ^ CurrentValue![tapIdx];
        }
        bool key = CurrentValue![0];
        OutBit = key;
        CurrentValue.RightShift(1);

        // Never happens
        _ = nextBit ?? throw new InvalidOperationException("Could not generate next bit");

        CurrentValue[^1] = nextBit.Value;
        return key;
    }

    public void Reset() => CurrentValue = new BitArray(Iv.Value);

    public int CalculatePeriod()
    {
        ThrowIfUninitialized();
        BitArray previousValue = new(CurrentValue!);
        BitArrayEqualityComparer comparer = new();
        int period = 0;
        Reset();
        do
        {
            Next();
            period++;
        } while (!comparer.Equals(CurrentValue, Iv.Value));
        Reset();
        CurrentValue = previousValue;
        return period;
    }

    public string GetPolynomial()
    {
        StringBuilder builder = new("1 + ");
        const string x = "x^";
        foreach (var tap in Taps.OrderBy(t => t))
        {
            builder
                .Append(x)
                .Append(tap)
                .Append(' ');
        }
        return builder.ToString();
    }

    public override string ToString()
    {
        if (CurrentValue is null)
        {
            return $"\nUninitialized LFSR [ Size: {Size}, Polynomial: {GetPolynomial()} ]\n";
        }
        const string delim = " | ";
        StringBuilder builder = new(delim);
        for (int i = CurrentValue!.Length - 1; i >= 0; i--)
        {
            builder.Append(CurrentValue[i] ? 1 : 0)
                .Append(delim);
        }
        return builder.Append('\n')
            .Append("Key: ")
            .Append(OutBit.HasValue ? OutBit.Value ? 1 : 0 : "-")
            .Append('\n')
            .ToString();
    }

    private void ThrowIfUninitialized()
    {
        if (CurrentValue is null)
        {
            throw new ArgumentException("LFSR has not been initialized yet.");
        }
    }

    public IEnumerator<bool> GetEnumerator() => new KeyStreamEnumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    internal sealed class KeyStreamEnumerator : IEnumerator<bool>
    {
        internal KeyStreamEnumerator(Lfsr lfsr)
        {
            Lfsr = lfsr;
            lfsr.Reset();
        }

        public bool Current => Lfsr.OutBit
            ?? throw new InvalidOperationException("Enumeration has not started yet; call MoveNext()");

        public Lfsr Lfsr { get; }

        object IEnumerator.Current => Current;

        public void Dispose() => Lfsr.Reset();

        public bool MoveNext()
        {
            if (!Lfsr.Initialized)
            {
                throw new InvalidOperationException("Enumeration on uninitialized LFSR; Call Initialize() on the LFSR.");
            }
            Lfsr.Next();
            return true;
        }

        public void Reset() => Lfsr.Reset();
    }
}


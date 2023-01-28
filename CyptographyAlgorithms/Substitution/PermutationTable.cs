using CyptographyAlgorithms.Extensions;
using System.Text;

namespace CyptographyAlgorithms.Substitution;

public sealed class PermutationTable
{
    private IReadOnlyDictionary<char, char> Table { get; }

    private IReadOnlyDictionary<char, char> InverseTable { get; }

    private PermutationTable(IReadOnlyDictionary<char, char> table)
    {
        Table = table;
        InverseTable = table.ToDictionary(p => p.Value, p => p.Key);
    }

    public static IEnumerable<char> Alphabet =>
         from number in Enumerable.Range(65, 26)
         select Convert.ToChar(number);

    public char Get(char @char) => char.IsLetter(@char)
        ? char.IsLower(@char)
            ? char.ToLower(Table[char.ToUpper(@char)])
            : Table[@char]
        : @char;

    public char GetInverse(char @char) => char.IsLetter(@char)
        ? char.IsLower(@char)
            ? char.ToLower(InverseTable[char.ToUpper(@char)])
            : InverseTable[@char]
        : @char;

    public char this[char @char, bool inverse = false] => inverse
        ? GetInverse(@char)
        : Get(@char);

    public static PermutationTable FromDictionary(IReadOnlyDictionary<char, char> permutation)
        => new(permutation);

    public static PermutationTable FromPermutationString(string permutation)
    {
        if (permutation.Length != 26)
        {
            throw new ArgumentException(
                "Length of permutation string must be 26",
                nameof(permutation));
        }
        var dictionary = new Dictionary<char, char>();
        var mappedAlphabets = new HashSet<char>();

        foreach (var (letter, index) in Alphabet
            .Select((letter, index) => (letter, index)))
        {
            char result = char.ToUpper(permutation[index]);
            if (mappedAlphabets.Contains(result))
            {
                throw new ArgumentException(
                "Permutation string contains duplicates",
                nameof(permutation));
            }
            if (!char.IsLetter(result))
            {
                throw new ArgumentException(
                    "Permutation string must only contain letters",
                    nameof(permutation));
            }
            dictionary[letter] = result;
            mappedAlphabets.Add(result);
        }
        return new(dictionary);
    }

    public static PermutationTable GenerateRandom() => new(
        Alphabet
            .Zip(Alphabet.Shuffle())
            .ToDictionary(p => p.First, p => p.Second));


    public override string ToString()
    {
        StringBuilder keyBuilder = new();
        StringBuilder valueBuilder = new();
        string separator = " | ";

        foreach (var letter in Alphabet)
        {
            keyBuilder.Append(letter).Append(separator);
            valueBuilder.Append(Table[letter]).Append(separator);
        }

        return keyBuilder
            .Append(Environment.NewLine)
            .Append(valueBuilder)
            .Append(Environment.NewLine)
            .ToString();
    }
}
using System.Collections;

namespace CyptographyAlgorithms.Extensions;

public static class Formatters
{
    public static string ToBitString(this byte[] bytes, char delimiter = ' ') => string.Join(
            "",
            bytes
                .SelectMany(@byte => Convert
                    .ToString(@byte, 2)
                    .PadLeft(8, '0')
                    .Select((bit, idx) => (idx + 1) % 4 == 0 ? $"{bit} " : bit.ToString()))
        );

    public static string ToBitString(this BitArray bitArray, char delimiter = ' ') => string.Concat(
        bitArray
            .OfType<bool>()
            //.Reverse()
            .Select((i, idx) => (i ? 1 : 0) + ((idx + 1) % 8 == 0 ? delimiter.ToString() : ""))
    );

}
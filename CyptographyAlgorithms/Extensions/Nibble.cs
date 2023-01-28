using System.Numerics;

namespace CyptographyAlgorithms.Extensions;

public static class Nibble
{
    public const int HighMask = 0xF0;

    public const int LowMask = 0x0F;

    public static byte ToByte(byte high, byte low) => (byte)(high << 4 | low);

    public static byte GetHighNibble(this byte b) => (byte)((b & HighMask) >> 4);

    public static byte GetLowNibble(this byte b) => (byte)(b & LowMask);

    public static (byte High, byte Low) GetNibbles(this byte b) => (b.GetHighNibble(), b.GetLowNibble());

    public static byte SwapNibbles(this byte b) => ToByte(b.GetLowNibble(), b.GetHighNibble());
}


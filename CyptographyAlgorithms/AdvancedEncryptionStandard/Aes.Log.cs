using System.Text;

namespace CyptographyAlgorithms.AdvancedEncryptionStandard;

public sealed partial class Aes
{
    public Action<string>? LogCallback { get; set; }

    private void Log(int round, byte[,] state, string step)
    {
        string message = $"round[{round,2}].{step}";
        LogCallback?.Invoke($"{message,-32}{Convert.ToHexString(State.Flatten(state)).ToLowerInvariant()}");

    }

    private void Log(int round, byte[,] state, ReadOnlySpan<byte[]> roundKey)
    {
        string stateMessage = $"round[{round,2}].AddRoundKey";
        string roundKeyMessage = $"round[{round,2}].RoundKey";

        StringBuilder builder = new();
        foreach (var bytes in roundKey)
        {
            builder.Append(Convert.ToHexString(bytes).ToLowerInvariant());
        }

        LogCallback?.Invoke($"{roundKeyMessage,-32}{builder.ToString()}");
        LogCallback?.Invoke($"{stateMessage,-32}{Convert.ToHexString(State.Flatten(state)).ToLowerInvariant()}");

    }

    private void Log(string message)
        => LogCallback?.Invoke(message);
}


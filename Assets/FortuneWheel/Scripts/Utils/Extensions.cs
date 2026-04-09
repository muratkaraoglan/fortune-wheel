using TMPro;

namespace FortuneWheel.Scripts.Utils
{
    public static class Extensions
    {
        /// <summary>Formats a number (uint) into human-readable string (1.2k, 5m, etc.).</summary>
        public static string FormatNumber(this int n) => n switch
        {
            < 1000 => n.ToString(),
            < 10000 => $"{n - 5:#,.##}k",
            < 100000 => $"{n - 50:#,.#}k",
            < 1000000 => $"{n - 500:#,.}k",
            < 10000000 => $"{n - 5000:#,,.##}m",
            < 100000000 => $"{n - 50000:#,,.#}m",
            < 1000000000 => $"{n - 500000:#,,.}m",
            _ => $"{n - 5000000:#,,,.##}b"
        };
        
        public static void SetFormattedNumber(this TextMeshProUGUI text, int n)
        {
            switch (n)
            {
                case < 1000:
                    text.SetText("x{0}", n);
                    return;
                case < 1_000_000:
                {
                    float value = n / 1000f;
                    text.SetText("x{0:0.#}k", value);
                    return;
                }
                case < 1_000_000_000:
                {
                    float value = n / 1_000_000f;
                    text.SetText("x{0:0.#}m", value);
                    return;
                }
                default:
                {
                    var v = n / 1_000_000_000f;
                    text.SetText("x{0:0.#}b", v);
                    break;
                }
            }
        }
    }
}
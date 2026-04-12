using System.Collections.Generic;
using FortuneWheel.Scripts.Item.Enums;
using TMPro;
using UnityEngine;

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
                    text.SetText("x{0:0}k", value);
                    return;
                }
                case < 1_000_000_000:
                {
                    float value = n / 1_000_000f;
                    text.SetText("x{0:0}m", value);
                    return;
                }
                default:
                {
                    var v = n / 1_000_000_000f;
                    text.SetText("x{0:0}b", v);
                    break;
                }
            }
        }
        
        /// <summary>
        /// Shuffle the list based on Fisher-Yates algorithm
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            System.Random rnd = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
        
        public static Color GetColor(this ItemRarity rarity)
        {
            return rarity switch
            {
                ItemRarity.Common => Color.white, 
                ItemRarity.Uncommon => new Color(0.12f, 0.77f, 0.12f), 
                ItemRarity.Rare => new Color(0f, 0.44f, 0.87f),    
                ItemRarity.Epic => new Color(0.64f, 0.21f, 0.93f),
                ItemRarity.Legendary => new Color(1f, 0.5f, 0f), 
                ItemRarity.Mythic => new Color(0.9f, 0.1f, 0.1f), 
                _ => Color.white
            };
        }

        public static string GetHexColor(this ItemRarity rarity)
        {
            return "#" + ColorUtility.ToHtmlStringRGB(rarity.GetColor());
        }

    }
}
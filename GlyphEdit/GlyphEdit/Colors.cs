using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;

namespace GlyphEdit
{
    public static class Colors
    {
        private static readonly Regex HexColorRegex =
            new Regex("#?(?<r>[0-9a-fA-F]{1,2})(?<g>[0-9a-fA-F]{1,2})(?<b>[0-9a-fA-F]{1,2})(?<a>[0-9a-fA-F]{1,2})?");

        public static bool IsHexCode(string hexCode)
        {
            return HexColorRegex.IsMatch(hexCode);
        }

        public static Color FromHex(string hexCode)
        {
            var match = HexColorRegex.Match(hexCode);
            if (!match.Success)
                throw new Exception($"Could not parse color from \"{hexCode}\".");
            var r = int.Parse(match.Groups["r"].Value, NumberStyles.HexNumber);
            var g = int.Parse(match.Groups["g"].Value, NumberStyles.HexNumber);
            var b = int.Parse(match.Groups["b"].Value, NumberStyles.HexNumber);
            if (match.Groups["a"].Success)
            {
                var a = int.Parse(match.Groups["a"].Value, NumberStyles.HexNumber);
                return new Color(r, g, b, a);
            }

            return new Color(r, g, b);
        }
    }
}

using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GlyphEdit.Model
{
    public static class Colors
    {
        private static readonly Regex HexColorRegex =
            new Regex("#?(?<r>[0-9a-fA-F]{1,2})(?<g>[0-9a-fA-F]{1,2})(?<b>[0-9a-fA-F]{1,2})(?<a>[0-9a-fA-F]{1,2})?");

        public static bool IsHexCode(string hexCode)
        {
            return HexColorRegex.IsMatch(hexCode);
        }

        public static GlyphColor FromHex(string hexCode)
        {
            var match = HexColorRegex.Match(hexCode);
            if (!match.Success)
                throw new Exception($"Could not parse color from \"{hexCode}\".");
            var r = (byte)int.Parse(match.Groups["r"].Value, NumberStyles.HexNumber);
            var g = (byte)int.Parse(match.Groups["g"].Value, NumberStyles.HexNumber);
            var b = (byte)int.Parse(match.Groups["b"].Value, NumberStyles.HexNumber);
            if (match.Groups["a"].Success)
            {
                var a = (byte)int.Parse(match.Groups["a"].Value, NumberStyles.HexNumber);
                return new GlyphColor(r, g, b, a);
            }

            return new GlyphColor(r, g, b);
        }
    }
}

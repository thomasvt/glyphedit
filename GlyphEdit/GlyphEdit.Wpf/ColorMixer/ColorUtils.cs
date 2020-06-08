using System.Windows.Media;


namespace GlyphEdit.Wpf.ColorMixer
{
    public static class ColorUtils
    {
        /// <summary>
        /// From HSL, all within [0, 1].
        /// </summary>
        public static Color FromHsl(float hue, float saturation, float luminance)
        {
            if (saturation == 0f)
            {
                var c = (byte)(luminance * byte.MaxValue);
                return Color.FromRgb(c, c, c);
            }

            var v2 = luminance + saturation - saturation * luminance;
            if (luminance < 0.5f)
            {
                v2 = luminance * (1 + saturation);
            }
            var v1 = 2f * luminance - v2;

            return Color.FromRgb((byte)(HueToRgb(v1, v2, hue + 1f / 3f) * byte.MaxValue), (byte)(HueToRgb(v1, v2, hue) * byte.MaxValue), (byte)(HueToRgb(v1, v2, hue - 1f / 3f) * byte.MaxValue));
        }

        private static float HueToRgb(float v1, float v2, float vH)
        {
            vH += vH < 0 ? 1 : 0;
            vH -= vH > 1 ? 1 : 0;
            var ret = v1;

            if (6 * vH < 1)
            {
                ret = v1 + (v2 - v1) * 6 * vH;
            }

            else if (2 * vH < 1)
            {
                ret = (v2);
            }

            else if (3 * vH < 2)
            {
                ret = v1 + (v2 - v1) * (2f / 3f - vH) * 6f;
            }

            return ret < 0f ? 0f : (ret > 1f ? 1f : ret);
        }
    }
}

using System;
using GlyphEdit.Model;

namespace GlyphEdit.Controls.DocumentControl
{
    public static class GridGeometry
    {
        public static void ForEachCellInLine(VectorI lineBegin, VectorI lineEnd, Action<VectorI> action)
        {
            // from https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm ("All Cases" section)

            var (x, y) = lineBegin;
            var dx = Math.Abs(lineEnd.X - x);
            var sx = x < lineEnd.X ? 1 : -1;
            var dy = -Math.Abs(lineEnd.Y - y);
            var sy = y < lineEnd.Y ? 1 : -1;
            var error = dx + dy; 
            while (true)
            {
                action(new VectorI(x, y));

                if (x == lineEnd.X && y == lineEnd.Y) break;
                var e2 = error + error;
                if (e2 >= dy)
                {
                    error += dy; 
                    x += sx;
                }
                if (e2 <= dx) 
                {
                    error += dx;
                    y += sy;
                }
            }
        }
    }
}

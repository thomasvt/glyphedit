﻿using Microsoft.Xna.Framework;

namespace GlyphEdit.Models
{
    public class Layer
    {
        internal readonly DocumentElement[,] Elements;

        internal Layer(int width, int height)
        {
            Elements = new DocumentElement[width, height];
        }

        public ref DocumentElement GetElementRef(Point coords)
        {
            var (x, y) = coords;
            return ref Elements[x, y];
        }
    }
}
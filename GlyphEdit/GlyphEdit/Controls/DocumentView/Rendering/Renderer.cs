using System;
using GlyphEdit.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlyphEdit.Controls.DocumentView.Rendering
{
    /// <summary>
    /// A very simple immediate renderer for orthographic 2D graphics where X+ is right and Y+ is down, as is common in 2D canvasses (unlike 3D where Y+ is often up).
    /// Each Draw~() call is rendered immediately.
    /// </summary>
    public class Renderer : IRenderer
    {
        private BasicEffect _effect;
        private GraphicsDevice _graphicsDevice;
        private VertexPositionColorTexture[] _texturedVertices;
        private VertexPositionColor[] _vertices;

        public void Load(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _effect = new BasicEffect(graphicsDevice);
            _texturedVertices = new VertexPositionColorTexture[1000];
            _vertices = new VertexPositionColor[1000];
        }

        /// <summary>
        /// Returns the render viewport size in pixels.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetViewport()
        {
            return new Vector2(_graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height);
        }

        public void BeginFrame(Matrix viewMatrix, Matrix projectionMatrix)
        {
            _effect.View = viewMatrix;
            _effect.Projection = projectionMatrix;
            _effect.World = Matrix.Identity;
            _effect.VertexColorEnabled = true;
        }

        void IRenderer.DrawQuad(Texture2D texture, Quad quad)
        {
            const int vertexCount = 6;
            if (vertexCount > _texturedVertices.Length)
                EnsureVertexPool(6);

            // tri 1
            _texturedVertices[0] = quad.TopLeft;
            _texturedVertices[1] = quad.TopRight;
            _texturedVertices[2] = quad.BottomLeft;
            // tri 2
            _texturedVertices[3] = quad.TopRight;
            _texturedVertices[4] = quad.BottomRight;
            _texturedVertices[5] = quad.BottomLeft;

            RenderVertices(_texturedVertices, texture, 2);
        }

        public void DrawElements(GlyphMapTexture glyphMapTexture, DocumentElement[,] elements)
        {
            // we directly render from the document datastructures. This is a tight coupling compromise for performance.

            var width = elements.GetLength(0);
            var height = elements.GetLength(1);

            // pool big enough?
            var vertexCount = width * height * 6;
            EnsureVertexPool(vertexCount);

            // render glyph backgrounds
            var i = 0;
            for (var y = 0; y < height; y++) // row first for less cache misses
            {
                for (var x = 0; x < width; x++)
                {
                    ref var element = ref elements[x, y];

                    var backColor = element.Background.ToRenderColor();
                    if (backColor.A == 0)
                        continue;

                    var left = (int)(x * glyphMapTexture.GlyphWidth);
                    var top = (int)(y * glyphMapTexture.GlyphHeight);
                    var right = (int)(left + glyphMapTexture.GlyphWidth);
                    var bottom = (int)(top + glyphMapTexture.GlyphHeight);

                    var topLeft = new VertexPositionColor(new Vector3(left, top, 0), backColor);
                    var topRight = new VertexPositionColor(new Vector3(right, top, 0), backColor);
                    var bottomLeft = new VertexPositionColor(new Vector3(left, bottom, 0), backColor);
                    var bottomRight = new VertexPositionColor(new Vector3(right, bottom, 0), backColor);

                    // tri 1
                    _vertices[i++] = topLeft;
                    _vertices[i++] = topRight;
                    _vertices[i++] = bottomLeft;
                    // tri 2
                    _vertices[i++] = topRight;
                    _vertices[i++] = bottomRight;
                    _vertices[i++] = bottomLeft;
                }
            }

            RenderVertices(_vertices, i / 3);

            // pool big enough?
            EnsureTexturedVertexPool(vertexCount);

            // render glyph foregrounds
            i = 0;
            for (var y = 0; y < height; y++) // row first for less cache misses
            {
                for (var x = 0; x < width; x++)
                {
                    ref var element = ref elements[x, y];

                    var frontColor = element.Foreground.ToRenderColor();

                    var left = (int)(x * glyphMapTexture.GlyphWidth);
                    var top = (int)(y * glyphMapTexture.GlyphHeight);
                    var right = (int)(left + glyphMapTexture.GlyphWidth);
                    var bottom = (int)(top + glyphMapTexture.GlyphHeight);

                    var glyphUvRect = glyphMapTexture.GetUVRect(element.Glyph);

                    var topLeft = new VertexPositionColorTexture(new Vector3(left, top, 0), frontColor, new Vector2(glyphUvRect.MinU, glyphUvRect.MinV));
                    var topRight = new VertexPositionColorTexture(new Vector3(right, top, 0), frontColor, new Vector2(glyphUvRect.MaxU, glyphUvRect.MinV));
                    var bottomLeft = new VertexPositionColorTexture(new Vector3(left, bottom, 0), frontColor, new Vector2(glyphUvRect.MinU, glyphUvRect.MaxV));
                    var bottomRight = new VertexPositionColorTexture(new Vector3(right, bottom, 0), frontColor, new Vector2(glyphUvRect.MaxU, glyphUvRect.MaxV));

                    // tri 1
                    _texturedVertices[i++] = topLeft;
                    _texturedVertices[i++] = topRight;
                    _texturedVertices[i++] = bottomLeft;
                    // tri 2
                    _texturedVertices[i++] = topRight;
                    _texturedVertices[i++] = bottomRight;
                    _texturedVertices[i++] = bottomLeft;
                }
            }

            RenderVertices(_texturedVertices, glyphMapTexture.Texture, i / 3);
        }

        private void RenderVertices(VertexPositionColorTexture[] vertices, Texture2D texture, int triangleCount)
        {
            if (triangleCount == 0)
                return;
            if (texture == null)
                throw new ArgumentNullException(nameof(texture));

            _graphicsDevice.SamplerStates[0] = new SamplerState
            {
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                Filter = TextureFilter.Point
            };
            _graphicsDevice.BlendState = BlendState.NonPremultiplied;

            _effect.TextureEnabled = true;
            _effect.Texture = texture;

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, triangleCount);
            }
        }

        private void RenderVertices(VertexPositionColor[] vertices, int triangleCount)
        {
            if (triangleCount == 0)
                return;

            _graphicsDevice.SamplerStates[0] = new SamplerState
            {
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                Filter = TextureFilter.Point
            };
            _graphicsDevice.BlendState = BlendState.AlphaBlend;

            _effect.TextureEnabled = false;
            _effect.Texture = null;

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, triangleCount);
            }
        }

        private void EnsureTexturedVertexPool(int needed)
        {
            if (needed <= _texturedVertices.Length)
                return;
            var newVertices = new VertexPositionColorTexture[(int)(needed * 1.2f)];
            _texturedVertices = newVertices;
        }

        private void EnsureVertexPool(int needed)
        {
            if (needed <= _vertices.Length)
                return;
            var newVertices = new VertexPositionColor[(int)(needed * 1.2f)];
            _vertices = newVertices;
        }

        public void Unload()
        {
            _effect.Dispose();
        }
    }
}

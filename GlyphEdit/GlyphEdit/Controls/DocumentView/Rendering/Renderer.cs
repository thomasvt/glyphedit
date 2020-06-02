using GlyphEdit.Controls.DocumentView.Model;
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
        private VertexPositionColorTexture[] _vertices;

        public void Load(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _effect = new BasicEffect(graphicsDevice);
            _vertices = new VertexPositionColorTexture[1000];
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

            _graphicsDevice.SamplerStates[0] = new SamplerState
            {
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                Filter = TextureFilter.Point
            };
        }

        void IRenderer.DrawQuad(Texture2D texture, Quad quad)
        {
            const int vertexCount = 6;
            if (vertexCount > _vertices.Length)
                ExpandVerticesPool(6);

            // tri 1
            _vertices[0] = quad.TopLeft;
            _vertices[1] = quad.TopRight;
            _vertices[2] = quad.BottomLeft;
            // tri 2
            _vertices[3] = quad.TopRight;
            _vertices[4] = quad.BottomRight;
            _vertices[5] = quad.BottomLeft;

            RenderCurrentVertices(texture, 2);
        }

        public void DrawElements(GlyphMapTexture glyphMapTexture, DocumentElement[,] elements, Vector2 glyphRenderSize)
        {
            // we directly render from the document datastructures. This is a tight coupling compromise for performance.

            var width = elements.GetLength(0);
            var height = elements.GetLength(1);

            // pool big enough?
            var vertexCount = width * height * 6 * 2;
            if (vertexCount > _vertices.Length)
                ExpandVerticesPool(vertexCount);
            
            // convert all to triangles

            var i = 0;
            for (var y = 0; y < height; y++) // row first for less cache misses
            {
                for (var x = 0; x < width; x++)
                {
                    ref var element = ref elements[x, y];

                    var backColor = element.Background.ToRenderColor();
                    var frontColor = element.Foreground.ToRenderColor();

                    var left = x * glyphRenderSize.X;
                    var top = y * glyphRenderSize.Y;
                    var right = left + glyphRenderSize.X;
                    var bottom = top + glyphRenderSize.Y;

                    var glyphUvRect = glyphMapTexture.GetUVRect(element.Glyph);

                    var topLeft = new VertexPositionColorTexture(new Vector3(left, top, 0), backColor, new Vector2(glyphUvRect.MinU, glyphUvRect.MinV));
                    var topRight = new VertexPositionColorTexture(new Vector3(right, top, 0), backColor, new Vector2(glyphUvRect.MaxU, glyphUvRect.MinV));
                    var bottomLeft = new VertexPositionColorTexture(new Vector3(left, bottom, 0), backColor, new Vector2(glyphUvRect.MinU, glyphUvRect.MaxV));
                    var bottomRight = new VertexPositionColorTexture(new Vector3(right, bottom, 0), backColor, new Vector2(glyphUvRect.MaxU, glyphUvRect.MaxV));

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

            RenderCurrentVertices(glyphMapTexture.Texture, i / 3);
        }

        private void ExpandVerticesPool(int needed)
        {
            var newVertices = new VertexPositionColorTexture[(int)(needed * 1.2f)];
            _vertices = newVertices;
        }

        private void RenderCurrentVertices(Texture2D texture, int triangleCount)
        {
            if (triangleCount == 0)
                return;
            
            _effect.TextureEnabled = texture != null;
            _effect.Texture = texture;

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _vertices, 0, triangleCount);
            }
        }

        public void Unload()
        {
            _effect.Dispose();
        }
    }
}

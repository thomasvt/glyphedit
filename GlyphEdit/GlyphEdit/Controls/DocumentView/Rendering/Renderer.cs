using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlyphEdit.Controls.DocumentView.Rendering
{
    /// <summary>
    /// A very simple immediate renderer for orthographic 2D graphics where X+ is right and Y+ is down, as is common in 2D (unlike 3D where Y+ is often up).
    /// </summary>
    public class Renderer : IRenderer
    {
        private const int MaxVertexCount = 1000;

        private BasicEffect _effect;
        private Vector2 _cameraPosition;
        private GraphicsDevice _graphicsDevice;
        private VertexPositionColorTexture[] _vertices;

        public void Load(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _effect = new BasicEffect(graphicsDevice);
            _vertices = new VertexPositionColorTexture[MaxVertexCount];
            
        }

        public void SetCameraPosition(Vector2 position)
        {
            _cameraPosition = position;
        }

        public void BeginFrame()
        {
            _effect.Projection = Matrix.CreateOrthographic(_graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height, 0.1f, 10f);
            _effect.View = Matrix.CreateLookAt(new Vector3(_cameraPosition.X, _cameraPosition.Y, -5f), new Vector3(_cameraPosition.X, _cameraPosition.Y, 0f), Vector3.Down);
            _effect.World = Matrix.Identity;

            _graphicsDevice.SamplerStates[0] = new SamplerState
            {
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                Filter = TextureFilter.Point
            };
        }

        void IRenderer.DrawQuad(Texture2D texture, Quad quad)
        {

            var i = 0;
            // tri 1
            _vertices[i++] = quad.TopLeft;
            _vertices[i++] = quad.TopRight;
            _vertices[i++] = quad.BottomLeft;
            // tri 2
            _vertices[i++] = quad.TopRight;
            _vertices[i++] = quad.BottomRight;
            _vertices[i] = quad.BottomLeft;

            RenderCurrentVertices(texture, 2);
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

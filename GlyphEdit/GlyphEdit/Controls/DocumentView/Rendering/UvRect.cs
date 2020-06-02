namespace GlyphEdit.Controls.DocumentView.Rendering
{
    public class UvRect
    {
        public readonly float MinV;
        public readonly float MaxU;
        public readonly float MaxV;
        public readonly float MinU;

        public UvRect(float minU, float minV, float maxU, float maxV)
        {
            MinV = minV;
            MaxU = maxU;
            MaxV = maxV;
            MinU = minU;
        }
    }
}

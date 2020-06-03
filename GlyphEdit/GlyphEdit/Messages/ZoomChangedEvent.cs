namespace GlyphEdit.Messages
{
    public class ZoomChangedEvent
    {
        public readonly float Zoom;

        public ZoomChangedEvent(float zoom)
        {
            Zoom = zoom;
        }
    }
}

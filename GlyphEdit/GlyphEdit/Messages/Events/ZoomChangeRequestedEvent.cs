namespace GlyphEdit.Messages.Events
{
    public class ZoomChangeRequestedEvent
    {
        public readonly float Zoom;

        public ZoomChangeRequestedEvent(float zoom)
        {
            Zoom = zoom;
        }
    }
}

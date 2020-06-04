using System.Diagnostics;

namespace GlyphEdit.Messages
{
    public class ZoomChangedEvent
    {
        public readonly float Zoom;

        [DebuggerStepThrough]
        public ZoomChangedEvent(float zoom)
        {
            Zoom = zoom;
        }
    }
}

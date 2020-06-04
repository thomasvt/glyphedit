using System.Diagnostics;

namespace GlyphEdit.Messages.Events
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

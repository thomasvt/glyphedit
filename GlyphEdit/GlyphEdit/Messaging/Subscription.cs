namespace GlyphEdit.Messaging
{
    internal class Subscription
    {
        public object Owner { get; }
        public object HandlerAction { get; }

        public Subscription(object owner, object handlerAction)
        {
            Owner = owner;
            HandlerAction = handlerAction;
        }
    }
}

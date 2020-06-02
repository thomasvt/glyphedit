using System;

namespace GlyphEdit.Messaging
{
    public interface IMessageBus
    {
        /// <summary>
        /// subscribes a message handler to messages of type T.
        /// </summary>
        void Subscribe<T>(Action<T> handler);
        /// <summary>
        /// Sends a message to all subscribers of that message type T.
        /// </summary>
        void Publish<T>(T message);
    }
}
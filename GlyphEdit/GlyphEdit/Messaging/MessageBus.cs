using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GlyphEdit.Messaging
{
    // This is a copy of the MessageBus in my Hellhole.Messaging library that I use in most projects, but I made it static for this app.
    // IoC with ctor injection in WPF requires quite some (MVVM frame)work. And I don't think it's worth the effort for this small app.
    public static class MessageBus
    {
        private static readonly Dictionary<Type, List<Subscription>> SubscriptionsPerMessageType;

        static MessageBus()
        {
            SubscriptionsPerMessageType = new Dictionary<Type, List<Subscription>>();
        }

        /// <summary>
        /// Subscribes a handler action to all message of type T. Note that this overload disables you to unsubscribe the handler.
        /// Do not use this from an instance that should be able to dispose of its subscriptions (eg. it has a limited lifetime in the application)
        /// </summary>
        [DebuggerStepThrough]
        public static void Subscribe<T>(Action<T> handler)
        {
            Subscribe<T>(null, handler);
        }

        /// <summary>
        /// Subscribes a handler action to all messages of type T. The owner is the instance that is subscribing, use this reference to unsubscribe the handlers.
        /// </summary>
        [DebuggerStepThrough]
        public static void Subscribe<T>(object owner, Action<T> handler)
        {
            if (!SubscriptionsPerMessageType.TryGetValue(typeof(T), out var list))
            {
                list = new List<Subscription>();
                SubscriptionsPerMessageType.Add(typeof(T), list);
            }
            list.Add(new Subscription(owner, handler));
        }

        [DebuggerStepThrough]
        public static void Publish<T>(T message)
        {
            if (!SubscriptionsPerMessageType.TryGetValue(message.GetType(), out var list))
                return;

            foreach (var subscription in list)
            {
                var handler = (Action<T>) (subscription.HandlerAction);
                handler.Invoke(message);
            }
        }

        /// <summary>
        /// Removes all handlers subscribed by the owner.
        /// </summary>
        /// <param name="owner"></param>
        public static void Unsubscribe(object owner)
        {
            foreach (var pair in SubscriptionsPerMessageType.ToList())
            {
                foreach (var subscription in pair.Value.ToList())
                {
                    if (subscription.Owner == owner)
                        pair.Value.Remove(subscription);
                    if (!pair.Value.Any()) // that message type has no more subscriptions: delete it from the dictionary.
                        SubscriptionsPerMessageType.Remove(pair.Key);
                }
            }
        }
    }
}

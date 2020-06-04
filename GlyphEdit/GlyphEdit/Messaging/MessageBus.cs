using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GlyphEdit.Messaging
{
    // This is a copy of the MessageBus in my Hellhole.Messaging library that I use in most projects, but I made it static for this app.
    // IoC with ctor injection in WPF requires quite some (MVVM frame)work. And I don't think it's worth the effort for this small app.
    public static class MessageBus
    {
        private static readonly Dictionary<Type, List<object>> Actions;

        static MessageBus()
        {
            Actions = new Dictionary<Type, List<object>>();
        }

        [DebuggerStepThrough]
        public static void Subscribe<T>(Action<T> handler)
        {
            if (!Actions.TryGetValue(typeof(T), out var list))
            {
                list = new List<object>();
                Actions.Add(typeof(T), list);
            }
            list.Add(handler);
        }

        [DebuggerStepThrough]
        public static void Publish<T>(T message)
        {
            if (!Actions.TryGetValue(message.GetType(), out var list))
                return;

            foreach (Action<T> action in list)
            {
                action.Invoke(message);
            }
        }
    }
}

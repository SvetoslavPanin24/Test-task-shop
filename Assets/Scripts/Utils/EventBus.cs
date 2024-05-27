using System;
using System.Collections.Generic;

    public static class EventBus
    {
        private static Dictionary<Type, List<Action<object>>> subscribers = new Dictionary<Type, List<Action<object>>>();

        public static void Subscribe<T>(Action<T> callback) where T : class
        {
            Type type = typeof(T);
            if (!subscribers.ContainsKey(type))
            {
                subscribers[type] = new List<Action<object>>();
            }
            subscribers[type].Add(e => callback(e as T));
        }

        public static void Unsubscribe<T>(Action<T> callback) where T : class
        {
            Type type = typeof(T);
            if (subscribers.ContainsKey(type))
            {
                subscribers[type].Remove(e => callback(e as T));
            }
        }

        public static void Invoke<T>(T eventMessage) where T : class
        {
            Type type = typeof(T);
            if (subscribers.ContainsKey(type))
            {
                foreach (var callback in subscribers[type])
                {
                    callback(eventMessage);
                }
            }
        }
    }

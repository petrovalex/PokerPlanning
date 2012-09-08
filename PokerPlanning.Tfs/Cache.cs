namespace PokerPlanning.Tfs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections.Concurrent;

    public static class Cache
    {
        private static ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();

        public static void Insert(string key, object value)
        {
            _cache.TryAdd(key, value);
        }

        public static object Get(string key)
        {
            object value = null;

            _cache.TryGetValue(key, out value);

            return value;
        }
    }
}

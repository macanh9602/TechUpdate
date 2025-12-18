using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VTLTools
{
        /// <summary>
        /// LINQ-style helpers with NO allocation.
        /// Designed for Unity runtime (Update / FixedUpdate safe).
        /// </summary>
        public static class LinQNoAllocUtils
        {
                // =========================================================
                // LIST
                // =========================================================

                public static void WhereTo<T>(
                    this List<T> source,
                    List<T> result,
                    Predicate<T> predicate)
                {
                        result.Clear();
                        for (int i = 0; i < source.Count; i++)
                        {
                                T item = source[i];
                                if (predicate(item))
                                        result.Add(item);
                        }
                }

                public static T FirstOrDefaultNoAlloc<T>(
                    this List<T> source,
                    Predicate<T> predicate)
                {
                        for (int i = 0; i < source.Count; i++)
                        {
                                T item = source[i];
                                if (predicate(item))
                                        return item;
                        }
                        return default;
                }

                public static bool AnyNoAlloc<T>(
                    this List<T> source,
                    Predicate<T> predicate)
                {
                        for (int i = 0; i < source.Count; i++)
                        {
                                if (predicate(source[i]))
                                        return true;
                        }
                        return false;
                }

                public static int CountNoAlloc<T>(
                    this List<T> source,
                    Predicate<T> predicate)
                {
                        int count = 0;
                        for (int i = 0; i < source.Count; i++)
                        {
                                if (predicate(source[i]))
                                        count++;
                        }
                        return count;
                }

                public static void SelectTo<TSource, TResult>(
                    this List<TSource> source,
                    List<TResult> result,
                    Func<TSource, TResult> selector)
                {
                        result.Clear();
                        for (int i = 0; i < source.Count; i++)
                        {
                                result.Add(selector(source[i]));
                        }
                }

                public static void ForEachNoAlloc<T>(
                    this List<T> source,
                    Action<T> action)
                {
                        for (int i = 0; i < source.Count; i++)
                        {
                                action(source[i]);
                        }
                }

                // =========================================================
                // DICTIONARY
                // =========================================================

                public static void WhereTo<TKey, TValue>(
                    this Dictionary<TKey, TValue> source,
                    List<KeyValuePair<TKey, TValue>> result,
                    Predicate<KeyValuePair<TKey, TValue>> predicate)
                {
                        result.Clear();
                        foreach (var kv in source)
                        {
                                if (predicate(kv))
                                        result.Add(kv);
                        }
                }

                public static bool AnyNoAlloc<TKey, TValue>(
                    this Dictionary<TKey, TValue> source,
                    Predicate<KeyValuePair<TKey, TValue>> predicate)
                {
                        foreach (var kv in source)
                        {
                                if (predicate(kv))
                                        return true;
                        }
                        return false;
                }

                public static TValue FirstValueOrDefaultNoAlloc<TKey, TValue>(
                    this Dictionary<TKey, TValue> source,
                    Predicate<KeyValuePair<TKey, TValue>> predicate)
                {
                        foreach (var kv in source)
                        {
                                if (predicate(kv))
                                        return kv.Value;
                        }
                        return default;
                }

                public static void KeysTo<TKey, TValue>(
                    this Dictionary<TKey, TValue> source,
                    List<TKey> result)
                {
                        result.Clear();
                        foreach (var kv in source)
                        {
                                result.Add(kv.Key);
                        }
                }

                public static void ValuesTo<TKey, TValue>(
                    this Dictionary<TKey, TValue> source,
                    List<TValue> result)
                {
                        result.Clear();
                        foreach (var kv in source)
                        {
                                result.Add(kv.Value);
                        }
                }

                public static void ForEachNoAlloc<TKey, TValue>(
                    this Dictionary<TKey, TValue> source,
                    Action<TKey, TValue> action)
                {
                        foreach (var kv in source)
                        {
                                action(kv.Key, kv.Value);
                        }
                }
        }
}

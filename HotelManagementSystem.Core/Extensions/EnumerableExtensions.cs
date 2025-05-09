using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelManagementSystem.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source)
        {
            return source == null || !source.Any();
        }

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T>? source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        public static IEnumerable<TResult> SelectNotNull<TSource, TResult>(
            this IEnumerable<TSource>? source, 
            Func<TSource, TResult> selector)
        {
            return source.EmptyIfNull()
                .Select(selector)
                .Where(item => item != null)!;
        }

        public static decimal Sum<TSource>(
            this IEnumerable<TSource>? source,
            Func<TSource, decimal?> selector)
        {
            return source.EmptyIfNull()
                .Select(selector)
                .Where(value => value.HasValue)
                .Sum(value => value!.Value);
        }

        public static IEnumerable<T> Distinct<T, TKey>(
            this IEnumerable<T>? source,
            Func<T, TKey> keySelector)
        {
            return source.EmptyIfNull()
                .GroupBy(keySelector)
                .Select(group => group.First());
        }
    }
} 
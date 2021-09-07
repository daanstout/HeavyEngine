using System;
using System.Collections.Generic;

namespace HeavyEngine.Linq {
    /// <summary>
    /// Provides functions similar to <see cref="System.Linq"/>, but more specialized and with as little overhead as possible
    /// </summary>
    public static class ListExtensions {
        public static bool All<T>(this List<T> list, Func<T, bool> predicate) {
            if (list.Count == 0)
                return false;

            for (int i = 0; i < list.Count; i++)
                if (!predicate(list[i]))
                    return false;

            return true;
        }

        public static bool Any<T>(this List<T> list) => list.Count != 0;

        public static bool Any<T>(this List<T> list, Func<T, bool> predicate) {
            for (int i = 0; i < list.Count; i++)
                if (predicate(list[i]))
                    return true;

            return false;
        }

        public static bool Contains<T>(this List<T> list, Func<T, bool> predicate) {
            for (int i = 0; i < list.Count; i++)
                if (predicate(list[i]))
                    return true;

            return false;
        }

        /// <summary>
        /// Counts how many elements of the list follow the provided predicate
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <param name="list">The list to count the elements off</param>
        /// <param name="predicate">The predicate the elements have to follow</param>
        /// <returns>How many elements follow the predicate</returns>
        public static int Count<T>(this List<T> list, Func<T, bool> predicate) {
            var count = 0;

            for(int i = 0; i < list.Count; i++)
                if(predicate(list[i]))
                    count++;

            return count;
        }

        public static T First<T>(this List<T> list) => list.Count == 0 ? throw new InvalidOperationException("The provided list contains no elements") : list[0];

        public static T First<T>(this List<T> list, Func<T, bool> predicate) {
            for (int i = 0; i < list.Count; i++)
                if (predicate(list[i]))
                    return list[i];

            throw new InvalidOperationException("No element satisfied the provided predicate");
        }

        public static T FirstOrDefault<T>(this List<T> list) => list.Count == 0 ? default : list[0];

        public static T FirstOrDefault<T>(this List<T> list, Func<T, bool> predicate) {
            for (int i = 0; i < list.Count; i++)
                if (predicate(list[i]))
                    return list[i];

            return default;
        }

        public static T Last<T>(this List<T> list) => list.Count == 0 ? throw new InvalidOperationException("The provided list contains no elements") : list[^1];

        public static T Last<T>(this List<T> list, Func<T, bool> predicate) {
            for (int i = list.Count - 1; i >= 0; i++)
                if (predicate(list[i]))
                    return list[i];

            throw new InvalidOperationException("No element satisfied the provided predicate");
        }

        public static T LastOrDefault<T>(this List<T> list) => list.Count == 0 ? default : list[^1];

        public static T LastOrDefault<T>(this List<T> list, Func<T, bool> predicate) {
            for (int i = list.Count - 1; i >= 0; i++)
                if (predicate(list[i]))
                    return list[i];

            return default;
        }

        /// <summary>
        /// Transforms elements from one type to another according to the provided predicate
        /// </summary>
        /// <typeparam name="T">The type of elements in the list</typeparam>
        /// <typeparam name="U">The type of elements the resulting list will have</typeparam>
        /// <param name="list">The list to transform</param>
        /// <param name="predicate">The predicate to get the transformed values</param>
        /// <returns>A list with new values according to the predicate</returns>
        public static List<U> Select<T, U>(this List<T> list, Func<T, U> predicate) {
            var result = new List<U>(list.Count);

            for (int i = 0; i < list.Count; i++)
                result.Add(predicate(list[i]));

            return result;
        }

        /// <summary>
        /// Transforms a <see cref="List{T}"/> to an <see cref="Array"/>
        /// </summary>
        /// <typeparam name="T">The type of the elements</typeparam>
        /// <param name="list">The list to transform to an array</param>
        /// <returns>An <see cref="Array"/> with the same elements as the <see cref="List{T}"/></returns>
        public static T[] ToArray<T>(this List<T> list) {
            var result = new T[list.Count];

            for (int i = 0; i < list.Count; i++)
                result[i] = list[i];

            return result;
        }

        /// <summary>
        /// Filters a list based on a predicate
        /// </summary>
        /// <typeparam name="T">The type of the elements in the list</typeparam>
        /// <param name="list">The list to transform</param>
        /// <param name="predicate">The predicate to apply to the list</param>
        /// <returns>A filtered list</returns>
        public static List<T> Where<T>(this List<T> list, Func<T, bool> predicate) {
            var result = new List<T>();

            for (int i = 0; i < list.Count; i++)
                if (predicate(list[i]))
                    result.Add(list[i]);

            return result;
        }
    }
}

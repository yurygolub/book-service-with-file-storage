using System;
using System.Collections.Generic;

namespace BufferArray
{
    /// <summary>
    /// Implements array helper methods.
    /// </summary>
    public static class BufferData
    {
        /// <summary>
        /// Creates array on base of enumerable sequence.
        /// </summary>
        /// <param name="source">The enumerable sequence.</param>
        /// <typeparam name="T">Type of the elements of the sequence.</typeparam>
        /// <returns>Single dimension zero based array.</returns>
        public static (T[] buffer, int count) ToArray<T>(IEnumerable<T> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            (T[] buffer, int count) result = (Array.Empty<T>(), 0);
            ICollection<T> array = source as ICollection<T>;
            if (array != null)
            {
                result.count = array.Count;
                if (result.count > 0)
                {
                    result.buffer = new T[result.count];
                    array.CopyTo(result.buffer, 0);
                }
            }
            else
            {
                foreach (T item in source)
                {
                    if (result.buffer.Length == 0)
                    {
                        result.buffer = new T[4];
                    }
                    else if (result.buffer.Length == result.count)
                    {
                        T[] temp = result.buffer;
                        result.buffer = new T[result.buffer.Length * 2];
                        Array.Copy(temp, 0, result.buffer, 0, temp.Length);
                    }

                    result.buffer[result.count] = item;
                    result.count++;
                }
            }

            return result;
        }
    }
}

namespace SyncfusionGenerateLargePdf
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Applies an action to each element of a sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to invoke an action function on.</param>
        /// <param name="action">A action function to apply to each source element.</param>
        public static void Iterate<T>(this IEnumerable<T> source, Action<T> action)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(action);

            foreach (var item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// Applies an action to each element of a sequence, incorporating the element's index.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to invoke an action function on.</param>
        /// <param name="action">A action function to apply to each source element; the second parameter of the 
        /// function represents the index of the source element.</param>
        public static void Iterate<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(action);

            var index = 0;
            foreach (var item in source)
            {
                action(item, index++);
            }
        }
    }
}

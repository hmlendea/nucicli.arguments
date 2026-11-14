using System.Collections;
using System.Collections.Generic;

namespace NuciCLI.Arguments
{
    /// <summary>
    /// A collection of parsed command line arguments.
    /// </summary>
    public class ArgumentsCollection : IEnumerable<KeyValuePair<string, object?>>
    {
        private readonly Dictionary<string, object?> values = [];

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the argument.</param>
        /// <returns>The value associated with the key, or null if the key does not exist.</returns>
        public object? this[string key]
        {
            get
            {
                if (!values.ContainsKey(key))
                {
                    return null;
                }

                return values[key];
            }
            set => values[key] = value;
        }

        /// <summary>
        /// Checks if the collection contains a value for the specified key.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key exists in the collection; otherwise, false.</returns>
        public bool Has(string key) => values.ContainsKey(key);

        /// <summary>
        /// Gets the value associated with the specified key, cast to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to cast the value to.</typeparam>
        /// <param name="key">The key of the argument.</param>
        /// <returns>The value associated with the key, cast to the specified type.</returns>
        public T Get<T>(string key) => (T)values[key]!;

        /// <summary>
        /// Returns an enumerator that iterates through the collection of arguments.
        /// </summary>
        /// <returns>An enumerator for the collection of arguments.</returns>
        public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
            => values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
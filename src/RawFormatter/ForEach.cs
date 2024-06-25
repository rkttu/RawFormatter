using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RawFormatter
{
    /// <summary>
    /// Represents a for-each loop.
    /// </summary>
    public sealed class ForEach : List<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForEach"/> class.
        /// </summary>
        /// <param name="collection">
        /// The collection to iterate over.
        /// </param>
        public ForEach(IEnumerable collection) { _collectionEval = () => collection; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForEach"/> class.
        /// </summary>
        /// <param name="collectionEval">
        /// Delegate that returns the collection to iterate over.
        /// </param>
        public ForEach(Func<IEnumerable> collectionEval) { _collectionEval = collectionEval; }

        private readonly Func<IEnumerable> _collectionEval;

        /// <summary>
        /// Returns a rendered string.
        /// </summary>
        /// <returns>
        /// A rendered string.
        /// </returns>
        public override string ToString()
        {
            var list = new List<string>();

            foreach (var eachItem in _collectionEval.Invoke())
            {
                var block = this.ElementAtOrDefault(0);

                if (block is Delegate callback &&
                    callback.TryInvokeWithSingleParameter(eachItem, eachItem.GetType(), out var result))
                    list.Add(Convert.ToString(result) ?? string.Empty);
                else
                    list.Add(Convert.ToString(block) ?? string.Empty);
            }

            return string.Join(Environment.NewLine, list);
        }
    }
}

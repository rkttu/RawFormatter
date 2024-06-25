using System;
using System.Collections.Generic;
using System.Linq;

namespace RawFormatter
{
    /// <summary>
    /// Represents a for loop.
    /// </summary>
    public sealed class For : List<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="For"/> class.
        /// </summary>
        /// <param name="count">
        /// The number of iterations.
        /// </param>
        public For(int count) { _countEval = () => count; }

        /// <summary>
        /// Initializes a new instance of the <see cref="For"/> class.
        /// </summary>
        /// <param name="countEval">
        /// Delegate that returns the number of iterations.
        /// </param>
        public For(Func<int> countEval) { _countEval = countEval; }

        private readonly Func<int> _countEval;

        /// <summary>
        /// Returns a rendered string.
        /// </summary>
        /// <returns>
        /// A rendered string.
        /// </returns>
        public override string ToString()
        {
            var list = new List<string>();

            for (var i = 0; i < _countEval.Invoke(); i++)
            {
                var block = this.ElementAtOrDefault(0);

                if (block is Delegate callback && callback.TryInvokeWithSingleParameter(i, i.GetType(), out var result))
                    list.Add(Convert.ToString(result) ?? string.Empty);
                else
                    list.Add(Convert.ToString(block) ?? string.Empty);
            }

            return string.Join(Environment.NewLine, list);
        }
    }
}

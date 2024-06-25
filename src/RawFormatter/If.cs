using System;
using System.Collections.Generic;
using System.Linq;

namespace RawFormatter
{
    /// <summary>
    /// Represents an if-else block.
    /// </summary>
    /// <remarks>
    /// Despite of this class inherits the list, but only first two elements are used.
    /// First element treated as the true block, and the second element treated as the false block.
    /// </remarks>
    public class If : List<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="If"/> class.
        /// </summary>
        /// <param name="condition">
        /// The condition to evaluate.
        /// </param>
        public If(bool condition) { _conditionEval = () => condition; }

        /// <summary>
        /// Initializes a new instance of the <see cref="If"/> class.
        /// </summary>
        /// <param name="conditionEval">
        /// Delegate that returns the condition to evaluate.
        /// </param>
        public If(Func<bool> conditionEval) { _conditionEval = conditionEval; }

        private readonly Func<bool> _conditionEval;

        /// <summary>
        /// Returns a rendered string.
        /// </summary>
        /// <returns>
        /// A rendered string.
        /// </returns>
        public override string ToString()
        {
            if (_conditionEval.Invoke())
            {
                var trueBlock = this.ElementAtOrDefault(0);

                if (trueBlock is Delegate callback && callback.TryInvokeWithSingleParameter(true, typeof(bool), out var result))
                    return Convert.ToString(result) ?? string.Empty;
                else
                    return Convert.ToString(trueBlock) ?? string.Empty;
            }
            else
            {
                var falseBlock = this.ElementAtOrDefault(1);

                if (falseBlock is Delegate callback && callback.TryInvokeWithSingleParameter(false, typeof(bool), out var result))
                    return Convert.ToString(result) ?? string.Empty;
                else
                    return Convert.ToString(falseBlock) ?? string.Empty;
            }
        }
    }
}

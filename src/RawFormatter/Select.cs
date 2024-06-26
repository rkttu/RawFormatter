using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RawFormatter
{
    /// <summary>
    /// Represents a select (switch) statement with multiple cases.
    /// </summary>
    public class Select : Dictionary<object, object>
    {
        private sealed class __Else__ { internal __Else__() { } }
        private static readonly Lazy<__Else__> _elseKeyFactory = new Lazy<__Else__>(
            () => new __Else__(), LazyThreadSafetyMode.None);

        /// <summary>
        /// Gets the default key for the switch statement.
        /// </summary>
        public static object Else => _elseKeyFactory.Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Select"/> class with the specified condition.
        /// </summary>
        /// <param name="condition">The condition for the switch statement.</param>
        public Select(object condition) { _conditionEval = () => condition; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Select"/> class with the specified condition evaluation function.
        /// </summary>
        /// <param name="conditionEval">The function that evaluates the condition for the switch statement.</param>
        public Select(Func<object> conditionEval) { _conditionEval = conditionEval; }

        private readonly Func<object> _conditionEval;

        /// <summary>
        /// Returns a rendered string based on the condition and the matching case.
        /// </summary>
        /// <returns>A rendered string.</returns>
        public override string ToString()
        {
            var criteria = _conditionEval.Invoke();
            var defaultKey = Keys.FirstOrDefault(x => x is __Else__);
            var defaultBlock = defaultKey != null ? this[defaultKey] : null;

            foreach (var eachPair in this)
            {
                if (object.Equals(criteria, eachPair.Key))
                {
                    var matchingBlock = eachPair.Value;

                    if (matchingBlock is Delegate callback && callback.TryInvokeWithSingleParameter(criteria, criteria.GetType(), out var result))
                        return Convert.ToString(result) ?? string.Empty;
                    else
                        return Convert.ToString(matchingBlock) ?? string.Empty;
                }
            }

            if (defaultBlock != null)
            {
                if (defaultBlock is Delegate defaultCallback && defaultCallback.TryInvokeWithSingleParameter(criteria, criteria.GetType(), out var defaultResult))
                    return Convert.ToString(defaultResult) ?? string.Empty;
                else
                    return Convert.ToString(defaultBlock) ?? string.Empty;
            }

            return string.Empty;
        }
    }
}

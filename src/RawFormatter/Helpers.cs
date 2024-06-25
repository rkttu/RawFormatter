using System;

namespace RawFormatter
{
    internal static class Helpers
    {
        /// <summary>
        /// Invokes the delegate with a single parameter dynamically.
        /// </summary>
        /// <param name="del">
        /// The delegate to invoke.
        /// </param>
        /// <param name="parameter">
        /// The parameter to pass to the delegate.
        /// </param>
        /// <param name="parameterType">
        /// The type of the parameter. This parameter will be used to check the parameter type of the delegate.
        /// </param>
        /// <param name="result">
        /// The result of the invocation.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the invocation is successful; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryInvokeWithSingleParameter(this Delegate del, object parameter, Type parameterType, out object result)
        {
            result = null;

            if (del == null)
                return false;

            var method = del.Method;
            var parameters = method.GetParameters();
            var returnType = method.ReturnType;

            if (parameters.Length != 1 || parameters[0].ParameterType != parameterType)
                return false;

            var invokeResult = del.DynamicInvoke(parameter);

            if (returnType == typeof(void))
                return false;

            result = invokeResult;

            return true;
        }
    }
}

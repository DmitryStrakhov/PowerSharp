using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace PowerSharp {
    public static class Guard {
        [
            MethodImpl(MethodImplOptions.AggressiveInlining),
            Conditional("DEBUG"),
            AssertionMethod
        ]
        public static void IsNotNull<T>([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] T value, [NotNull] string argument)
            where T : class {

            if(value == null)
                throw new ArgumentNullException(argument);
        }
        [
            MethodImpl(MethodImplOptions.AggressiveInlining),
            Conditional("DEBUG"),
            AssertionMethod
        ]
        public static void IsNotEmpty([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] string value, [NotNull] string argumentName) {
            if(value == null)
                throw new ArgumentNullException(argumentName);
            if(string.IsNullOrEmpty(value))
                throw new ArgumentException(argumentName);
        }
    }
}
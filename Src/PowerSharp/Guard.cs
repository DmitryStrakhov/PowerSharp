using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PowerSharp {
    public static class Guard {
        [
            MethodImpl(MethodImplOptions.AggressiveInlining),
            Conditional("DEBUG")
        ]
        public static void IsNotNull<T>(T value, string argument)
            where T : class {

            if(value == null)
                throw new ArgumentNullException(argument);
        }
    }
}
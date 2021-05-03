using System;

namespace PowerSharp.Extensions {
    public static class ClassExtensions {
        public static T With<T>(this T @this, Action<T> action)
            where T : class {

            Guard.IsNotNull(@this, nameof(@this));
            Guard.IsNotNull(action, nameof(action));

            action(@this);
            return @this;
        }
        public static T With<T, TI>(this T @this, TI arg, Action<T, TI> action)
            where T : class {

            Guard.IsNotNull(@this, nameof(@this));
            Guard.IsNotNull(action, nameof(action));

            action(@this, arg);
            return @this;
        }
        public static T With<T, TI1, TI2>(this T @this, TI1 arg1, TI2 arg2, Action<T, TI1, TI2> action)
            where T : class {

            Guard.IsNotNull(@this, nameof(@this));
            Guard.IsNotNull(action, nameof(action));

            action(@this, arg1, arg2);
            return @this;
        }
    }
}
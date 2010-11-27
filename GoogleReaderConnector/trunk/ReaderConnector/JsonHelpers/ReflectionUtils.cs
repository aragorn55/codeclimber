using System;

namespace CodeClimber.GoogleReaderConnector.JsonHelpers
{
    internal static class ReflectionUtils
    {
        public static bool IsNullableType(Type t)
        {
            return (t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(Nullable<>)));
        }
    }
}

using System;
using System.Linq;

namespace Miracle.Settings
{
    /// <summary>
    /// Extensions for generic type assignment.
    /// </summary>
    public static class GenericTypeExtensions
    {
        /// <summary>
        /// Test if a generic type definition "to" derives from or implements "from".
        /// </summary>
        public static bool IsGenericTypeDefinitionAssignableFrom(this Type to, Type from)
        {
            if (from == null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            var interfaceTest = new Predicate<Type>(i => i.IsGenericType && i.GetGenericTypeDefinition() == to);

            return interfaceTest(from) || from.GetInterfaces().Any(i => interfaceTest(i));
        }
    }
}

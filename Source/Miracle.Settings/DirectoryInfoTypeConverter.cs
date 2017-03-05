using System;
using System.IO;
using System.Linq;

namespace Miracle.Settings
{
    /// <summary>
    /// Type converter that combines one or more strings into a path, and then converts to a DirectoryInfo object.
    /// Can create or check existance of Directory.
    /// </summary>
    public class DirectoryInfoTypeConverter : ITypeConverter
    {
        private readonly Func<string, string> _pathResolver;
        private readonly bool _required;
        private readonly bool _create;

        /// <summary>
        /// Create instance of converter
        /// </summary>
        /// <param name="pathResolver">Function used to convert relative paths to absolute Directory paths. For a web site this could be HostingEnvironment.MapPath. For an application use a function like Path.GetFullPath.</param>
        /// <param name="required">The Directory must exist or an error is thrown.</param>
        /// <param name="create">Create the Directory if it doesnt exist.</param>
        public DirectoryInfoTypeConverter(Func<string, string> pathResolver, bool required = true, bool create = false)
        {
            _pathResolver = pathResolver;
            _required = required;
            _create = create;
        }

        private string MapPath(object[] values)
        {
            if (values.Length > 0 && values.All(x => x is string) && !string.IsNullOrEmpty((string) values[0]))
            {
                try
                {
                    return _pathResolver(Path.Combine(values.Cast<string>().ToArray()));
                }
                catch
                {
                    // ignored
                }
            }
            return null;
        }

        /// <summary>
        /// Check if <param name="values"/> can be converted to type <param name="conversionType"/>
        /// </summary>
        /// <param name="values">Values to convert</param>
        /// <param name="conversionType">Destination type to convert to</param>
        /// <returns>True if type converter is able to convert values to desired type, otherwise false</returns>
        public bool CanConvert(object[] values, Type conversionType)
        {
            return conversionType == typeof(DirectoryInfo) && MapPath(values) != null;
        }

        /// <summary>
        /// Convert <param name="values"/> into instance of type <param name="conversionType"/>
        /// </summary>
        /// <param name="values">Values to convert</param>
        /// <param name="conversionType">The type of object to return.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <returns>Instance of type <param name="conversionType"/> or null if unable to convert</returns>
        public object ChangeType(object[] values, Type conversionType, IFormatProvider formatProvider)
        {
            var path = MapPath(values);
            DirectoryInfo fi = new DirectoryInfo(path);
            if (!fi.Exists)
            {
                if (_create)
                {
                    // Create Directory
                    fi.Create();
                }
                if (_required)
                {
                    throw new DirectoryNotFoundException(path);
                }
            }
            return fi;
        }
    }
}

using System;
using System.IO;
using System.Linq;

namespace Miracle.Settings
{
    /// <summary>
    /// Type converter that combines one or more strings into a path, and then converts to a FileInfo object.
    /// Can create or check existance of file.
    /// </summary>
    public class FileInfoTypeConverter : ITypeConverter
    {
        private readonly Func<string, string> _pathResolver;
        private readonly bool _required;
        private readonly bool _create;

        /// <summary>
        /// Create instance of converter
        /// </summary>
        /// <param name="pathResolver">Function used to convert relative paths to absolute file paths. For a web site this could be HostingEnvironment.MapPath. For an application use a function like Path.GetFullPath.</param>
        /// <param name="required">The file must exist or an error is thrown.</param>
        /// <param name="create">Create the file if it doesnt exist.</param>
        public FileInfoTypeConverter(Func<string, string> pathResolver, bool required = false, bool create = false)
        {
            _pathResolver = pathResolver;
            _required = required;
            _create = create;
        }

        /// <summary>
        /// Attempt to map a relative path to a full path using path resolver.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private string MapPath(object[] values)
        {
            if (values.Length > 0 && values.All(x => x is string) && !string.IsNullOrEmpty((string) values[0]))
            {
                return _pathResolver(Path.Combine(values.Cast<string>().ToArray()));
            }
            return null;
        }

        /// <summary>
        /// Check if <paramref name="values"/> can be converted to type <paramref name="conversionType"/>
        /// </summary>
        /// <param name="values">Values to convert</param>
        /// <param name="conversionType">Destination type to convert to</param>
        /// <returns>True if type converter is able to convert values to desired type, otherwise false</returns>
        public bool CanConvert(object[] values, Type conversionType)
        {
            return conversionType == typeof(FileInfo) &&  values.Length > 0 && values.All(x => x is string) && !string.IsNullOrEmpty((string)values[0]);
        }

        /// <summary>
        /// Convert <paramref name="values"/> into instance of type <paramref name="conversionType"/>
        /// </summary>
        /// <param name="values">Values to convert</param>
        /// <param name="conversionType">The type of object to return.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <returns>Instance of type <paramref name="conversionType"/> or null if unable to convert</returns>
        public object ChangeType(object[] values, Type conversionType, IFormatProvider formatProvider)
        {
            var fileName = MapPath(values);
            var fi = new FileInfo(fileName);
            if (!fi.Exists)
            {
                if (_create)
                {
                    using (fi.Create())
                    {
                        // Create file and close it immediately
                    }
                }
                if (_required)
                {
                    throw new FileNotFoundException(fileName);
                }
            }
            return fi;
        }
    }
}

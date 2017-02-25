using System;
using System.Configuration;
using System.IO;
using System.Linq;
using Miracle.Settings.Properties;

namespace Miracle.Settings
{
    /// <summary>
    /// Type converter that combines one or more strings into a path, and then converts to a FileInfo object.
    /// Can create or check existance of file.
    /// </summary>
    public class FileInfoTypeConverter : ITypeConverter
    {
        public Func<string, string> PathResolver { get; }
        public bool Required { get; }
        public bool Create { get; }

        /// <summary>
        /// Create instance of converter
        /// </summary>
        /// <param name="pathResolver">Function used to convert relative paths to absolute file paths. For a web site this could be HostingEnvironment.MapPath. For an application use a function like Path.GetFullPath.</param>
        /// <param name="required">The file must exist or an error is thrown.</param>
        /// <param name="create">Create the file if it doesnt exist.</param>
        public FileInfoTypeConverter(Func<string, string> pathResolver, bool required = false, bool create = false)
        {
            PathResolver = pathResolver;
            Required = required;
            Create = create;
        }

        private string MapPath(object[] values)
        {
            if (values.Length > 0 && values.All(x => x is string) && !string.IsNullOrEmpty((string) values[0]))
            {
                try
                {
                    return PathResolver(Path.Combine(values.Cast<string>().ToArray()));
                }
                catch
                {
                    // ignored
                }
            }
            return null;
        }

        public bool CanConvert(object[] values, Type conversionType)
        {
            return conversionType == typeof(FileInfo) && MapPath(values) != null;
        }

        public object ChangeType(object[] values, Type conversionType, IFormatProvider formatProvider)
        {
            var fileName = MapPath(values);
            FileInfo fi = new FileInfo(fileName);
            if (!fi.Exists)
            {
                if (Create)
                {
                    // Create file and close it immediately
                    using (fi.Create())
                    {
                    }
                }
                if (Required)
                {
                    throw new FileNotFoundException(fileName);
                }
            }
            return fi;
        }
    }
}

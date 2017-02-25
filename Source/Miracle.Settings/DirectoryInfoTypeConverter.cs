using System;
using System.Configuration;
using System.IO;
using System.Linq;
using Miracle.Settings.Properties;

namespace Miracle.Settings
{
    /// <summary>
    /// Type converter that combines one or more strings into a path, and then converts to a DirectoryInfo object.
    /// Can create or check existance of Directory.
    /// </summary>
    public class DirectoryInfoTypeConverter : ITypeConverter
    {
        public Func<string, string> PathResolver { get; }
        public bool Required { get; }
        public bool Create { get; }

        /// <summary>
        /// Create instance of converter
        /// </summary>
        /// <param name="pathResolver">Function used to convert relative paths to absolute Directory paths. For a web site this could be HostingEnvironment.MapPath. For an application use a function like Path.GetFullPath.</param>
        /// <param name="required">The Directory must exist or an error is thrown.</param>
        /// <param name="create">Create the Directory if it doesnt exist.</param>
        public DirectoryInfoTypeConverter(Func<string, string> pathResolver, bool required = true, bool create = false)
        {
            PathResolver = pathResolver;
            Required = required;
            Create = create;
        }

        private string MapPath(object[] values)
        {
			if (values.Length > 0 && values.All(x => x is string) && !string.IsNullOrEmpty((string)values[0]))
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
            return conversionType == typeof(DirectoryInfo) && MapPath(values) != null;
        }

        public object ChangeType(object[] values, Type conversionType, IFormatProvider formatProvider)
        {
            var path = MapPath(values);
            DirectoryInfo fi = new DirectoryInfo(path);
            if (!fi.Exists)
            {
                if (Create)
                {
                    // Create Directory
                    fi.Create();
                }
                if (Required)
                {
                    throw new DirectoryNotFoundException(path);
                }
            }
            return fi;
        }
    }
}

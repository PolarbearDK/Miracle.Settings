using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Miracle.Settings
{
	public class FileInfoTypeConverter : ITypeConverter
	{
		public Func<string, string> PathResolver { get; }
		public bool Required { get; }
		public bool Create { get; }

		/// <summary>
		/// Create instance of converter
		/// </summary>
		/// <param name="pathResolver">Function used to convert relative paths to absolute file paths. For a web site this could be HttpServerUtility.MapPath. For an application use a function like Path.GetFullPath.</param>
		/// <param name="required">The file must exist or an error is thrown.</param>
		/// <param name="create">Create the file if it doesnt exist.</param>
		public FileInfoTypeConverter(Func<string, string> pathResolver, bool required = false, bool create = false)
		{
			PathResolver = pathResolver;
			Required = required;
			Create = create;
		}

		public bool CanConvert(object[] values, Type conversionType)
		{
			return conversionType == typeof(FileInfo)
				&& values.Length == 1
				&& values[0] is string;
		}

		public object ChangeType(object[] values, Type conversionType)
		{
			switch (values.Length)
			{
				case 1:
					string path = PathResolver((string) values[0]);
					FileInfo fi = new FileInfo(path);
					if (Required)
					{
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
								throw new FileNotFoundException(path);
							}
						}
					}
					return fi;
				default:
					throw new ConfigurationErrorsException("Wrong number of values provided for type converter.");
			}
		}
	}

	/// <summary>
    /// Type converter that combines several strings into a valid path
    /// </summary>
    public class PathTypeConverter : ITypeConverter
    {
        public bool CanConvert(object[] values, Type conversionType)
        {
            return conversionType == typeof(string) && values.Length > 0 && values.All(x=>x is string);
        }

        public object ChangeType(object[] values, Type conversionType)
        {
           return Path.Combine(values.Cast<string>().ToArray());
        }
    }
}

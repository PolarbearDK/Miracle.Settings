using System;
using System.Linq;

namespace Miracle.Settings
{
	[Flags]
	public enum PathOptions
	{
		MustExist = 1,
		Create = 2,
	}

	/// <summary>
    /// Describe how to construct the value of a property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SettingAttribute : Attribute
    {
        /// <summary>
        /// Name of setting
        /// </summary>
        public string Name { get; set; }

		/// <summary>
		/// Name of setting
		/// </summary>
		public PathOptions PathOptions { get; set; }

		/// <summary>
		/// Add additional settings reference used by typeconverter to construct this property
		/// </summary>
		public string Reference
        {
            get { return References != null ? References.FirstOrDefault() : null; }
            set { References = new[] {value}; }
        }
        /// <summary>
        /// Add additional setting references used by typeconverter to construct this property
        /// </summary>
        public string[] References { get; set; }

        /// <summary>
        /// Construct property value using specific type converter
        /// </summary>
        public Type TypeConverter { get; set; } 

        /// <summary>
        /// Construct setting attribute
        /// </summary>
        public SettingAttribute()
        {
        }

        /// <summary>
        /// Construct setting attribute with specific name
        /// </summary>
        /// <param name="name"></param>
        public SettingAttribute(string name)
        {
            Name = name;
        }
    }
}
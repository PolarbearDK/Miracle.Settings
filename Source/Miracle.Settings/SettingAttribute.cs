using System;

namespace Miracle.Settings
{
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
        /// Add additional settings reference used by typeconverter to construct this property
        /// </summary>
        public string Reference
        {
            get { throw new NotImplementedException(); }
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
        /// Split string into string array before converting into Array or List 
        /// Ignored for other properties.
        /// </summary>
        public char Separator
        {
            get { throw new NotImplementedException(); }
            set { Separators = new[] {value}; }
        }

        /// <summary>
        /// Split string into string array before converting into Array or List 
        /// Ignored for other properties.
        /// </summary>
        public char[] Separators { get; set; }

        /// <summary>
        /// Used with Separators. Default: RemoveEmptyEntries
        /// </summary>
        public StringSplitOptions StringSplitOptions { get; set; } = StringSplitOptions.RemoveEmptyEntries;

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
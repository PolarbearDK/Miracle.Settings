using System;

namespace Miracle.Settings
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SettingAttribute : Attribute
    {
        public string Name { get; }

        public SettingAttribute(string name)
        {
            Name = name;
        }
    }
}
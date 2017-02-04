using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Miracle.Settings.Tests
{
    public class Nested
    {
        public string Foo { get; set; }
        public int Bar { get; set; }
    }

    public class DefaultSettings
    {
        // -- Vefault values --
        [DefaultValue("Default Hello World")]
        public string DefaultString { get; private set; }

        [DefaultValue(BindingFlags.Instance | BindingFlags.Public)]
        public BindingFlags DefaultEnum { get; private set; }

        [DefaultValue("1966-06-11T12:34:56.7890000+01:00")]
        public DateTime DefaultDateTime { get; private set; }

        [DefaultValue("1.02:03:04")]
        public TimeSpan DefaultTimeSpan { get; private set; }

        [DefaultValue("https://foo.bar")]
        public Uri DefaultUri { get; private set; }

        [DefaultValue("EE58EE2B-4CE6-44A4-8773-EC4E283146EB")]
        public Guid DefaultGuid { get; private set; }

        [DefaultValue(new[] {"foo","bar"})]
        public string[] DefaultArray { get; private set; }
    }

    public class SimpleSettings
    {
        // -- Simple values --
        public string String { get; private set; }
        public BindingFlags Enum { get; private set; }
        public DateTime DateTime { get; private set; }
        public TimeSpan TimeSpan { get; private set; }
        public Uri Uri { get; private set; }
        public Guid Guid { get; private set; }
    }

    public class SimpleSettingsWithSettingName
    {
        // -- Values with Setting Attribute --
        [Setting("String")]
        public string String1 { get; private set; }
        [Setting(Name="Enum")]
        public BindingFlags Enum2 { get; private set; }
        [Setting("DateTime")]
        public DateTime DateTime3 { get; private set; }
        [Setting("TimeSpan")]
        public TimeSpan TimeSpan4 { get; private set; }
        [Setting("Uri")]
        public Uri Uri5 { get; private set; }
        [Setting("Guid")]
        public Guid Guid6 { get; private set; }
    }

	public class PathSettings
	{
		// -- Values with Setting Attribute --
		[Setting(PathOptions=PathOptions.MustExist)]
		public FileInfo SimpleFile { get; private set; }
		[Setting("FullFile")]
		public FileInfo FullFileInfo { get; private set; }
		[Setting("RelativeFile")]
		public FileInfo RelativeFileInfo { get; private set; }

		[Setting(PathOptions = PathOptions.MustExist)]
		public DirectoryInfo SimpleDirectory { get; private set; }
		[Setting("FullDirectory")]
		public DirectoryInfo FullDirInfo { get; private set; }
		[Setting("RelativeDirectory")]
		public DirectoryInfo RelativeDirInfo { get; private set; }
	}

	public class NestedSettings
    {
        public Nested MyNestedProperty { get; set; }
    }

    public class ArraySettings
    {
        public string[] MySimpleArrayProperty { get; private set; }
        public decimal[] MyNumericArrayProperty { get; private set; }
        public Nested[] MyArrayProperty { get; private set; }
    }

    public class ListSettings
    {
        public IList<string> MySimpleListProperty { get; private set; }
        public List<Nested> MyListProperty { get; private set; }
    }

    public class DictionarySettings
    {
        public Dictionary<string, string> MySimpleDictionaryProperty { get; private set; }
        public Dictionary<string, byte> MyNumericDictionaryProperty { get; private set; }
        public Dictionary<string, Nested> MyDictionaryProperty { get; private set; }
    }

    enum AnimalType { Parrot, Rabbit, Fox, Otter };

    class Animal
    {
        public string Name { get; set; }
        public bool Talks { get; set; }
    }

    public class ReferenceSettings
    {
        // -- Settings with References --
        [Setting(Name="BaseUrl")]
        public Uri MyUrl { get; set; }
        [Setting(Reference="BaseUrl")]
        public Uri PageUrl { get; set; }
        [Setting(References = new [] {"BaseUrl"})]
        public Uri LoginUrl { get; set; }
    }

    public class PathTypeConverterSettings
    {
        // -- Settings with TypeConverter --
        [Setting(Reference="RootFolder", TypeConverter = typeof(PathTypeConverter))]
        public string Upload { get; set; }
        // -- Settings with TypeConverter --
        [Setting(Reference="RootFolder", TypeConverter = typeof(PathTypeConverter))]
        public string Download { get; set; }
        // -- Settings with TypeConverter --
        [Setting(References= new[] { "RootFolder", "Upload"}, TypeConverter = typeof(PathTypeConverter))]
        public string Sub { get; set; }
    }

    public class MissingStringSetting
    {
        public string MissingString { get; set; }
    }

    public class MissingDateTimeSetting
    {
        public string MissingDateTime { get; set; }
    }

    public class MissingIntSetting
    {
        public string MissingInt { get; set; }
    }

    public class MissingTimeSpanSetting
    {
        public string MissingTimeSpan { get; set; }
    }

    public class MissingUriSetting
    {
        public Uri MissingUri { get; set; }
    }

    public class BadTypeConverterSetting
    {
        [Setting(TypeConverter = typeof(string))]
        [DefaultValue("Bar")]
        public string Foo { get; set; }
    }

    public class TypeConversionFailureSetting
    {
        [DefaultValue("XYZ")]
        public int Foo { get; set; }
    }

    public class ExplicitTypeConversionFailureSetting
    {
        [DefaultValue("http://XYZ")]
        [Setting(TypeConverter = typeof(UriTypeConverter))]
        public int Foo { get; set; }
    }
}

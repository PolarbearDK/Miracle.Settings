using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Reflection;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

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

		[DefaultValue(typeof(AccessViolationException))]
		public Type DefaultType { get; private set; }

		[DefaultValue("https://foo.bar")]
		public Uri DefaultUri { get; private set; }

        [DefaultValue("EE58EE2B-4CE6-44A4-8773-EC4E283146EB")]
        public Guid DefaultGuid { get; private set; }

        [DefaultValue("10.42.42.42")]
        public IPAddress DefaultIp { get; private set; }

        [DefaultValue(new[] {"foo","bar"})]
        public string[] DefaultArray { get; private set; }
    }

    public class SimpleSettings
    {
        // -- Simple values --
        public string String { get; private set; }
        
        
        public AnimalType? NullableEnum { get; private set; }
        
        public int? NullableInt { get; private set; }
        
        public BindingFlags Enum { get; private set; }
        public DateTime DateTime { get; private set; }
        public TimeSpan TimeSpan { get; private set; }
	    public Type Type { get; private set; }
	    public Uri Uri { get; private set; }
	    public Guid Guid { get; private set; }
	    public IPAddress IPAddress { get; private set; }
    }

    public class IgnoreSettings
    {
        // Required
        public string String { get; private set; }

		// Ignored using IgnoreAttribute
        [Ignore]
        public string Ignored { get; private set; }
        [Ignore]
        public Nested NestedIgnored { get; private set; }
        [Ignore]
        public string[] NestedIgnoredArray { get; private set; }

		// Optional by convension
	    public TimeSpan? TimeSpanIgnored { get; private set; }
	}

	public class OptionalSettings
    {
        // Required
        public string String { get; private set; }
        [Optional]
        public string OptionalPresent { get; private set; }
        [Optional]
        public string OptionalMissing { get; private set; }
        [Optional]
        public Nested OptionalNestedPresent { get; private set; }
        [Optional]
        public Nested OptionalNestedMissing { get; private set; }
        [Optional]
        public string[] OptionalArrayPresent { get; private set; }
        [Optional]
        public string[] OptionalArrayMissing { get; private set; }
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

    public class FileSettings
    {
        public FileInfo SimpleFile { get; private set; }
        public FileInfo PartialFile { get; private set; }
		[Setting("FullFile")]
		public FileInfo FullFileInfo { get; private set; }
		[Setting("RelativeFile", Reference = "RelativeDirectory")]
        public FileInfo RelativeFileInfo { get; private set; }
    }

    public class FileAnnotationSettings
    {
		[Setting(Name = "FileName", References = new[] { "Drive", "Folder" })]
		public FileInfo FullName { get; private set; }
	}

    public class TypeSettings
    {
		public Type MyType { get; private set; }
	}

    public class DirectorySettings
    {
        public DirectoryInfo SimpleDirectory { get; private set; }
		public DirectoryInfo PartialDirectory { get; private set; }
		[Setting("FullDirectory")]
        public DirectoryInfo FullDirectoryInfo { get; private set; }
		[Setting("RelativeDirectory2", Reference = "RelativeDirectory")]
		public DirectoryInfo RelativeDirectoryInfo { get; private set; }
	}

    public class NestedSettings
    {
        public Nested MyNestedProperty { get; internal set; }
    }

    public class InlineSettings
    {
        [Setting(Inline = true)]
        public Nested MyNestedProperty { get; internal set; }
        public TimeSpan Baz { get; set; }
    }

    public class OptionalInlineSettings
    {
        [Setting(Inline = true)]
        [Optional]
        public TypeSettings MyNestedProperty { get; internal set; }
        public TimeSpan Baz { get; set; }
    }

    public class ArraySettings
    {
        public string[] MySimpleArrayProperty { get; private set; }
        public decimal[] MyNumericArrayProperty { get; private set; }
        public Nested[] MyArrayProperty { get; private set; }
        [Setting("LostNumbers", Separator =',')]
        public int[] LostNumbersArray { get; private set; }
    }

    public class ListSettings
    {
        public IList<string> MySimpleListProperty { get; private set; }
        public List<Nested> MyListProperty { get; private set; }
        [Setting("LostNumbers", Separator = ',')]
        public List<int> LostNumbersList { get; private set; }
    }

    public class DictionarySettings
    {
        public Dictionary<string, string> MySimpleDictionaryProperty { get; private set; }
        public Dictionary<string, byte> MyNumericDictionaryProperty { get; private set; }
        public Dictionary<string, Nested> MyDictionaryProperty { get; private set; }
    }

    public enum AnimalType { Parrot, Rabbit, Fox, Otter };

    class Animal
    {
        public string Name { get; internal set; }
        public bool Talks { get; internal set; }
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
        public string MissingString { get; private set; }
    }

    public class MissingDateTimeSetting
    {
        public DateTime MissingDateTime { get; private set; }
    }

    public class MissingIntSetting
    {
        public int MissingInt { get; private set; }
    }

    public class MissingTimeSpanSetting
    {
        public TimeSpan MissingTimeSpan { get; private set; }
    }

    public class MissingUriSetting
    {
        public Uri MissingUri { get; private set; }
    }

	public class MissingFileInfoSetting
	{
		public FileInfo MissingFile { get; private set; }
	}

	public class MissingDirectoryInfoSetting
	{
		public DirectoryInfo MissingDir { get; private set; }
	}

    public class MissingInlineSetting
    {
        [Setting(Inline = true)]
        public Nested Inline { get; private set; }
    }

    public class InlineTypeConversionSetting
    {
        [Setting(Inline = true)]
        public MissingIntSetting Inline { get; private set; }
    }

    public class BadTypeConverterSetting
    {
        [Setting(TypeConverter = typeof(string))]
        [DefaultValue("Bar")]
        public string Foo { get; private set; }
    }

    public class TypeConversionFailureSetting
    {
        [DefaultValue("XYZ")]
        public int Foo { get; private set; }
    }

    public class ExplicitTypeConversionFailureSetting
    {
        [DefaultValue("http://XYZ")]
        [Setting(TypeConverter = typeof(UriTypeConverter))]
        public int Foo { get; private set; }
    }

    public interface IMyInterface
    {
        string Foo { get;  }
        int Bar { get; }
    }

    public class MyConcreteImplementation: IMyInterface
    {
        public string Foo { get; private set; }
        public int Bar { get; private set; }
    }

    public class InterfaceMapping
    {
        [Setting(ConcreteType = typeof(MyConcreteImplementation))]
        public IMyInterface Prop { get; private set; }
    }

    public class BadInterfaceMapping
    {
        [Setting(ConcreteType = typeof(Nested))]
        public IMyInterface Prop { get; private set; }
    }

    public class BadConversion
    {
        public Uri BadUri { get; set; }
    }

    public class MultiLevel1
    {
        public string Foo { get; set; }
        public MultiLevel2 Level2 { get; set; }
    }

    public class MultiLevel2
    {
        public string Bar { get; set; }
        public MultiLevel3 Level3 { get; set; }
    }

    public class MultiLevel3
    {
        public string Baz { get; set; }
    }
}

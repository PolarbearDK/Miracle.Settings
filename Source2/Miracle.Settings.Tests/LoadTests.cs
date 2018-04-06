using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using NUnit.Framework;
using Is = NUnit.DeepObjectCompare.Is;

namespace Miracle.Settings.Tests
{
    [TestFixture]
    public class LoadTests : LoadTestBase
    {
	    public LoadTests()
		    : base(TestSettingsLoader
			    .GetSettingsLoader()
			    .AddProvider(new EnvironmentValueProvider()))
	    {
	    }

	    [Test]
        public void DefaultLoadTest()
        {
            var settings = SettingsLoader.Create<DefaultSettings>();

            // Defaults
            Assert.That(settings.DefaultString, Is.EqualTo("Default Hello World"));
            Assert.That(settings.DefaultEnum, Is.EqualTo(BindingFlags.Instance | BindingFlags.Public));
            Assert.That(settings.DefaultDateTime, Is.EqualTo(new DateTime(1966, 6, 11, 13, 34, 56, DateTimeKind.Local).AddMilliseconds(789)));
            Assert.That(settings.DefaultTimeSpan, Is.EqualTo(new TimeSpan(1, 2, 3, 4)));
            Assert.That(settings.DefaultType, Is.EqualTo(typeof(AccessViolationException)));
            Assert.That(settings.DefaultUri, Is.EqualTo(new Uri("https://foo.bar")));
            Assert.That(settings.DefaultGuid, Is.EqualTo(Guid.Parse("EE58EE2B-4CE6-44A4-8773-EC4E283146EB")));
            Assert.That(settings.DefaultIp, Is.EqualTo(IPAddress.Parse("10.42.42.42")));
            Assert.That(settings.DefaultArray, Is.EqualTo(new[] {"foo", "bar"}));
        }

        [Test]
        public void SimpleLoadTest()
        {
            var settings = SettingsLoader.Create<SimpleSettings>();

            //Simple
            Assert.That(settings.Enum, Is.EqualTo(BindingFlags.NonPublic | BindingFlags.Static));
            Assert.That(settings.String, Is.EqualTo("Hello world"));
            Assert.That(settings.DateTime, Is.EqualTo(new DateTime(2004, 07, 17, 9, 0, 0, DateTimeKind.Local)));
            Assert.That(settings.TimeSpan, Is.EqualTo(new TimeSpan(11, 22, 33, 44, 560)));
            Assert.That(settings.Type, Is.EqualTo(typeof(System.Data.IDbConnection)));
			Assert.That(settings.Uri, Is.EqualTo(new Uri("Http://hello.eu")));
			Assert.That(settings.Guid, Is.EqualTo(Guid.Parse("DCFA0942-0BEC-43E4-8D77-57BA63C7BF7B")));
			Assert.That(settings.IPAddress, Is.EqualTo(IPAddress.Parse("192.168.0.42")));
        }

        [Test]
        public void SettingNameLoadTest()
        {
            var settings = SettingsLoader.Create<SimpleSettingsWithSettingName>();

            //Simple
            Assert.That(settings.Enum2, Is.EqualTo(BindingFlags.NonPublic | BindingFlags.Static));
            Assert.That(settings.String1, Is.EqualTo("Hello world"));
            Assert.That(settings.DateTime3, Is.EqualTo(new DateTime(2004, 07, 17, 9, 0, 0, DateTimeKind.Local)));
            Assert.That(settings.TimeSpan4, Is.EqualTo(new TimeSpan(11, 22, 33, 44, 560)));
            Assert.That(settings.Uri5, Is.EqualTo(new Uri("Http://hello.eu")));
            Assert.That(settings.Guid6, Is.EqualTo(Guid.Parse("DCFA0942-0BEC-43E4-8D77-57BA63C7BF7B")));
        }

        

		[Test]
		public void CustomDateTimeTest()
		{
			var key = "Christmas";

			// Pick any type to use as test type
			var date = new DateTime(2017, 12, 24);
			var value = date.ToString("dd/MM/yyyy");

            // Setup dictionary value provider 
            var settingsLoader = DictionaryValueProvider.CreateSettingsLoader(new Dictionary<string, string>
			{
				{key, value}
			});
			settingsLoader.AddTypeConverter<DateTime>(x => DateTime.Parse(x, System.Globalization.CultureInfo.GetCultureInfo("da-DK")));

            var result = settingsLoader.Create<DateTime>(key);

			Assert.That(result, Is.Not.Null);
			Assert.That(result, Is.EqualTo(date));
		}
        
		[Test]
        public void SettingReferenceLoadTest()
        {
            var settings = SettingsLoader.Create<ReferenceSettings>("SettingReference");

            // Example of loading settings with references to other settings
            Assert.That(settings, Is.DeepEqualTo(
                new ReferenceSettings()
                {
                    MyUrl = new Uri("http://foo.bar"),
                    PageUrl = new Uri(new Uri("http://foo.bar"), "/home/index.aspx"),
                    LoginUrl = new Uri(new Uri("http://foo.bar"), "/login.aspx"),
                }));
        }

        [Test]
        public void SettingAttributesWithTypeConverterLoadTest()
        {
            var settings = SettingsLoader.Create<PathTypeConverterSettings>("TypeConverter");

            //Simple
            // Array
            Assert.That(settings, Is.DeepEqualTo(
                new PathTypeConverterSettings()
                {
                    Upload = @"C:\Foo\Bar\Upload\",
                    Download = @"E:\Download\",
                    Sub = @"C:\Foo\Bar\Upload\Sub\",
                }));
        }

        [Test]
        public void NestedLoadTest()
        {
            var settings = SettingsLoader.Create<NestedSettings>();

            // Nested
            Assert.That(settings, Is.DeepEqualTo(
                new NestedSettings
                {
                    MyNestedProperty = new Nested()
                    {
                        Foo = "Foo string",
                        Bar = 42
                    }
                }));
        }

        [Test]
        public void ArrayLoadTest()
        {
            var settings = SettingsLoader.Create<ArraySettings>();

            // Array
            Assert.That(settings.MySimpleArrayProperty, Is.EqualTo(
                new[]
                {
                    "Foo Primary", "Foo 1", "Foo 2"
                }));
            Assert.That(settings.MyNumericArrayProperty, Is.EqualTo(
                new decimal[]
                {
                    1, 2, 3, 4
                }));
			Assert.That(settings.MyArrayProperty, Is.DeepEqualTo(
                new[]
                {
                    new Nested {Foo = "Foo Primary", Bar = 420},
                    new Nested {Foo = "Foo 1", Bar = 421},
                    new Nested {Foo = "Foo 2", Bar = 422},
                }));
            Assert.That(settings.LostNumbersArray, Is.EqualTo(
                new[]
                {
                    4, 8, 15, 16, 23, 42
                }));
        }

        [Test]
        public void DirectStringArrayLoadTest()
        {
            var settings = SettingsLoader.CreateArray<string>("MySimpleArrayProperty");

            // Array
            Assert.That(settings, Is.EqualTo(
                new[]
                {
                    "Foo Primary", "Foo 1", "Foo 2"
                }));
        }

        [Test]
        public void DirectSplitStringArrayLoadTest()
        {

            var numbers = SettingsLoader.CreateArray<int>("LostNumbers", new[] {','});

            Assert.That(numbers, Is.EqualTo(
                new[]
                {
                    4, 8, 15, 16, 23, 42
                }));
        }

        [Test]
        public void DirectSplitListArrayLoadTest()
        {

            var numbers = SettingsLoader.CreateList<int>("LostNumbers", new[] {','});

            Assert.That(numbers, Is.EqualTo(
                new List<int>
                {
                    4, 8, 15, 16, 23, 42
                }));
        }

        [Test]
        public void DirectNumericArrayLoadTest()
        {
            var settings = SettingsLoader.CreateArray<int>("MyNumericArrayProperty");

            // Array
            Assert.That(settings, Is.DeepEqualTo(
                new int[]
                {
                    1, 2, 3, 4
                }));
        }

		[Test]
        public void DirectClassArrayLoadTest()
        {
            var settings = SettingsLoader.CreateArray<Nested>("MyArrayProperty");

            // Array
            Assert.That(settings, Is.DeepEqualTo(
                new[]
                {
                    new Nested {Foo = "Foo Primary", Bar = 420},
                    new Nested {Foo = "Foo 1", Bar = 421},
                    new Nested {Foo = "Foo 2", Bar = 422},
                }));
        }


        [Test]
        public void ListLoadTest()
        {
            var settings = SettingsLoader.Create<ListSettings>();

            // List
            Assert.That(settings.MySimpleListProperty, Is.DeepEqualTo(
                new List<string>
                {
                    "Foo 1",
                    "Foo 2"
                }));
            Assert.That(settings.MyListProperty, Is.DeepEqualTo(
                new List<Nested>
                {
                    new Nested {Foo = "Foo 1", Bar = 421},
                    new Nested {Foo = "Foo 2", Bar = 422},
                }));
            Assert.That(settings.LostNumbersList, Is.EqualTo(
                new[]
                {
                    4, 8, 15, 16, 23, 42
                }));
        }

        [Test]
        public void DictionaryLoadTest()
        {
            var settings = SettingsLoader.Create<DictionarySettings>();

            // Various Dictionarys
            Assert.That(settings.MySimpleDictionaryProperty, Is.DeepEqualTo(
                new Dictionary<string, string>
                {
                    {"Foo", "oof"},
                    {"Bar", "rab"}
                }));
            Assert.That(settings.MyNumericDictionaryProperty, Is.DeepEqualTo(
                new Dictionary<string, byte>
                {
                    {"Foo", 14},
                    {"Bar", 42}
                }));
            Assert.That(settings.MyDictionaryProperty, Is.DeepEqualTo(
                new Dictionary<string, Nested>
                {
                    {"Key1", new Nested {Foo = "oof1", Bar = 421}},
                    {"Key2", new Nested {Foo = "oof2", Bar = 422}}
                }));
        }


        [Test]
        public void EnumDictionaryLoadTest()
        {
            var settings = SettingsLoader.CreateDictionary<AnimalType, Animal>("Animals");

            // Dictionary with non string TKey
            Assert.That(settings, Is.DeepEqualTo(
                new Dictionary<AnimalType, Animal>
                {
                    {AnimalType.Parrot, new Animal {Name = "Polly", Talks = true}},
                    {AnimalType.Rabbit, new Animal {Name = "Beatrice", Talks = false}},
                    {AnimalType.Fox, new Animal {Name = "John", Talks = false}},
                }));
        }
    }
}

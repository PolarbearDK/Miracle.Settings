using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using NUnit.Framework;
using Is = NUnit.DeepObjectCompare.Is;

namespace Miracle.Settings.Tests
{
    [TestFixture]
    public class LoadTests
    {
        private readonly ISettingsLoader _loader;

        public LoadTests()
        {
            _loader = new SettingsLoader()
                .RemoveTypeConverter(typeof(int))
                .AddTypeConverter(typeof(DateTime), s => XmlConvert.ToDateTime(s, XmlDateTimeSerializationMode.Local))
                .AddProvider(new EnvironmentProvider());
        }

        [Test]
        public void DefaultLoadTest()
        {
            var settings = _loader.Create<DefaultSettings>();

            // Defaults
            Assert.That(settings.DefaultString, Is.EqualTo("Default Hello World"));
            Assert.That(settings.DefaultEnum, Is.EqualTo(BindingFlags.Instance | BindingFlags.Public));
            Assert.That(settings.DefaultDateTime, Is.EqualTo(new DateTime(1966, 6, 11, 13, 34, 56, DateTimeKind.Local).AddMilliseconds(789)));
            Assert.That(settings.DefaultTimeSpan, Is.EqualTo(new TimeSpan(1, 2, 3, 4)));
            Assert.That(settings.DefaultUri, Is.EqualTo(new Uri("https://foo.bar")));
            Assert.That(settings.DefaultGuid, Is.EqualTo(Guid.Parse("EE58EE2B-4CE6-44A4-8773-EC4E283146EB")));
        }

        [Test]
        public void SimpleLoadTest()
        {
            var settings = _loader.Create<SimpleSettings>();

            //Simple
            Assert.That(settings.Enum, Is.EqualTo(BindingFlags.NonPublic | BindingFlags.Static));
            Assert.That(settings.String, Is.EqualTo("Hello world"));
            Assert.That(settings.DateTime, Is.EqualTo(new DateTime(2004, 07, 17, 9, 0, 0, DateTimeKind.Local)));
            Assert.That(settings.TimeSpan, Is.EqualTo(new TimeSpan(11, 22, 33, 44, 560)));
            Assert.That(settings.Uri, Is.EqualTo(new Uri("Http://hello.eu")));
            Assert.That(settings.Guid, Is.EqualTo(Guid.Parse("DCFA0942-0BEC-43E4-8D77-57BA63C7BF7B")));
        }

        [Test]
        public void SettingAttributesLoadTest()
        {
            var settings = _loader.Create<SimpleSettingsWithSettingAttributes>();

            //Simple
            Assert.That(settings.Enum2, Is.EqualTo(BindingFlags.NonPublic | BindingFlags.Static));
            Assert.That(settings.String1, Is.EqualTo("Hello world"));
            Assert.That(settings.DateTime3, Is.EqualTo(new DateTime(2004, 07, 17, 9, 0, 0, DateTimeKind.Local)));
            Assert.That(settings.TimeSpan4, Is.EqualTo(new TimeSpan(11, 22, 33, 44, 560)));
            Assert.That(settings.Uri5, Is.EqualTo(new Uri("Http://hello.eu")));
            Assert.That(settings.Guid6, Is.EqualTo(Guid.Parse("DCFA0942-0BEC-43E4-8D77-57BA63C7BF7B")));
        }

        [Test]
        public void NestedLoadTest()
        {
            var settings = _loader.Create<NestedSettings>();

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
            var settings = _loader.Create<ArraySettings>();

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
        }


        [Test]
        public void ListLoadTest()
        {
            var settings = _loader.Create<ListSettings>();

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
        }

        [Test]
        public void DictionaryLoadTest()
        {
            var settings = _loader.Create<DictionarySettings>();

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
        public void CreateDictionaryLoadTest()
        {
            var settings = _loader.CreateDictionary<AnimalType, Animal>("Animals");

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
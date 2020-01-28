using System;
using System.Collections.Generic;
using Miracle.Settings.Properties;
using NUnit.Framework;
using Is = NUnit.DeepObjectCompare.Is;

namespace Miracle.Settings.Tests
{
    [TestFixture]
    public class InlineTests : LoadTestBase
    {
        public InlineTests()
            : base(
                new SettingsLoader(
                    new DictionaryValueProvider(
                        new Dictionary<string, string>()
                        {
                            {"Foo", "Inline Foo string"},
                            {"Bar", "21"},
                            {"Baz", "01:02:03"},
                            {"MissingInt", "xyz"},
                            {"Partial:Baz", "02:03:04"},

                            {"Fail1:Foo", "zoom"},
                            {"Fail1:Baz", "03:02:01"},

                            {"Fail2:Bar", "117"},
                            {"Fail2:Baz", "03:02:01"},
                        }
                    )
                )
            )
        {
        }

        [Test]
        public void InlineLoadTest()
        {
            var settings = SettingsLoader.Create<InlineSettings>();

            // Nested
            Assert.That(settings, Is.DeepEqualTo(
                new InlineSettings
                {
                    MyNestedProperty = new Nested()
                    {
                        Foo = "Inline Foo string",
                        Bar = 21
                    },
                    Baz = TimeSpan.Parse("01:02:03")

                }));
        }

        [Test]
        public void OptionalInlineLoadTest1()
        {
            var settings = SettingsLoader.Create<OptionalInlineSettings>();

            // Nested
            Assert.That(settings, Is.DeepEqualTo(
                new OptionalInlineSettings()
                {
                    MyNestedProperty = new Nested()
                    {
                        Foo = "Inline Foo string",
                        Bar = 21
                    },
                    Baz = TimeSpan.Parse("01:02:03")
                }));
        }

        [Test]
        public void OptionalInlineLoadTest2()
        {
            var settings = SettingsLoader.Create<OptionalInlineSettings>("Partial");

            // Nested
            Assert.That(settings, Is.DeepEqualTo(
                new OptionalInlineSettings()
                {
                    MyNestedProperty = null,
                    Baz = TimeSpan.Parse("02:03:04")
                }));
        }

        [Test]
        public void OptionalInlineLoadFailTest1()
        {
            AssertThrowsSettingsExceptionMessageTest(
                () => SettingsLoader.Create<OptionalInlineSettings>("Fail1"),
                Resources.MissingValueFormat,
                typeof(int),
                Settings.SettingsLoader.GetSettingKey("Fail1", nameof(Nested.Bar)));
        }

        [Test]
        public void OptionalInlineLoadFailTest2()
        {
            AssertThrowsSettingsExceptionMessageTest(
                () => SettingsLoader.Create<OptionalInlineSettings>("Fail2"),
                Resources.MissingValueFormat,
                typeof(string),
                Settings.SettingsLoader.GetSettingKey("Fail2", nameof(Nested.Foo)));
        }

        [Test]
        public void MissingInlineSettingTest()
        {
            AssertThrowsSettingsExceptionMessageTest<MissingInlineSetting>(
                Resources.MissingValueFormat, typeof(string),
                Settings.SettingsLoader.GetSettingKey(NotFoundPrefix, nameof(Nested.Foo)));
        }

        [Test]
        public void InlineTypeConversionErrorSettingTest()
        {
            AssertThrowsSettingsExceptionMessageTest(
                () => SettingsLoader.Create<InlineTypeConversionSetting>(),
                Resources.ConversionErrorSuffix,
                string.Format(Resources.ConvertValueErrorFormat, "xyz", typeof(int)),
                Settings.SettingsLoader.GetSettingKey(nameof(MissingIntSetting.MissingInt)));
        }
    }
}

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
        public void OptionalInlineLoadTest()
        {
            var settings = SettingsLoader.Create<OptionalInlineSettings>();

            // Nested
            Assert.That(settings, Is.DeepEqualTo(
                new OptionalInlineSettings()
                {
                    MyNestedProperty = null,
                    Baz = TimeSpan.Parse("01:02:03")
                }));
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

using System;
using System.Configuration;
using NUnit.Framework;
using Miracle.Settings.Properties;

namespace Miracle.Settings.Tests
{
    [TestFixture]
    public class LoadFailTests : LoadTestBase
    {
        private const string Prefix = "Missing";

        private string GetKey(params string[] elements)
        {
            return string.Join(".", elements);
        }

        private void AssertThrowsConfigurationErrorsExceptionMessageTest<T>(string format, params object[] args)
        {
            var ex = Assert.Throws<ConfigurationErrorsException>(() => SettingsLoader.Create<T>(Prefix));
            Console.WriteLine(ex);
            var expectedMessage = string.Format(format, args);
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void CreateMissingStringSettingTest()
        {
            AssertThrowsConfigurationErrorsExceptionMessageTest<MissingStringSetting>(
                Resources.MissingValueFormat, GetKey(Prefix, nameof(MissingStringSetting.MissingString)));
        }

        [Test]
        public void CreateMissingDateTimeSettingTest()
        {
            AssertThrowsConfigurationErrorsExceptionMessageTest<MissingDateTimeSetting>(
                Resources.MissingValueFormat, GetKey(Prefix, nameof(MissingDateTimeSetting.MissingDateTime)));
        }

        [Test]
        public void CreateMissingUriSettingTest()
        {
            AssertThrowsConfigurationErrorsExceptionMessageTest<MissingUriSetting>(
                Resources.MissingValueFormat, GetKey(Prefix, nameof(MissingUriSetting.MissingUri)));
        }

        [Test]
        public void CreateMissingTimeSpanSettingTest()
        {
            AssertThrowsConfigurationErrorsExceptionMessageTest<MissingTimeSpanSetting>(
                Resources.MissingValueFormat, GetKey(Prefix, nameof(MissingTimeSpanSetting.MissingTimeSpan)));
        }

        [Test]
        public void CreateMissingIntSettingTest()
        {
            AssertThrowsConfigurationErrorsExceptionMessageTest<MissingIntSetting>(
                Resources.MissingValueFormat, GetKey(Prefix, nameof(MissingIntSetting.MissingInt)));
        }

        [Test]
        public void CreateMissingArraySettingTest()
        {
            AssertThrowsConfigurationErrorsExceptionMessageTest<ArraySettings>(
                Resources.MissingArrayValueFormat, GetKey(Prefix, nameof(ArraySettings.MySimpleArrayProperty)));
        }

        [Test]
        public void CreateMissingListSettingTest()
        {
            AssertThrowsConfigurationErrorsExceptionMessageTest<ListSettings>(
                Resources.MissingListValueFormat, GetKey(Prefix, nameof(ListSettings.MySimpleListProperty)));
        }

        [Test]
        public void CreateMissingDictionarySettingTest()
        {
            AssertThrowsConfigurationErrorsExceptionMessageTest<DictionarySettings>(
                Resources.MissingDictionaryValueFormat, GetKey(Prefix, nameof(DictionarySettings.MySimpleDictionaryProperty)));
        }

        [Test]
        public void CreateMissingNestedSettingTest()
        {
            AssertThrowsConfigurationErrorsExceptionMessageTest<NestedSettings>(
                Resources.MissingValueFormat, GetKey(Prefix, nameof(NestedSettings.MyNestedProperty), nameof(Nested.Foo)));
        }

        [Test]
        public void BadTypeConverterSettingTest()
        {
            AssertThrowsConfigurationErrorsExceptionMessageTest<BadTypeConverterSetting>(
                Resources.CreateTypeConverterErrorFormat, typeof(string));
        }

        [Test]
        public void TypeConversionFailureSettingTest()
        {
            AssertThrowsConfigurationErrorsExceptionMessageTest<TypeConversionFailureSetting>(
                Resources.ConvertValueErrorFormat, "XYZ", typeof(int));
        }

        [Test]
        public void ExplicitTypeConversionFailureSettingTest()
        {
            AssertThrowsConfigurationErrorsExceptionMessageTest<ExplicitTypeConversionFailureSetting>(
                Resources.ExplicitTypeConverterErrorFormat, "http://XYZ",typeof (int));
        }
    }
}
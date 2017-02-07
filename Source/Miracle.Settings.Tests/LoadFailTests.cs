using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Miracle.Settings.Properties;

namespace Miracle.Settings.Tests
{
	[TestFixture]
    public class LoadFailTests : LoadTestBase
    {
		public LoadFailTests()
			: base(new SettingsLoader())
		{
		}

		[Test]
		public void CreateMissingStringTest()
		{
			AssertThrowsConfigurationErrorsExceptionMessageTest<string>(
				Resources.MissingValueFormat, typeof(string), GetKey(NotFoundPrefix));
		}

		[Test]
		public void CreateMissingNumericTest()
		{
			AssertThrowsConfigurationErrorsExceptionMessageTest<int>(
				Resources.MissingValueFormat, typeof(int), GetKey(NotFoundPrefix));
		}

		[Test]
		public void CreateMissingStringSettingTest()
		{
			AssertThrowsConfigurationErrorsExceptionMessageTest<MissingStringSetting>(
				Resources.MissingValueFormat, typeof(String), GetKey(NotFoundPrefix, nameof(MissingStringSetting.MissingString)));
		}

		[Test]
        public void CreateMissingDateTimeSettingTest()
        {
            AssertThrowsConfigurationErrorsExceptionMessageTest<MissingDateTimeSetting>(
                Resources.MissingValueFormat, typeof(DateTime), GetKey(NotFoundPrefix, nameof(MissingDateTimeSetting.MissingDateTime)));
        }

        [Test]
        public void CreateMissingUriSettingTest()
        {
            AssertThrowsConfigurationErrorsExceptionMessageTest<MissingUriSetting>(
                Resources.MissingValueFormat, typeof(Uri), GetKey(NotFoundPrefix, nameof(MissingUriSetting.MissingUri)));
        }

        [Test]
        public void CreateMissingTimeSpanSettingTest()
        {
            AssertThrowsConfigurationErrorsExceptionMessageTest<MissingTimeSpanSetting>(
                Resources.MissingValueFormat, typeof(TimeSpan), GetKey(NotFoundPrefix, nameof(MissingTimeSpanSetting.MissingTimeSpan)));
        }

        [Test]
        public void CreateMissingIntSettingTest()
        {
            AssertThrowsConfigurationErrorsExceptionMessageTest<MissingIntSetting>(
                Resources.MissingValueFormat, typeof(int), GetKey(NotFoundPrefix, nameof(MissingIntSetting.MissingInt)));
        }

		[Test]
		public void CreateMissingFileInfoSettingTest()
		{
			AssertThrowsConfigurationErrorsExceptionMessageTest<MissingFileInfoSetting>(
				Resources.MissingValueFormat, typeof(FileInfo), GetKey(NotFoundPrefix, nameof(MissingFileInfoSetting.MissingFile)));
		}
		[Test]
		public void CreateMissingDirectoryInfoSettingTest()
		{
			AssertThrowsConfigurationErrorsExceptionMessageTest<MissingDirectoryInfoSetting>(
				Resources.MissingValueFormat, typeof(DirectoryInfo), GetKey(NotFoundPrefix, nameof(MissingDirectoryInfoSetting.MissingDir)));
		}

		[Test]
        public void CreateMissingArraySettingTest()
        {
            AssertThrowsConfigurationErrorsExceptionMessageTest<ArraySettings>(
                Resources.MissingValueFormat, typeof(string[]), GetKey(NotFoundPrefix, nameof(ArraySettings.MySimpleArrayProperty)));
        }

        [Test]
        public void CreateMissingListSettingTest()
        {
            AssertThrowsConfigurationErrorsExceptionMessageTest<ListSettings>(
                Resources.MissingValueFormat, typeof(IList<string>), GetKey(NotFoundPrefix, nameof(ListSettings.MySimpleListProperty)));
        }

        [Test]
        public void CreateMissingDictionarySettingTest()
        {
            AssertThrowsConfigurationErrorsExceptionMessageTest<DictionarySettings>(
                Resources.MissingValueFormat, typeof(Dictionary<string, string>), GetKey(NotFoundPrefix, nameof(DictionarySettings.MySimpleDictionaryProperty)));
        }

		[Test]
		public void CreateMissingNestedSettingTest()
		{
			AssertThrowsConfigurationErrorsExceptionMessageTest<NestedSettings>(
				Resources.MissingValueFormat, typeof(string), GetKey(NotFoundPrefix, nameof(NestedSettings.MyNestedProperty), nameof(Nested.Foo)));
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

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
			: base(TestSettingsLoader.GetSettingsLoader())
		{
		}

		[Test]
		public void CreateMissingStringTest()
		{
			AssertThrowsSettingsExceptionMessageTest<string>(
				Resources.MissingValueFormat, typeof(string), NotFoundPrefix);
		}

		[Test]
		public void CreateMissingNumericTest()
		{
			AssertThrowsSettingsExceptionMessageTest<int>(
				Resources.MissingValueFormat, typeof(int), NotFoundPrefix);
		}

		[Test]
		public void CreateMissingStringSettingTest()
		{
			AssertThrowsSettingsExceptionMessageTest<MissingStringSetting>(
				Resources.MissingValueFormat, typeof(String), Settings.SettingsLoader.GetSettingKey(NotFoundPrefix, nameof(MissingStringSetting.MissingString)));
		}

		[Test]
        public void CreateMissingDateTimeSettingTest()
        {
            AssertThrowsSettingsExceptionMessageTest<MissingDateTimeSetting>(
                Resources.MissingValueFormat, typeof(DateTime), Settings.SettingsLoader.GetSettingKey(NotFoundPrefix, nameof(MissingDateTimeSetting.MissingDateTime)));
        }

        [Test]
        public void CreateMissingUriSettingTest()
        {
            AssertThrowsSettingsExceptionMessageTest<MissingUriSetting>(
                Resources.MissingValueFormat, typeof(Uri), Settings.SettingsLoader.GetSettingKey(NotFoundPrefix, nameof(MissingUriSetting.MissingUri)));
        }

        [Test]
        public void CreateMissingTimeSpanSettingTest()
        {
            AssertThrowsSettingsExceptionMessageTest<MissingTimeSpanSetting>(
                Resources.MissingValueFormat, typeof(TimeSpan), Settings.SettingsLoader.GetSettingKey(NotFoundPrefix, nameof(MissingTimeSpanSetting.MissingTimeSpan)));
        }

        [Test]
        public void CreateMissingIntSettingTest()
        {
            AssertThrowsSettingsExceptionMessageTest<MissingIntSetting>(
                Resources.MissingValueFormat, typeof(int), Settings.SettingsLoader.GetSettingKey(NotFoundPrefix, nameof(MissingIntSetting.MissingInt)));
        }

		[Test]
		public void CreateMissingFileInfoSettingTest()
		{
			AssertThrowsSettingsExceptionMessageTest<MissingFileInfoSetting>(
				Resources.MissingValueFormat, typeof(FileInfo), Settings.SettingsLoader.GetSettingKey(NotFoundPrefix, nameof(MissingFileInfoSetting.MissingFile)));
		}
		[Test]
		public void CreateMissingDirectoryInfoSettingTest()
		{
			AssertThrowsSettingsExceptionMessageTest<MissingDirectoryInfoSetting>(
				Resources.MissingValueFormat, typeof(DirectoryInfo), Settings.SettingsLoader.GetSettingKey(NotFoundPrefix, nameof(MissingDirectoryInfoSetting.MissingDir)));
		}

		[Test]
        public void CreateMissingArraySettingTest()
        {
            AssertThrowsSettingsExceptionMessageTest<ArraySettings>(
                Resources.MissingValueFormat, typeof(string[]), Settings.SettingsLoader.GetSettingKey(NotFoundPrefix, nameof(ArraySettings.MySimpleArrayProperty)));
        }

        [Test]
        public void CreateMissingListSettingTest()
        {
            AssertThrowsSettingsExceptionMessageTest<ListSettings>(
                Resources.MissingValueFormat, typeof(IList<string>), Settings.SettingsLoader.GetSettingKey(NotFoundPrefix, nameof(ListSettings.MySimpleListProperty)));
        }

        [Test]
        public void CreateMissingDictionarySettingTest()
        {
            AssertThrowsSettingsExceptionMessageTest<DictionarySettings>(
                Resources.MissingValueFormat, typeof(Dictionary<string, string>), Settings.SettingsLoader.GetSettingKey(NotFoundPrefix, nameof(DictionarySettings.MySimpleDictionaryProperty)));
        }

		[Test]
		public void CreateMissingNestedSettingTest()
		{
			AssertThrowsSettingsExceptionMessageTest<NestedSettings>(
				Resources.MissingValueFormat, typeof(string), Settings.SettingsLoader.GetSettingKey(NotFoundPrefix, nameof(NestedSettings.MyNestedProperty), nameof(Nested.Foo)));
		}

		[Test]
        public void BadTypeConverterSettingTest()
        {
            AssertThrowsSettingsExceptionMessageTest<BadTypeConverterSetting>(
                Resources.CreateTypeConverterErrorFormat, typeof(string));
        }

        [Test]
        public void TypeConversionFailureSettingTest()
        {
            AssertThrowsSettingsExceptionMessageTest<TypeConversionFailureSetting>(
                Resources.ConvertValueErrorFormat, "XYZ", typeof(int));
        }

        [Test]
        public void ExplicitTypeConversionFailureSettingTest()
        {
            AssertThrowsSettingsExceptionMessageTest<ExplicitTypeConversionFailureSetting>(
                Resources.ExplicitTypeConverterErrorFormat, "http://XYZ",typeof (int));
        }
    }
}

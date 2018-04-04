using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Miracle.Settings.Properties;
using NUnit.Framework;

namespace Miracle.Settings.Tests
{
	[TestFixture]
	public class FileInfoLoadTests : LoadTestBase
	{
		private string _basePath;

		public FileInfoLoadTests()
			: base(new SettingsLoader())
		{
		}

		[SetUp]
		public void Setup()
		{
			_basePath = GetProjectDirectory();
		}

		[Test]
		public void BasicFileInfoTest()
		{
			// Important for test that current directory is the test project folder
			Directory.SetCurrentDirectory(_basePath);

			var settings = SettingsLoader.Create<FileSettings>();

			// Defaults
			Assert.That(settings.SimpleFile.FullName, Is.EqualTo(Path.Combine(_basePath, "App.config")));
			Assert.That(settings.FullFileInfo.FullName, Is.EqualTo(@"c:\Windows\regedit.exe"));
			Assert.That(settings.PartialFile.FullName, Is.EqualTo(Path.GetFullPath(Path.Combine(_basePath, @"..\Miracle.Settings.sln"))));
			Assert.That(settings.RelativeFileInfo.FullName, Is.EqualTo(Path.Combine(_basePath, "TestFolder", "TextFile1.txt")));
		}

		[Test]
		public void FileInfoArrayTest()
		{
			// Important for test that current directory is the test project folder
			Directory.SetCurrentDirectory(_basePath);

			var settings = SettingsLoader.CreateArray<FileInfo>("FileArray");

			// Defaults
			Assert.That(settings.Length, Is.EqualTo(2));
			Assert.That(settings[0].FullName, Is.EqualTo(Path.GetFullPath(Path.Combine(_basePath, @"..\Miracle.Settings.sln"))));
			Assert.That(settings[1].FullName, Is.EqualTo(@"c:\Windows\regedit.exe"));
		}

		[Test]
		public void FileAnnotationTest()
		{
            // Setup mock value provider with type
            var settingsLoader = DictionaryValueProvider.CreateSettingsLoader(new Dictionary<string, string>
			{
				{"Drive", "C:\\"},
				{"Folder", "Windows"},
				{"FileName", "notepad.exe"},
			});
			var settings = settingsLoader.Create<FileAnnotationSettings>();

			// Defaults
			Assert.That(settings.FullName, Is.Not.Null);
			Assert.That(settings.FullName.FullName, Is.EqualTo(@"C:\Windows\notepad.exe"));
		}

		[Test]
		public void FailTest1()
		{
			AssertThrowsSettingsExceptionMessageTest<FileSettings>(
				Resources.MissingValueFormat, typeof(FileInfo), GetKey(NotFoundPrefix, nameof(FileSettings.SimpleFile)));
		}

        [Test]
        public void FailTest2()
        {
            AssertThrowsSettingsExceptionMessageTest<FileInfo>(
                Resources.MissingValueFormat, typeof(FileInfo), GetKey(NotFoundPrefix));
        }

        [Test]
        public void FailTest3()
        {
            Assert.That(() => new SettingsLoader().Create<FileInfo>(nameof(SimpleSettings.Uri)), 
                Throws.Exception.TypeOf<SettingsException>()
                .With.Message.EqualTo(string.Format(Resources.CreateErrorFormat, typeof(FileInfo), nameof(SimpleSettings.Uri))));
        }
    }
}

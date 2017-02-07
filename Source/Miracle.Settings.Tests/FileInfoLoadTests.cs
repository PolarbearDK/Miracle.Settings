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
			_basePath = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory));
		}

		[Test]
		public void BasicFileInfoTest()
		{
			// Important for test that current directory is the test project folder
			Directory.SetCurrentDirectory(_basePath);

			var settings = SettingsLoader.Create<FileSettings>();

			// Defaults
			Assert.That(settings.SimpleFile.FullName, Is.EqualTo(Path.Combine(_basePath, "packages.config")));
			Assert.That(settings.FullFileInfo.FullName, Is.EqualTo("c:\\Windows\\regedit.exe"));
			Assert.That(settings.PartialFile.FullName, Is.EqualTo(Path.GetFullPath(Path.Combine(_basePath, "..\\Miracle.Settings.sln"))));
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
			Assert.That(settings[0].FullName, Is.EqualTo(Path.GetFullPath(Path.Combine(_basePath, "..\\Miracle.Settings.sln"))));
			Assert.That(settings[1].FullName, Is.EqualTo("c:\\Windows\\regedit.exe"));
		}

		[Test]
		public void FailTest1()
		{
			AssertThrowsConfigurationErrorsExceptionMessageTest<FileSettings>(
				Resources.MissingValueFormat, typeof(FileInfo), GetKey(NotFoundPrefix, nameof(FileSettings.SimpleFile)));
		}

		[Test]
		public void FailTest2()
		{
			AssertThrowsConfigurationErrorsExceptionMessageTest<FileInfo>(
				Resources.MissingValueFormat, typeof(FileInfo), GetKey(NotFoundPrefix));
		}

	}
}

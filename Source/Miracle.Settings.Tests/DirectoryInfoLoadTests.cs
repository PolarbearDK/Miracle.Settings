using System.IO;
using Miracle.Settings.Properties;
using NUnit.Framework;

namespace Miracle.Settings.Tests
{
	[TestFixture]
	public class DirectoryInfoLoadTests : LoadTestBase
	{
		private string _basePath;

		public DirectoryInfoLoadTests()
			: base(new SettingsLoader())
		{
		}

		[SetUp]
		public void Setup()
		{
			_basePath = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory));
		}

		[Test]
		public void BasicTest()
		{
			// Important for test that current directory is the test project folder
			Directory.SetCurrentDirectory(_basePath);

			var settings = SettingsLoader.Create<DirectorySettings>();

			// Defaults
			Assert.That(settings.SimpleDirectory.FullName, Is.EqualTo(Path.Combine(_basePath, "TestFolder")));
			Assert.That(settings.PartialDirectory.FullName, Is.EqualTo(Path.GetFullPath(Path.Combine(_basePath, "TestFolder\\TestSubFolder"))));
			Assert.That(settings.FullDirectoryInfo.FullName, Is.EqualTo("c:\\Windows"));
			Assert.That(settings.RelativeDirectoryInfo.FullName, Is.EqualTo(Path.Combine(_basePath, "TestFolder", "TestSubFolder")));
		}

		[Test]
		public void ArrayTest()
		{
			var settingsLoader = new SettingsLoader();

			// Important for test that current directory is the test project folder
			Directory.SetCurrentDirectory(_basePath);

			var settings = SettingsLoader.CreateArray<DirectoryInfo>("DirectoryArray");

			// Defaults
			Assert.That(settings.Length, Is.EqualTo(3));
			Assert.That(settings[0].FullName, Is.EqualTo(Path.Combine(_basePath, "TestFolder")));
			Assert.That(settings[1].FullName, Is.EqualTo(Path.GetFullPath(Path.Combine(_basePath, ".."))));
			Assert.That(settings[2].FullName, Is.EqualTo("c:\\Windows"));
		}

		[Test]
		public void FailTest1()
		{
			AssertThrowsConfigurationErrorsExceptionMessageTest<DirectorySettings>(
				string.Format(Resources.MissingValueFormat, typeof(DirectoryInfo), GetKey(NotFoundPrefix, nameof(DirectorySettings.SimpleDirectory)))
				);
		}

		[Test]
		public void FailTest2()
		{
			AssertThrowsConfigurationErrorsExceptionMessageTest<DirectoryInfo>(
				Resources.MissingValueFormat, typeof(DirectoryInfo), GetKey(NotFoundPrefix));
		}
	}
}

using System.IO;
using Miracle.Settings.Properties;
using NUnit.Framework;

#if NETCORE
using Microsoft.Extensions.Configuration;
#endif

namespace Miracle.Settings.Tests
{
	static class TestSettingsLoader
	{
		public static SettingsLoader GetSettingsLoader()
		{
#if NETCORE
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json");

			var configuration = builder.Build();

			return new SettingsLoader(new ConfigurationValueProvider(configuration));
#else
			return new SettingsLoader();
#endif

		}
	}

	[TestFixture]
	public class DirectoryInfoLoadTests : LoadTestBase
	{
		public DirectoryInfoLoadTests()
			: base(TestSettingsLoader.GetSettingsLoader())
		{
		}

		[SetUp]
		public void SetUp()
		{
			// Important for some tests that current directory is the test project folder
			Directory.SetCurrentDirectory(TestProjectFolder);
		}

		[Test]
		public void BasicTest()
		{
			// Important for test that current directory is the test project folder
			Directory.SetCurrentDirectory(TestProjectFolder);

			var settings = SettingsLoader.Create<DirectorySettings>();

			// Defaults
			Assert.That(settings.SimpleDirectory.FullName, Is.EqualTo(Path.Combine(TestProjectFolder, "TestFolder")));
			Assert.That(settings.PartialDirectory.FullName, Is.EqualTo(Path.GetFullPath(Path.Combine(TestProjectFolder, "TestFolder\\TestSubFolder"))));
			Assert.That(settings.FullDirectoryInfo.FullName, Is.EqualTo("c:\\Windows"));
			Assert.That(settings.RelativeDirectoryInfo.FullName, Is.EqualTo(Path.Combine(TestProjectFolder, "TestFolder", "TestSubFolder")));
		}

		[Test]
		public void ArrayTest()
		{
			var settingsLoader = TestSettingsLoader.GetSettingsLoader();

			// Important for test that current directory is the test project folder
			Directory.SetCurrentDirectory(TestProjectFolder);

			var settings = settingsLoader.CreateArray<DirectoryInfo>("DirectoryArray");

			// Defaults
			Assert.That(settings.Length, Is.EqualTo(3));
			Assert.That(settings[0].FullName, Is.EqualTo(Path.Combine(TestProjectFolder, "TestFolder")));
			Assert.That(settings[1].FullName, Is.EqualTo(Path.GetFullPath(Path.Combine(TestProjectFolder, ".."))));
			Assert.That(settings[2].FullName, Is.EqualTo("c:\\Windows"));
		}

		[Test]
		public void FailTest1()
		{
			AssertThrowsSettingsExceptionMessageTest<DirectorySettings>(
				string.Format(Resources.MissingValueFormat, typeof(DirectoryInfo), GetKey(NotFoundPrefix, nameof(DirectorySettings.SimpleDirectory)))
				);
		}

		[Test]
		public void FailTest2()
		{
			AssertThrowsSettingsExceptionMessageTest<DirectoryInfo>(
				Resources.MissingValueFormat, typeof(DirectoryInfo), GetKey(NotFoundPrefix));
		}
	}
}

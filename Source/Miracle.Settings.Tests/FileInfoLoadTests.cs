using System.IO;
using NUnit.Framework;

namespace Miracle.Settings.Tests
{
    [TestFixture]
    public class FileInfoLoadTests
    {
        private string _basePath;

		[SetUp]
		public void Setup()
		{
			_basePath = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory));
		}

        [Test]
        public void LoadTest()
        {
            var settingsLoader = new SettingsLoader();

            // Important for test that current directory is the test project folder
            Directory.SetCurrentDirectory(_basePath);

            var settings = settingsLoader.Create<FileSettings>();

            // Defaults
            Assert.That(settings.SimpleFile.FullName, Is.EqualTo(Path.Combine(_basePath, "packages.config")));
            Assert.That(settings.FullFileInfo.FullName, Is.EqualTo("c:\\Windows\\regedit.exe"));
            Assert.That(settings.PartialFile.FullName, Is.EqualTo(Path.GetFullPath(Path.Combine(_basePath, "..\\Miracle.Settings.sln"))));
            Assert.That(settings.RelativeFileInfo.FullName, Is.EqualTo(Path.Combine(_basePath, "DatabaseTest", "setup_database.sql")));
        }
    }
}

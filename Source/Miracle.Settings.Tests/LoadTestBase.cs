using System;
using System.IO;
using System.Xml;
using NUnit.Framework;

namespace Miracle.Settings.Tests
{
    public abstract class LoadTestBase: LoadFailTestBase
    {
        protected readonly ISettingsLoader SettingsLoader;
	    public string TestProjectFolder { get; private set; }

		protected LoadTestBase(ISettingsLoader settingsLoader)
		{
			SettingsLoader = settingsLoader;
			TestProjectFolder = GetProjectDirectory();
			Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
		}

		private string GetProjectDirectory()
	    {
		    var directory = TestContext.CurrentContext.TestDirectory;
		    string folder = null;
		    do
		    {
			    folder = Path.GetFileName(directory);
			    directory = Path.GetDirectoryName(directory);
		    } while (folder != null && !folder.Equals("bin", StringComparison.InvariantCultureIgnoreCase));

		    return directory;
	    }

		protected void AssertThrowsSettingsExceptionMessageTest<T>(string format, params object[] args)
		{
			AssertThrowsSettingsExceptionMessageTest(() => SettingsLoader.Create<T>(NotFoundPrefix), format, args);
		}
	}
}

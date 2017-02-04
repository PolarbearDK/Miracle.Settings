using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;
using Is = NUnit.DeepObjectCompare.Is;

namespace Miracle.Settings.Tests
{
    [TestFixture]
    public class PathLoadTests : LoadTestBase
    {
		string basePath;

		[SetUp]
		public void Setup()
		{
			basePath = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory));
		}

		[Test]
        public void LoadTest()
        {
			var settings = SettingsLoader.Create<PathSettings>();

			// Defaults
			Assert.That(settings, Is.DeepEqualTo(
				new
				{
					SimpleFile = new FileInfo(Path.Combine(basePath, ""))
				})
				.WithComparisonConfig(new ComparisonConfig() { IgnoreObjectTypes = true })
				);
		}
    }
}

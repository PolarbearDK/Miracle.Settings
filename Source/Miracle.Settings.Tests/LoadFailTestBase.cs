using System;
using System.Configuration;
using System.Linq;
using NUnit.Framework;

namespace Miracle.Settings.Tests
{
	[TestFixture]
	public class LoadFailTestBase
	{
		public const string NotFoundPrefix = "Missing";

		protected string GetKey(params string[] elements)
		{
			return string.Join(".", elements);
		}

		protected void AssertThrowsConfigurationErrorsExceptionMessageTest(Action action, string format, params object[] args)
		{
			var ex = Assert.Throws<ConfigurationErrorsException>(() => action());
			Console.WriteLine(ex);
			var expectedMessage = string.Format(format, args);
			Assert.That(ex.Message, Is.EqualTo(expectedMessage));
		}
	}
}

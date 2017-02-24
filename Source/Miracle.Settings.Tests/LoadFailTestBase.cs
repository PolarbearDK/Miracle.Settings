using System;
using System.Configuration;
using NUnit.Framework;

namespace Miracle.Settings.Tests
{
	public class LoadFailTestBase
	{
		public const string NotFoundPrefix = "Missing";

		protected string GetKey(params string[] elements)
		{
			return string.Join(".", elements);
		}

		protected void AssertThrowsConfigurationErrorsExceptionMessageTest(Action action, string format, params object[] args)
		{
            var expectedMessage = string.Format(format, args);
		    Assert.That(() => action(),
		        Throws.Exception.TypeOf<ConfigurationErrorsException>()
		            .With.Message.EqualTo(expectedMessage));
		}
	}
}

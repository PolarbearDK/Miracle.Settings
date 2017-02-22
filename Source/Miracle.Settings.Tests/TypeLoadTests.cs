using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Versioning;
using Miracle.Settings.Properties;
using NUnit.Framework;

namespace Miracle.Settings.Tests
{
	[TestFixture]
	public class TypeLoadTests
	{
		private static SettingsLoader MockSettingsValues(IDictionary<string, string> values)
		{
			var valueProvider = new MockValueProvider(values);
			var settingsLoader = new SettingsLoader();
			settingsLoader.ValueProviders.Clear();
			settingsLoader.ValueProviders.Add(valueProvider);
			return settingsLoader;
		}

		[Test]
		public void Test()
		{
			var key = "FooType";

			// Pick any type to use as test type
			var someType = typeof(TestFixtureAttribute);

			// Setup mock value provider with type
			var settingsLoader = MockSettingsValues(new Dictionary<string, string>
			{
				{key, someType.AssemblyQualifiedName}
			});

			var type = settingsLoader.Create<Type>(key);

			Assert.That(type, Is.Not.Null);
			Assert.That(type, Is.EqualTo(someType));
		}

		[Test]
		public void NoValueTest1()
		{
			var key = "Foo.Type";

			// Pick any type to use as test type
			var someType = "Foo, Foo.Dll";

			// Setup mock value provider with type
			var settingsLoader = MockSettingsValues(new Dictionary<string, string>());

			Assert.Throws<ConfigurationErrorsException>(() =>
			{
				var type = settingsLoader.Create<Type>(key);
			}, string.Format(Resources.ConvertValueErrorFormat, someType, key));
		}


		[Test]
		public void NoValueTest2()
		{
			// Pick any type to use as test type
			var someType = "Foo, Foo.Dll";

			// Setup mock value provider with type
			var settingsLoader = MockSettingsValues(new Dictionary<string, string>());

			Assert.Throws<ConfigurationErrorsException>(() =>
			{
				var type = settingsLoader.Create<TypeSettings>();
			}, string.Format(Resources.ConvertValueErrorFormat, someType, nameof(TypeSettings.Foo)));
		}

		[Test]
		public void FailTest()
		{
			var key = "Foo.Type";

			// Pick any type to use as test type
			var someType = "Foo, Foo.Dll";

			// Setup mock value provider with type
			var valueProvider = new MockValueProvider(new Dictionary<string, string>
			{
				{key, someType}
			});
			var settingsLoader = new SettingsLoader();
			settingsLoader.ValueProviders.Clear();
			settingsLoader.ValueProviders.Add(valueProvider);

			Assert.Throws<ConfigurationErrorsException>(() =>
			{
				var type = settingsLoader.Create<Type>(key);
			}, string.Format(Resources.ConvertValueErrorFormat, someType, key) );
		}
	}
}

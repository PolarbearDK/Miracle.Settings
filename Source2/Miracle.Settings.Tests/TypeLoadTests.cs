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
		[Test]
		public void Test()
		{
			var key = "FooType";

			// Pick any type to use as test type
			var someType = typeof(TestFixtureAttribute);

			// Setup mock value provider with type
			var settingsLoader = new SettingsLoader(new DictionaryValueProvider(new Dictionary<string, string>
			{
				{key, someType.AssemblyQualifiedName}
			}));

			var type = settingsLoader.Create<Type>(key);

			Assert.That(type, Is.Not.Null);
			Assert.That(type, Is.EqualTo(someType));
		}

		[Test]
		public void NoValueTest1()
		{
			var key = "Foo";

			// Setup mock value provider with type
			var settingsLoader = new SettingsLoader(new DictionaryValueProvider(new Dictionary<string, string>()));

			Assert.That(
                () => settingsLoader.Create<Type>(key), 
                Throws.Exception.TypeOf<SettingsException>()
                .With.Message.EqualTo(string.Format(Resources.MissingValueFormat, typeof(Type).FullName, key)));
		}


		[Test]
		public void NoValueTest2()
		{
            var key = "Foo";

            // Pick any type to use as test type
            var someType = "";

			// Setup mock value provider with type
		    var settingsLoader = new SettingsLoader(new DictionaryValueProvider(new Dictionary<string, string>
		    {
		        {key, someType}
		    }));


		    Assert.That(
		        () => settingsLoader.Create<TypeSettings>(key),
		        Throws.Exception.TypeOf<SettingsException>()
		            .With.Message.EqualTo(string.Format(Resources.MissingValueFormat, typeof(Type).FullName, SettingsLoader.GetSettingKey(key, nameof(TypeSettings.MyType)))));
        }

		[Test]
		public void FailLoadTest()
		{
			var key = "Foo.Type";

			// Pick any type to use as test type
			var someType = "Foo, MyType.Dll";

			// Setup mock value provider with type
			var settingsLoader = new SettingsLoader(new DictionaryValueProvider(new Dictionary<string, string>
			{
				{key, someType}
			}));

            Assert.That( 
                () => settingsLoader.Create<Type>(key),
                Throws.Exception.TypeOf<SettingsException>()
                .With.Message.EqualTo(string.Format(Resources.ConvertValueErrorFormat, someType, typeof(Type).FullName)));
		}
    }
}

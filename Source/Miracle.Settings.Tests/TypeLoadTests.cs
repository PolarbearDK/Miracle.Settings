using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Runtime.Versioning;
using Miracle.Settings.Properties;
using NUnit.Framework;

namespace Miracle.Settings.Tests
{
    [TestFixture]
    public class AssemblyLoadTests
    {
        [Test]
        public void AssemblyLoadSuccess()
        {
            var key = "Assembly";

            // Pick any type to use as test assembly
            var someAssembly = typeof(TestFixtureAttribute).Assembly;

            // Setup mock value provider with type
            var settingsLoader = new SettingsLoader(new DictionaryValueProvider(new Dictionary<string, string>
            {
                {key, someAssembly.FullName}
            }));

            var type = settingsLoader.Create<Assembly>(key);

            Assert.That(type, Is.Not.Null);
            Assert.That(type, Is.EqualTo(someAssembly));
        }

        [Test]
        public void AssemblyLoadFailure()
        {
            var settingKey = "Assembly";

            // This does not exist
            var someAssembly = "Foo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a7c55c561934e089";

            // Setup mock value provider with type
            var settingsLoader = new SettingsLoader(new DictionaryValueProvider(new Dictionary<string, string>
            {
                {settingKey, someAssembly}
            }));

            var expectedMessage = string.Format(
                    Resources.ConvertValueErrorFormat,
                    someAssembly,
                    typeof(Assembly).FullName);

            Assert.That(() => { settingsLoader.Create<Assembly>(settingKey); },
                Throws
                    .Exception.TypeOf<SettingsException>()
                    .With.Message.EqualTo(expectedMessage)
            );
        }
	}

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
        public void BadUri()
        {
            var prefix = "FooPrefix";
            var propPrefix = SettingsLoader.GetSettingKey(prefix, "foo");
            var settingKey = SettingsLoader.GetSettingKey(propPrefix, nameof(BadConversion.BadUri));
            var value = "[]";

            var expectedMessage = string.Format(
                Resources.ConversionErrorSuffix,
                string.Format(
                    Resources.ConvertValueErrorFormat,
                    value,
                    typeof(Uri).FullName),
                settingKey);

            var settingsLoader = new SettingsLoader(new DictionaryValueProvider(new Dictionary<string, string>
            {
                {settingKey, "[]"},
            }));

            Assert.That(() => { settingsLoader.Create<BadConversion>(propPrefix); },
                Throws
                .Exception.TypeOf<SettingsException>()
                .With.Message.EqualTo(expectedMessage)
                );
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

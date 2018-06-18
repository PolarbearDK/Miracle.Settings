using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Miracle.Settings.Tests
{
    [TestFixture]
    public class InterfaceMappingTests
    {
	    [Test]
	    public void Test()
	    {
		    const string foo = "Testing";
		    const int bar = 100;


		    var prefix = "MyPrefix";
		    var propPrefix = SettingsLoader.GetSettingKey(prefix, nameof(InterfaceMapping.Prop));

		    var settingsLoader = new SettingsLoader(new DictionaryValueProvider(new Dictionary<string, string>
		    {
			    {SettingsLoader.GetSettingKey(propPrefix, nameof(IMyInterface.Foo)), foo},
			    {SettingsLoader.GetSettingKey(propPrefix, nameof(IMyInterface.Bar)), bar.ToString()},
		    }));

		    var setting = settingsLoader.Create<InterfaceMapping>(prefix);

		    Assert.That(setting, Is.Not.Null);
		    Assert.That(setting.Prop, Is.Not.Null);
		    Assert.That(setting.Prop.Foo, Is.EqualTo(foo));
		    Assert.That(setting.Prop.Bar, Is.EqualTo(bar));
	    }

	    [Test]
        public void BadTest()
        {
            const string foo = "Testing";
            const int bar = 100;


            var prefix = "MyPrefix";
            var propPrefix = SettingsLoader.GetSettingKey(prefix, nameof(InterfaceMapping.Prop));

	        var settingsLoader = new SettingsLoader(new DictionaryValueProvider(new Dictionary<string, string>
	        {
		        {SettingsLoader.GetSettingKey(propPrefix, nameof(IMyInterface.Foo)), foo},
		        {SettingsLoader.GetSettingKey(propPrefix, nameof(IMyInterface.Bar)), bar.ToString()},
	        }));

            Assert.That(() => { settingsLoader.Create<BadInterfaceMapping>(prefix);}, Throws.Exception.TypeOf<ArgumentException>());
        }
    }
}

using System.Collections.Generic;
using NUnit.Framework;
using Is = NUnit.DeepObjectCompare.Is;
// ReSharper disable AccessToStaticMemberViaDerivedType

namespace Miracle.Settings.Tests
{
    [TestFixture]
    public class IgnoreSettingTests
    {
        [Test]
        public void Test()
        {
            const string @string = "My String";
            var prefix = "Hello";

            var settingsLoader = new SettingsLoader(new DictionaryValueProvider(new Dictionary<string, string>
            {
                { SettingsLoader.GetSettingKey(prefix, nameof(IgnoreSettings.String)), @string},
            }));

            var setting = settingsLoader.Create<IgnoreSettings>(prefix);

            Assert.That(setting, Is.Not.Null);
            Assert.That(setting.String, Is.EqualTo(@string));
            Assert.That(setting.Ignored, Is.Null);
	        Assert.That(setting.NestedIgnored, Is.Null);
        }
	}
}

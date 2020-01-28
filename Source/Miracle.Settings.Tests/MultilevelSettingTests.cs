using System.Collections.Generic;
using NUnit.Framework;
using Is = NUnit.DeepObjectCompare.Is;

namespace Miracle.Settings.Tests
{
    [TestFixture]
    public class MultilevelSettingTests
    {
        [Test]
        public void Test()
        {
            const string prefix = "Whatever";
            const string foo = "My String foo";
            const string bar = "My String bar";
            const string baz = "My baz String";

            var settingsLoader = new SettingsLoader(new DictionaryValueProvider(new Dictionary<string, string>
            {
                {SettingsLoader.GetSettingKey(prefix, nameof(MultiLevel1.Foo)), foo},
                {SettingsLoader.GetSettingKey(prefix, nameof(MultiLevel1.Level2), nameof(MultiLevel2.Bar)), bar},
                {SettingsLoader.GetSettingKey(prefix, nameof(MultiLevel1.Level2), nameof(MultiLevel2.Level3), nameof(MultiLevel3.Baz)), baz},
            }));

            var setting = settingsLoader.Create<MultiLevel1>(prefix);

            var expected = new MultiLevel1()
            {
                Foo = foo,
                Level2 = new MultiLevel2()
                {
                    Bar = bar,
                    Level3 = new MultiLevel3()
                    {
                        Baz = baz
                    }
                }
            };
            Assert.That(setting, Is.Not.Null);
            Assert.That(setting, Is.DeepEqualTo(expected));
        }
    }
}

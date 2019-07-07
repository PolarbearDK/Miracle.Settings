using System.Collections.Generic;
using NUnit.Framework;
using Is = NUnit.DeepObjectCompare.Is;
// ReSharper disable AccessToStaticMemberViaDerivedType

namespace Miracle.Settings.Tests
{
    [TestFixture]
    public class OptionalSettingTests
    {
        [Test]
        public void Test()
        {
            const string @string = "My String foo";
            const string present = "World";
            var nested = new Nested {Foo = "Foo1", Bar = -15};
            var prefix = "Hello";
            var array = new[] {" Mickey", "Mouse "};

            var settingsLoader = new SettingsLoader(new DictionaryValueProvider(new Dictionary<string, string>
            {
                {SettingsLoader.GetSettingKey(prefix, nameof(OptionalSettings.String)), @string},
                {SettingsLoader.GetSettingKey(prefix, nameof(OptionalSettings.OptionalPresent)), present},
                {SettingsLoader.GetSettingKey(prefix, nameof(OptionalSettings.OptionalNestedPresent), nameof(Nested.Foo)), nested.Foo},
                {SettingsLoader.GetSettingKey(prefix, nameof(OptionalSettings.OptionalNestedPresent), nameof(Nested.Bar)), nested.Bar.ToString()},
                {SettingsLoader.GetSettingKey(prefix, nameof(OptionalSettings.OptionalArrayPresent), "1"), array[0]},
                {SettingsLoader.GetSettingKey(prefix, nameof(OptionalSettings.OptionalArrayPresent), "2"), array[1]}
            }));

            var setting = settingsLoader.Create<OptionalSettings>(prefix);

            Assert.That(setting, Is.Not.Null);
            Assert.That(setting.String, Is.EqualTo(@string));
            Assert.That(setting.OptionalMissing, Is.Null);
            Assert.That(setting.OptionalPresent, Is.EqualTo(present));
            Assert.That(setting.OptionalNestedMissing, Is.Null);
            Assert.That(setting.OptionalNestedPresent, Is.DeepEqualTo(nested));
            Assert.That(setting.OptionalArrayMissing, Is.Null);
            Assert.That(setting.OptionalArrayPresent, Is.DeepEqualTo(array));
        }
    }

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

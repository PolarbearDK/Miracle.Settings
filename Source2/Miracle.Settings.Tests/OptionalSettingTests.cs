using System.Collections.Generic;
using NUnit.Framework;
using Is = NUnit.DeepObjectCompare.Is;

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
            var nested = new Nested() {Foo= "Foo1", Bar = -15};
            var prefix = "Hello";
            var array = new[] {" Mickey", "Mouse "};

            var settingsLoader = DictionaryValueProvider.CreateSettingsLoader(new Dictionary<string, string>
            {
                { $"{prefix}.{nameof(OptionalSettings.String)}", @string},
                { $"{prefix}.{nameof(OptionalSettings.OptionalPresent)}", present},
                { $"{prefix}.{nameof(OptionalSettings.OptionalNestedPresent)}.{nameof(Nested.Foo)}", nested.Foo},
                { $"{prefix}.{nameof(OptionalSettings.OptionalNestedPresent)}.{nameof(Nested.Bar)}", nested.Bar.ToString()},
                { $"{prefix}.{nameof(OptionalSettings.OptionalArrayPresent)}.1", array[0]},
                { $"{prefix}.{nameof(OptionalSettings.OptionalArrayPresent)}.2", array[1]},
            });

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
}
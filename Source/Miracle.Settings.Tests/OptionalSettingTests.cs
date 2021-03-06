﻿using System;
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
            var timespan = TimeSpan.FromMilliseconds(9514593762);
            var number = 1234567;
            var animal = AnimalType.Rabbit;

            var settingsLoader = new SettingsLoader(new DictionaryValueProvider(new Dictionary<string, string>
            {
                {SettingsLoader.GetSettingKey(prefix, nameof(OptionalSettings.String)), @string},
                {SettingsLoader.GetSettingKey(prefix, nameof(OptionalSettings.OptionalPresent)), present},
                {SettingsLoader.GetSettingKey(prefix, nameof(OptionalSettings.OptionalNestedPresent), nameof(Nested.Foo)), nested.Foo},
                {SettingsLoader.GetSettingKey(prefix, nameof(OptionalSettings.OptionalNestedPresent), nameof(Nested.Bar)), nested.Bar.ToString()},
                {SettingsLoader.GetSettingKey(prefix, nameof(OptionalSettings.OptionalArrayPresent), "1"), array[0]},
                {SettingsLoader.GetSettingKey(prefix, nameof(OptionalSettings.OptionalArrayPresent), "2"), array[1]},
                {SettingsLoader.GetSettingKey(prefix, nameof(OptionalSettings.OptionalTimespanPresent)), timespan.ToString()},
                {SettingsLoader.GetSettingKey(prefix, nameof(OptionalSettings.OptionalIntPresent)), number.ToString()},
                {SettingsLoader.GetSettingKey(prefix, nameof(OptionalSettings.OptionalEnumPresent)), animal.ToString()},
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
            Assert.That(setting.OptionalTimespanMissing, Is.Null);
            Assert.That(setting.OptionalTimespanPresent, Is.EqualTo(timespan));
            Assert.That(setting.OptionalIntMissing, Is.Null);
            Assert.That(setting.OptionalIntPresent, Is.EqualTo(number));
            Assert.That(setting.OptionalEnumMissing, Is.Null);
            Assert.That(setting.OptionalEnumPresent, Is.EqualTo(animal));
        }
    }
}

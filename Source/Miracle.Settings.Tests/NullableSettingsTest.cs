using System;
using System.Collections.Generic;
using NUnit.Framework;
using Is = NUnit.DeepObjectCompare.Is;

namespace Miracle.Settings.Tests
{
    [TestFixture]
    public class NullableSettingsTest
    {
        [Test]
        public void Test()
        {
            var timespan = TimeSpan.FromMilliseconds(9514593762);
            var number = 1234567;
            var animal = AnimalType.Rabbit;

            var settingsLoader = new SettingsLoader(new DictionaryValueProvider(new Dictionary<string, string>
            {
                {nameof(NullableSettings.NullableTimespan), timespan.ToString()},
                {nameof(NullableSettings.NullableInt), number.ToString()},
                {nameof(NullableSettings.NullableAnimal), animal.ToString()},
            }));

            var setting = settingsLoader.Create<NullableSettings>();

            Assert.That(setting, Is.Not.Null);
            Assert.That(setting.NullableTimespan, Is.EqualTo(timespan));
            Assert.That(setting.NullableInt, Is.EqualTo(number));
            Assert.That(setting.NullableAnimal, Is.EqualTo(animal));
        }
    }

    [TestFixture]
    public class NullableSettingsTest2 : LoadTestBase
    {
        public NullableSettingsTest2()
            : base(TestSettingsLoader
                .GetSettingsLoader()
                .AddProvider(new EnvironmentValueProvider()))
        {
        }

        [Test]
        public void NullableLoadTest()
        {

            // NOTE! This will fail in .NET Core due to bug in Json Configuration loader
            // Only reason this will work is because properties is annotated with [Setting(IgnoreValue = "")] 
            var settings = SettingsLoader.Create<NullableSettings>();

            Assert.That(settings.NullableTimespan, Is.Null);
            Assert.That(settings.NullableInt, Is.Null);
            Assert.That(settings.NullableAnimal, Is.Null);
        }
    }
}
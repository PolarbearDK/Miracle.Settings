using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Runtime.Versioning;
using Miracle.Settings.Properties;
using NUnit.Framework;

namespace Miracle.Settings.Tests
{
    [TestFixture]
    public class IPAddressTests
    {
        [Test]
        public void Test1()
        {
            var key = "FooIp";
            var ip = "42.41.40.39";

            // Setup mock value provider
            var settingsLoader = DictionaryValueProvider.CreateSettingsLoader(new Dictionary<string, string>
            {
                {key, ip}
            });

            var ipAddress = settingsLoader.Create<IPAddress>(key);

            Assert.That(ipAddress, Is.Not.Null);
            Assert.That(ipAddress, Is.EqualTo(IPAddress.Parse(ip)));
        }

        [Test]
        public void Test2()
        {
            var key = "FooIp";
            var ips = new[] { "42.41.40.39", "38.37.36.35" };
            var ip = string.Join(",", ips);

            // Setup mock value provider
            var settingsLoader = DictionaryValueProvider.CreateSettingsLoader(new Dictionary<string, string>
            {
                {key, ip}
            });

            var ipAddresses = settingsLoader.CreateArray<IPAddress>(key, new[] { ',' });

            Assert.That(ipAddresses, Is.Not.Null);
            Assert.That(ipAddresses, Has.Length.EqualTo(2));
            Assert.That(ipAddresses, Is.EqualTo(ips.Select(IPAddress.Parse).ToArray()));
        }

        [Test]
        public void NoValueTest1()
        {
            var key = "Foo";

            // Setup mock value provider 
            var settingsLoader = DictionaryValueProvider.CreateSettingsLoader(new Dictionary<string, string>());

            Assert.That(
                () => settingsLoader.Create<IPAddress>(key),
                Throws.Exception.TypeOf<ConfigurationErrorsException>()
                .With.Message.EqualTo(string.Format(Resources.MissingValueFormat, typeof(IPAddress).FullName, key)));
        }
    }
}

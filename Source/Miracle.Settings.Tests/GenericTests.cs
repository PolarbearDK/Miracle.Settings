using System.Collections.Generic;
using NUnit.Framework;
using Is = NUnit.DeepObjectCompare.Is;

namespace Miracle.Settings.Tests
{
    [TestFixture]
    public class GenericTests
    {
        [Test]
        public void IsGenericTypeDefinitionAssignableFromTest()
        {
            Assert.That(typeof(List<>).IsGenericTypeDefinitionAssignableFrom(typeof(List<>)), Is.True);
            Assert.That(typeof(IList<>).IsGenericTypeDefinitionAssignableFrom(typeof(List<>)), Is.True);
            Assert.That(typeof(IEnumerable<>).IsGenericTypeDefinitionAssignableFrom(typeof(List<>)), Is.True);

            Assert.That(typeof(IList<>).IsGenericTypeDefinitionAssignableFrom(typeof(Dictionary<,>)), Is.False);
            Assert.That(typeof(List<>).IsGenericTypeDefinitionAssignableFrom(typeof(IList<>)), Is.False);
        }
    }
}
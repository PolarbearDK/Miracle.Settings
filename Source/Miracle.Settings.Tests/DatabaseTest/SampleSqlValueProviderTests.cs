using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Versioning;
using Miracle.Settings.Properties;
using NUnit.Framework;
using Is = NUnit.DeepObjectCompare.Is;

namespace Miracle.Settings.Tests.DatabaseTest
{

    [TestFixture]
    [Category("Integration")]
    class SampleSqlValueProviderTests: LoadTestBase, IDisposable
	{
        /*
        * Requires SQL server to test: Run script setup_database.sql
        */

        private readonly IDbConnection _connection;

		public void Dispose()
		{
			_connection?.Dispose();
		}

		public SampleSqlValueProviderTests(IDbConnection connection)
			: base(new SettingsLoader()
				.ClearProviders()
				.AddProvider(new SampleSqlValueProvider(connection)))
		{
			_connection = connection;
		}

		public SampleSqlValueProviderTests()
			:this(new SqlConnection("Data Source=.;Database=SettingsUnitTest;Trusted_Connection=Yes"))
		{
		}

		[SetUp]
        public void SetUp()
        {
			_connection.Open();
		}

		[TearDown]
        public void TearDown()
        {
            _connection.Close();
        }

        [Test]
        public void GetValueTest()
        {
            var result = SettingsLoader.Create<string>("Foo");

            Assert.That(result, Is.EqualTo("Foo from database"));
        }

        [Test]
        public void GetKeysTest()
        {
            var dictionary = SettingsLoader.CreateDictionary<string, string>();

            Assert.That(dictionary, Is.DeepEqualTo(
                new Dictionary<string, string>
                {
                    {"Bar", "14"},
                    {"Baz", "hello world"},
                    {"Foo", "Foo from database"},
                }));
        }

        [Test]
        public void GetNestedFromDatabase()
        {
            var dictionary = SettingsLoader.Create<Nested>();

            Assert.That(dictionary, Is.DeepEqualTo(
                new Nested
                {
                    Bar = 14,
                    Foo = "Foo from database"
                }));
        }

		[Test]
		public void CreateMissingStringTest()
		{
			AssertThrowsConfigurationErrorsExceptionMessageTest<string>(
				Resources.MissingValueFormat, typeof(string), GetKey(NotFoundPrefix));
		}

		[Test]
		public void CreateMissingNumericTest()
		{
			AssertThrowsConfigurationErrorsExceptionMessageTest<int>(
				Resources.MissingValueFormat, typeof(int), GetKey(NotFoundPrefix));
		}

        [Test]
        public void GetNestedSettingsNotFoundTest()
        {
			AssertThrowsConfigurationErrorsExceptionMessageTest<NestedSettings>(
				Resources.MissingValueFormat, typeof(string), GetKey(NotFoundPrefix, nameof(NestedSettings.MyNestedProperty), nameof(Nested.Foo)));
        }
	}
}

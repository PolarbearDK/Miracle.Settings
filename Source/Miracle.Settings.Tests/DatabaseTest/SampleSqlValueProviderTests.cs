using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using NUnit.Framework;
using Is = NUnit.DeepObjectCompare.Is;

namespace Miracle.Settings.Tests.DatabaseTest
{

    [TestFixture]
    [Category("Integration")]
    class SampleSqlValueProviderTests
    {
        /*
        * Requires SQL script:
        * 
        USE [Settings]
        GO

        DROP TABLE [dbo].[Setting]
        GO

        CREATE TABLE [dbo].[Setting]
        (
            [Key] NVARCHAR(50) NOT NULL PRIMARY KEY, 
            [Value] NVARCHAR(MAX) NULL
        )
        GO

        INSERT INTO  [dbo].[Setting] ([Key],[Value])
        VALUES 
        (N'Foo', N'Foo from database'),
        (N'Bar', N'14'),
        (N'Baz', N'hello world')

        */

        private IDbConnection _connection;
        private ISettingsLoader _settingLoader;

        [SetUp]
        public void SetUp()
        {
            _connection = new SqlConnection("Data Source=.;Database=Settings;Trusted_Connection=Yes");
            _connection.Open();
            _settingLoader = new SettingsLoader()
                .ClearProviders()
                .AddProvider(new SampleSqlValueProvider(_connection));
        }

        [TearDown]
        public void TearDown()
        {
            _connection.Close();
            _connection.Dispose();
        }

        [Test]
        public void GetValueTest()
        {
            var result = _settingLoader.Create<string>("Foo");

            Assert.That(result, Is.EqualTo("Foo from database"));
        }

        [Test]
        public void GetKeysTest()
        {
            var dictionary = _settingLoader.CreateDictionary<string, string>();

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
            var dictionary = _settingLoader.Create<Nested>();

            Assert.That(dictionary, Is.DeepEqualTo(
                new Nested
                {
                    Bar = 14,
                    Foo = "Foo from database"
                }));
        }

        [Test]
        public void GetValueNotFoundTest()
        {
            var result = _settingLoader.Create<string>("NotFound");
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetNestedSettingsNotFoundTest()
        {
            var ex = Assert.Throws<ConfigurationErrorsException>(() =>
            {
                var result = _settingLoader.Create<NestedSettings>("NotFound");
            });
            Assert.That(ex, Has.Message.EqualTo("A value has to be provided for Setting: NotFound.MyNestedProperty.Foo"));
        }

    }
}
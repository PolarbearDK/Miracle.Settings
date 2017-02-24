using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using Miracle.Settings.Properties;
using NUnit.Framework;

namespace Miracle.Settings.Tests
{
    // This is just an example taken from: https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/introduction/adding-validation
    // Never load a movie database from config!

    [TestFixture]
    public class ValidationTests
    {
        [Test]
        public void Test()
        {
            const string title = "The Shawshank Redemption";
            const string releaseDate = "1994-02-01";
            const string genre = "Drama";
            const decimal price = 50;
            const string rating = "R";

            var settingsLoader = DictionaryValueProvider.CreateSettingsLoader(new Dictionary<string, string>
            {
                {nameof(Movie.ID), "10"},
                {nameof(Movie.Title), title},
                {nameof(Movie.ReleaseDate), releaseDate},
                {nameof(Movie.Genre), genre},
                {nameof(Movie.Price), price.ToString(CultureInfo.InvariantCulture)},
                {nameof(Movie.Rating), rating},
            });

            var movie = settingsLoader.Create<Movie>();

            Assert.That(movie, Is.Not.Null);
            Assert.That(movie.Title, Is.EqualTo(title));
            Assert.That(movie.ReleaseDate, Is.EqualTo(DateTime.ParseExact(releaseDate,"yyyy-MM-dd",CultureInfo.InvariantCulture,DateTimeStyles.None)));
            Assert.That(movie.Genre, Is.EqualTo(genre));
            Assert.That(movie.Price, Is.EqualTo(price));
            Assert.That(movie.Rating, Is.EqualTo(rating));
        }

        [Test]
        public void ValidationErrorTest()
        {
            const int id = 10;
            const string title = "The Shawshank Redemption 1234567890123456789012345678901234567890123456789012345678901234567890";
            const string releaseDate = "1994-02-01";
            const string genre = "Drama";
            const decimal price = 50;
            const string rating = "12345R";

            var settingsLoader = DictionaryValueProvider.CreateSettingsLoader(new Dictionary<string, string>
            {
                {nameof(Movie.ID), id.ToString()},
                {nameof(Movie.Title), title},
                {nameof(Movie.ReleaseDate), releaseDate},
                {nameof(Movie.Genre), genre},
                {nameof(Movie.Price), price.ToString(CultureInfo.InvariantCulture)},
                {nameof(Movie.Rating), rating},
            });

            var ex = Assert.Throws<ConfigurationErrorsException>(() =>
            {
                var movie = settingsLoader.Create<Movie>();
            });

            Console.WriteLine(ex.Message);
            
            var expectedMessage = string.Format(Resources.ValidationError, nameof(Movie.Title), "");

            Assert.That(ex.Message, Does.StartWith(expectedMessage));
        }
    }
}

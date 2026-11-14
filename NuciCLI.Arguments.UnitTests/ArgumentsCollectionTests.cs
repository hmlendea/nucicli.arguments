using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace NuciCLI.Arguments.UnitTests
{
    public class ArgumentsCollectionTests
    {
        [Test]
        public void GivenAMissingKey_WhenReadingTheIndexer_ThenNullIsReturned()
        {
            ArgumentsCollection collection = new();

            Assert.That(collection["missing"], Is.Null);
        }

        [Test]
        public void GivenAStoredValue_WhenReadingTheIndexer_ThenTheValueIsReturned()
        {
            ArgumentsCollection collection = new()
            {
                ["name"] = "John Doe"
            };

            Assert.That(collection["name"], Is.EqualTo("John Doe"));
        }

        [Test]
        public void GivenAStoredValue_WhenReplacingItThroughTheIndexer_ThenTheLatestValueIsReturned()
        {
            ArgumentsCollection collection = new()
            {
                ["name"] = "John Doe"
            };

            collection["name"] = "John Wick";

            Assert.That(collection["name"], Is.EqualTo("John Wick"));
        }

        [TestCase("name")]
        [TestCase("age")]
        public void GivenAStoredKey_WhenCheckingExistence_ThenTrueIsReturned(string key)
        {
            ArgumentsCollection collection = new()
            {
                [key] = "42"
            };

            Assert.That(collection.Has(key));
        }

        [Test]
        public void GivenAMissingKey_WhenCheckingExistence_ThenFalseIsReturned()
        {
            ArgumentsCollection collection = new();

            Assert.That(collection.Has("missing"), Is.False);
        }

        [Test]
        public void GivenANullValue_WhenCheckingExistence_ThenTrueIsReturned()
        {
            ArgumentsCollection collection = new()
            {
                ["name"] = null
            };

            Assert.That(collection.Has("name"));
        }

        [Test]
        public void GivenAStoredString_WhenRetrievingItWithAType_ThenItIsCast()
        {
            ArgumentsCollection collection = new()
            {
                ["name"] = "John Doe"
            };

            Assert.That(collection.Get<string>("name"), Is.EqualTo("John Doe"));
        }

        [Test]
        public void GivenAMissingKey_WhenRetrievingItWithAType_ThenAKeyNotFoundExceptionIsThrown()
        {
            ArgumentsCollection collection = new();

            Assert.Throws<KeyNotFoundException>(() => collection.Get<string>("missing"));
        }

        [Test]
        public void GivenAnIncompatibleValue_WhenRetrievingItWithAType_ThenAnInvalidCastExceptionIsThrown()
        {
            ArgumentsCollection collection = new()
            {
                ["name"] = "John Doe"
            };

            Assert.Throws<InvalidCastException>(() => collection.Get<int>("name"));
        }

        [Test]
        public void GivenAStoredValue_WhenEnumeratingTheCollection_ThenTheEntryIsYielded()
        {
            ArgumentsCollection collection = new()
            {
                ["name"] = "John Doe"
            };

            Assert.That(collection, Is.EquivalentTo(new[] { new KeyValuePair<string, object?>("name", "John Doe") }));
        }

        [Test]
        public void GivenAnEmptyCollection_WhenEnumeratingIt_ThenNoEntriesAreYielded()
            => Assert.That(new ArgumentsCollection(), Is.Empty);
    }
}
using System;
using NUnit;
using NUnit.Framework;

namespace NuciCLI.Arguments.UnitTests
{
    public class ArgumentParserTests
    {
        ArgumentParser parser;

        [SetUp]
        public void Setup()
            => parser = new();

        [Test]
        public void GivenKnownArguments_WhenParsingTheArguments_ThenAllArgumentsHaveTheExpectedValued()
        {
            parser.AddArgument("name", "The name of the user");
            parser.AddArgument("age", "The age of the user");

            ArgumentsCollection args = parser.ParseArgs(["--name", "John", "--age", "30"]);

            Assert.That(args["name"], Is.EqualTo("John"));
            Assert.That(args["age"], Is.EqualTo("30"));
        }

        [Test]
        public void GivenUnknownArguments_WhenParsingTheArguments_ThenAnArgumentExceptionIsThrown()
        {
            ArgumentParser parser = new();

            parser.AddArgument("known", "A known argument");

            ArgumentException ex = Assert.Throws<ArgumentException>(() =>
            {
                parser.ParseArgs(["--known", "Jon", "--unknown", "Snow"]);
            });

            Assert.That(ex.Message, Is.EqualTo("Unknown argument: unknown"));
        }
    }
}
using System;

using NUnit.Framework;

namespace NuciCLI.Arguments.UnitTests
{
    public class ArgumentParserTests
    {
        private static readonly string[] Tokens = ["--name", "John Doe", "--age", "32"];

        private ArgumentParser parser = new();

        [SetUp]
        public void SetUp()
            => parser = new();

        [Test]
        public void GivenKnownArguments_WhenParsingTheArguments_ThenAllArgumentsHaveTheExpectedValued()
        {
            parser.AddArgument("name", "The name of the user");
            parser.AddArgument("age", "The age of the user");

            ArgumentsCollection args = parser.ParseArgs(Tokens);

            Assert.That(args["name"], Is.EqualTo("John Doe"));
            Assert.That(args["age"], Is.EqualTo("32"));
        }

        [Test]
        public void GivenUnknownArguments_WhenParsingTheArguments_ThenAnArgumentExceptionIsThrown()
        {
            parser.AddArgument("known", "A known argument");

            ArgumentException ex = Assert.Throws<ArgumentException>(() =>
            {
                parser.ParseArgs(["--known", "John Doe", "--unknown", "Solaire of Astora"]);
            })!;

            Assert.That(ex.Message, Is.EqualTo("Unknown argument: unknown"));
        }

        [Test]
        public void GivenATokenWithoutThePrefix_WhenParsingTheArguments_ThenAnArgumentExceptionIsThrown()
        {
            parser.AddArgument("name", "The name of the user");

            ArgumentException ex = Assert.Throws<ArgumentException>(() =>
            {
                parser.ParseArgs(["name", "John Doe"]);
            })!;

            Assert.That(ex.Message, Is.EqualTo("Invalid argument format: name"));
        }

        [Test]
        public void GivenAKnownArgumentWithoutAValue_WhenParsingTheArguments_ThenAnArgumentNullExceptionIsThrown()
        {
            parser.AddArgument("name", "The name of the user");

            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() =>
            {
                parser.ParseArgs(["--name"]);
            })!;

            Assert.That(ex.ParamName, Is.EqualTo("name"));
        }

        [Test]
        public void GivenAMissingRequiredArgument_WhenParsingTheArguments_ThenAnArgumentNullExceptionIsThrown()
        {
            parser.AddArgument("name", "The name of the user");
            parser.AddArgument("nickname", "The nickname of the user", required: true);

            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() =>
            {
                parser.ParseArgs(["--name", "John Doe"]);
            })!;

            Assert.That(ex.ParamName, Is.EqualTo("nickname"));
        }

        [Test]
        public void GivenAnOmittedOptionalArgument_WhenParsingTheArguments_ThenTheDefaultValueIsApplied()
        {
            parser.AddArgument("name", "The name of the user");
            parser.AddArgument("age", "The age of the user", defaultValue: "42");

            ArgumentsCollection args = parser.ParseArgs(["--name", "John Doe"]);

            Assert.That(args["age"], Is.EqualTo("42"));
        }

        [Test]
        public void GivenAnOmittedOptionalArgumentWithoutADefault_WhenParsingTheArguments_ThenTheValueIsNull()
        {
            parser.AddArgument("name", "The name of the user");

            ArgumentsCollection args = parser.ParseArgs([]);

            Assert.That(args.Has("name"));
            Assert.That(args["name"], Is.Null);
        }

        [Test]
        public void GivenADuplicateRegistration_WhenParsingTheArguments_ThenTheLatestRegistrationIsUsed()
        {
            parser.AddArgument("name", "The name of the user", defaultValue: "John Doe");
            parser.AddArgument("name", "The updated name of the user", defaultValue: "John Wick");

            ArgumentsCollection args = parser.ParseArgs([]);

            Assert.That(args["name"], Is.EqualTo("John Wick"));
        }

        [Test]
        public void GivenADifferentlyCasedArgumentName_WhenParsingTheArguments_ThenAnArgumentExceptionIsThrown()
        {
            parser.AddArgument("Name", "The name of the user");

            Assert.Throws<ArgumentException>(() =>
            {
                parser.ParseArgs(["--name", "John Doe"]);
            });
        }

        [Test]
        public void GivenRepeatedParsing_WhenInvoked_ThenEachCallReturnsANewCollection()
        {
            parser.AddArgument("name", "The name of the user");

            ArgumentsCollection firstParse = parser.ParseArgs(["--name", "John Doe"]);
            ArgumentsCollection secondParse = parser.ParseArgs(["--name", "Solaire of Astora"]);

            Assert.That(firstParse, Is.Not.SameAs(secondParse));
            Assert.That(firstParse["name"], Is.EqualTo("John Doe"));
            Assert.That(secondParse["name"], Is.EqualTo("Solaire of Astora"));
        }
    }
}
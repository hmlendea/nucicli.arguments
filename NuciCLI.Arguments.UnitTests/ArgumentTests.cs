using NUnit.Framework;

namespace NuciCLI.Arguments.UnitTests
{
    public class ArgumentTests
    {
        [Test]
        public void GivenAllValues_WhenConstructingAnArgument_ThenTheyAreExposed()
        {
            Argument argument = new("name", "The name of the user", true, "John Doe");

            Assert.That(argument.Name, Is.EqualTo("name"));
            Assert.That(argument.Description, Is.EqualTo("The name of the user"));
            Assert.That(argument.IsRequired);
            Assert.That(argument.DefaultValue, Is.EqualTo("John Doe"));
        }

        [Test]
        public void GivenNoDefaultValue_WhenConstructingAnArgument_ThenTheDefaultValueIsNull()
        {
            Argument argument = new("name", "The name of the user", false);

            Assert.That(argument.DefaultValue, Is.Null);
        }

        [Test]
        public void GivenTwoArgumentsWithTheSameValues_WhenComparingThemForEquality_ThenTheyAreNotEqual()
        {
            Argument firstArgument = new("name", "The name of the user", false);
            Argument secondArgument = new("name", "The name of the user", false);

            Assert.That(firstArgument, Is.Not.EqualTo(secondArgument));
        }
    }
}
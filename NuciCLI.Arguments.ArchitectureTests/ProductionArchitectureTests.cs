using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using ArchUnitNet;

using NUnit.Framework;

namespace NuciCLI.Arguments.ArchitectureTests
{
    [TestFixture]
    public class ProductionArchitectureTests
    {
        [Test]
        public async Task GivenProductionProject_WhenChecked_ThenItsSourceGraphHasNoCycles()
        {
            string originalDirectory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(GetRepositoryDirectory());
            var rule = ArchUnit.ProjectFiles("NuciCLI.Arguments/NuciCLI.Arguments.csproj")
                .InPath("**/*.cs")
                .Should()
                .HaveNoCycles();
            var violations = await rule.CheckAsync();
            Directory.SetCurrentDirectory(originalDirectory);

            Assert.That(violations, Is.Empty, "Production source files must not contain dependency cycles.");
        }

        [Test]
        public void GivenProductionAssembly_WhenLoaded_ThenItContainsExpectedTypes()
        {
            Type[] productionTypes = typeof(Argument).Assembly.GetTypes();

            Assert.That(productionTypes, Does.Contain(typeof(Argument)));
            Assert.That(productionTypes, Does.Contain(typeof(ArgumentParser)));
            Assert.That(productionTypes, Does.Contain(typeof(ArgumentsCollection)));
        }

        [Test]
        public void GivenProductionTypes_WhenChecked_ThenTheyUseTheProductionNamespace()
        {
            Type[] productionTypes = typeof(Argument).Assembly.GetTypes();

            Assert.That(productionTypes.Where(type => type.IsPublic), Is.Not.Empty);
            Assert.That(productionTypes.Where(type => type.IsPublic).Select(type => type.Namespace),
                Has.All.EqualTo("NuciCLI.Arguments"));
        }

        [Test]
        public void GivenProductionAssembly_WhenChecked_ThenItDoesNotExposeInternalsToTests()
        {
            InternalsVisibleToAttribute[] friendAssemblies = typeof(Argument).Assembly
                .GetCustomAttributes<InternalsVisibleToAttribute>()
                .ToArray();

            Assert.That(friendAssemblies, Is.Empty);
        }

        [Test]
        public void GivenProductionProject_WhenChecked_ThenItHasNoTestProjectReferences()
        {
            string projectText = File.ReadAllText(GetProductionProjectPath());

            Assert.That(projectText, Does.Not.Contain("ArchitectureTests"));
            Assert.That(projectText, Does.Not.Contain("UnitTests"));
        }

        [Test]
        public void GivenProductionProject_WhenChecked_ThenItTargetsNet10()
        {
            string projectText = File.ReadAllText(GetProductionProjectPath());

            Assert.That(projectText, Does.Contain("<TargetFramework>net10.0</TargetFramework>"));
        }

        [Test]
        public void GivenProductionProject_WhenChecked_ThenItEnablesNullableAndDisablesImplicitUsings()
        {
            string projectText = File.ReadAllText(GetProductionProjectPath());

            Assert.That(projectText, Does.Contain("<Nullable>enable</Nullable>"));
            Assert.That(projectText, Does.Contain("<ImplicitUsings>disable</ImplicitUsings>"));
        }

        private static string GetRepositoryDirectory()
        {
            string projectPath = typeof(ProductionArchitectureTests).Assembly
                .GetCustomAttribute<AssemblyMetadataAttribute>()?.Value
                ?? throw new InvalidOperationException("Production project metadata is unavailable.");

            return Path.GetDirectoryName(Path.GetDirectoryName(Path.GetFullPath(projectPath)))
                ?? throw new InvalidOperationException("Repository directory is unavailable.");
        }

        private static string GetProductionProjectPath()
            => Path.Combine(GetRepositoryDirectory(), "NuciCLI.Arguments", "NuciCLI.Arguments.csproj");
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NUnit.Framework;

namespace NuciCLI.Arguments.ArchitectureTests
{
    [TestFixture]
    public class SourceFileConventionTests
    {
        [Test]
        public void GivenProductionSourceFiles_WhenChecked_ThenEveryFileHasExactlyOneDeclaredType()
        {
            foreach (string sourceFile in GetProductionSourceFiles())
            {
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(sourceFile), path: sourceFile);
                int declaredTypeCount = GetDeclaredTypes(syntaxTree.GetRoot()).Count;

                Assert.That(declaredTypeCount, Is.EqualTo(1), $"File '{sourceFile}' must declare exactly one type.");
            }
        }

        [Test]
        public void GivenProductionSourceFiles_WhenChecked_ThenTypeNamesMatchFileNames()
        {
            foreach (string sourceFile in GetProductionSourceFiles())
            {
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(sourceFile), path: sourceFile);
                BaseTypeDeclarationSyntax declaredType = GetDeclaredTypes(syntaxTree.GetRoot()).Single();
                string fileName = Path.GetFileNameWithoutExtension(sourceFile);

                Assert.That(declaredType.Identifier.ValueText, Is.EqualTo(fileName),
                    $"File '{sourceFile}' must contain a type named '{fileName}'.");
            }
        }

        [Test]
        public void GivenProductionSourceFiles_WhenChecked_ThenAllTypesUseExplicitAccessibility()
        {
            foreach (string sourceFile in GetProductionSourceFiles())
            {
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(sourceFile), path: sourceFile);

                foreach (BaseTypeDeclarationSyntax declaredType in GetDeclaredTypes(syntaxTree.GetRoot()))
                {
                    SyntaxTokenList modifiers = declaredType.Modifiers;
                    bool hasAccessibility = modifiers.Any(SyntaxKind.PublicKeyword)
                        || modifiers.Any(SyntaxKind.InternalKeyword)
                        || modifiers.Any(SyntaxKind.PrivateKeyword)
                        || modifiers.Any(SyntaxKind.ProtectedKeyword);

                    Assert.That(hasAccessibility, Is.True,
                        $"Type '{declaredType.Identifier.ValueText}' in '{sourceFile}' must declare accessibility.");
                }
            }
        }

        [Test]
        public void GivenProductionSourceFiles_WhenChecked_ThenControlFlowBodiesUseBraces()
        {
            foreach (string sourceFile in GetProductionSourceFiles())
            {
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(sourceFile), path: sourceFile);
                IEnumerable<StatementSyntax> unbracedStatements = syntaxTree.GetRoot()
                    .DescendantNodes()
                    .OfType<IfStatementSyntax>()
                    .Where(statement => statement.Statement is not BlockSyntax)
                    .Select(statement => statement.Statement)
                    .Concat(syntaxTree.GetRoot().DescendantNodes().OfType<ForStatementSyntax>()
                        .Where(statement => statement.Statement is not BlockSyntax)
                        .Select(statement => statement.Statement))
                    .Concat(syntaxTree.GetRoot().DescendantNodes().OfType<WhileStatementSyntax>()
                        .Where(statement => statement.Statement is not BlockSyntax)
                        .Select(statement => statement.Statement));

                Assert.That(unbracedStatements, Is.Empty, $"File '{sourceFile}' contains an unbraced control-flow body.");
            }
        }

        private static IReadOnlyList<string> GetProductionSourceFiles()
        {
            string projectDirectory = GetProjectDirectory();
            string[] sourceFiles = Directory.GetFiles(projectDirectory, "*.cs", SearchOption.TopDirectoryOnly)
                .OrderBy(filePath => filePath, StringComparer.Ordinal)
                .ToArray();

            Assert.That(sourceFiles, Is.Not.Empty, "No production source files were found.");
            return sourceFiles;
        }

        private static IReadOnlyList<BaseTypeDeclarationSyntax> GetDeclaredTypes(SyntaxNode syntaxNode)
            => syntaxNode.DescendantNodes().OfType<BaseTypeDeclarationSyntax>().ToArray();

        private static string GetProjectDirectory()
        {
            string projectPath = typeof(SourceFileConventionTests).Assembly
                .GetCustomAttribute<AssemblyMetadataAttribute>()?.Value
                ?? throw new InvalidOperationException("Production project metadata is unavailable.");

            return Path.GetDirectoryName(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, projectPath)))
                ?? throw new InvalidOperationException("Production project directory is unavailable.");
        }
    }
}

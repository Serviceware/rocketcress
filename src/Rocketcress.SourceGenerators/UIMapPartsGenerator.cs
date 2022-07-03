using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocketcress.SourceGenerators.Common;
using Rocketcress.SourceGenerators.Extensions;
using Rocketcress.SourceGenerators.UIMapParts;
using Rocketcress.SourceGenerators.UIMapParts.Generation;
using Rocketcress.SourceGenerators.UIMapParts.Models;
using Rocketcress.SourceGenerators.UIMapParts.Selenium;
using Rocketcress.SourceGenerators.UIMapParts.UIAutomation;
using System.Collections.Immutable;

namespace Rocketcress.SourceGenerators
{
    /// <summary>
    /// Generator for creating some bolerplat code for UIMap classes.
    /// </summary>
    /// <seealso cref="IIncrementalGenerator" />
    [Generator]
    public partial class UIMapPartsGenerator : IIncrementalGenerator
    {
        /// <inheritdoc/>
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterSourceOutput(
                IncrementalValueProviderFactory
                    .From(context)
                    .GetClassDeclarationsWithGenerateUIMapPartsAttributeAndCorrectBaseType(),
                (context, source) => Execute(context, source.Compilation, source.ClassDeclarations));
        }

        private static void Execute(SourceProductionContext context, Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classDeclarations)
        {
            if (classDeclarations.Length == 0)
                return;

            var sw = Stopwatch.StartNew();

            if (!TypeSymbols.TryCreate(compilation, out var typeSymbols))
                return;

            Trace.WriteLine($"Create type symbols: {sw.Elapsed}");
            sw.Restart();

            var seleniumTypeSymbols = new SeleniumTypeSymbols(compilation);
            var uiautomationTypeSymbols = new UIAutomationTypeSymbols(compilation);

            Trace.WriteLine($"Create ui test type symbols: {sw.Elapsed}");
            sw.Restart();

            foreach (var classDeclaration in classDeclarations)
            {
                if (!ValidateClass(context, compilation, classDeclaration, out INamedTypeSymbol typeSymbol))
                    continue;

                Trace.WriteLine($"Validate class: {sw.Elapsed}");
                sw.Restart();

                UIMapPartsGeneratorContext generatorContext;
                if (seleniumTypeSymbols.HasAllSymbols && typeSymbol.IsAssignableTo(seleniumTypeSymbols.UIMapBaseClassType))
                {
                    generatorContext = new SeleniumUIMapPartsGeneratorContext(
                        compilation,
                        typeSymbols,
                        seleniumTypeSymbols,
                        typeSymbol,
                        context.ReportDiagnostic,
                        context.AddSource);
                }
                else if (uiautomationTypeSymbols.HasAllSymbols && typeSymbol.IsAssignableTo(uiautomationTypeSymbols.UIMapBaseClassType))
                {
                    generatorContext = new UIAutomationUIMapPartsGeneratorContext(
                        compilation,
                        typeSymbols,
                        uiautomationTypeSymbols,
                        typeSymbol,
                        context.ReportDiagnostic,
                        context.AddSource);
                }
                else
                {
                    var location = classDeclaration.Identifier.GetLocation();
                    var typeName = classDeclaration.Identifier.ValueText;
                    context.ReportDiagnostic(DiagnosticFactory.UIMapParts.BadBaseType(location, typeName));
                    continue;
                }

                Trace.WriteLine($"Create context: {sw.Elapsed}");
                sw.Restart();

                var data = GeneratorData.Create(generatorContext);

                Trace.WriteLine($"Create data: {sw.Elapsed}");
                sw.Restart();

                FileGenerator.Generate(generatorContext).UIMapParts(data);

                Trace.WriteLine($"Generate code: {sw.Elapsed}");
                sw.Restart();
            }
        }

        private static bool ValidateClass(SourceProductionContext context, Compilation compilation, ClassDeclarationSyntax classDeclarationSyntax, out INamedTypeSymbol typeSymbol)
        {
            var validator = ClassDeclarationValidator.Create(
                classDeclarationSyntax,
                compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree),
                context.ReportDiagnostic);

            typeSymbol = null!;
            var isValidClass =
                validator.IsPartial() &&
                validator.IsNamedTypeSymbol(out typeSymbol);

            if (!isValidClass)
                return false;

            if (validator.HasDebugGeneratorAttribute())
                CodeGenerationHelper.LaunchDebuggerOnBuild();

            return true;
        }
    }
}

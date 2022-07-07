using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocketcress.SourceGenerators.Common;
using Rocketcress.SourceGenerators.Extensions;
using Rocketcress.SourceGenerators.UIMapParts.Common;
using Rocketcress.SourceGenerators.UIMapParts.Generation;
using Rocketcress.SourceGenerators.UIMapParts.Models;
using Rocketcress.SourceGenerators.UIMapParts.Selenium;
using Rocketcress.SourceGenerators.UIMapParts.UIAutomation;
using System.Collections.Immutable;

namespace Rocketcress.SourceGenerators.UIMapParts;

/// <summary>
/// Generator for creating some bolerplat code for UIMap classes.
/// </summary>
/// <seealso cref="IIncrementalGenerator" />
[Generator]
public partial class Generator : IIncrementalGenerator
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

        if (!TypeSymbols.TryCreate(compilation, out var typeSymbols))
            return;

        var seleniumTypeSymbols = new SeleniumTypeSymbols(compilation);
        var uiautomationTypeSymbols = new UIAutomationTypeSymbols(compilation);

        foreach (var classDeclaration in classDeclarations)
        {
            if (!ValidateClass(context, compilation, classDeclaration, out INamedTypeSymbol typeSymbol))
                continue;

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
                continue;
            }

            var data = GeneratorData.Create(generatorContext);

            FileGenerator.Generate(generatorContext).UIMapParts(data);
        }
    }

    private static bool ValidateClass(SourceProductionContext context, Compilation compilation, ClassDeclarationSyntax classDeclarationSyntax, out INamedTypeSymbol typeSymbol)
    {
        var validator = ClassDeclarationValidator.Validate(
            classDeclarationSyntax,
            compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree));

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

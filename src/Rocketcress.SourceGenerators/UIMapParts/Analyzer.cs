using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Rocketcress.SourceGenerators.Common;
using Rocketcress.SourceGenerators.UIMapParts.Common;
using Rocketcress.SourceGenerators.UIMapParts.Selenium;
using Rocketcress.SourceGenerators.UIMapParts.UIAutomation;
using System.Collections.Immutable;

namespace Rocketcress.SourceGenerators.UIMapParts;

/// <summary>
/// Analyzer used to make sure everything is setup correctly for the UIMapParts Generator.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class Analyzer : DiagnosticAnalyzer
{
    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => DiagnosticFactory.UIMapParts.AllDescriptors;

    /// <inheritdoc/>
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "I assume this will never be null (called by IDE and build).")]
    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterSyntaxNodeAction(AnalyzeClassDeclaration, SyntaxKind.ClassDeclaration);
    }

    private static void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not ClassDeclarationSyntax classDeclarationSyntax)
            return;

        ValidateClass(context, classDeclarationSyntax);
    }

    private static void ValidateClass(SyntaxNodeAnalysisContext context, ClassDeclarationSyntax classDeclarationSyntax)
    {
        var validator = ClassDeclarationValidator.Validate(classDeclarationSyntax, context.SemanticModel, context.ReportDiagnostic);
        if (!validator.HasGenerateUIMapPartsAttribute())
            return;

        _ = validator.IsPartial();
        if (!validator.IsNamedTypeSymbol(out var typeSymbol))
            return;

        _ = validator.IsDerivedFromSupportedBaseType(typeSymbol);

        var loopedProperties = new HashSet<string>();
        foreach (var member in classDeclarationSyntax.Members)
        {
            if (member is not PropertyDeclarationSyntax propertyDeclarationSyntax)
                continue;

            ValidateUIMapControlProperty(context, typeSymbol, propertyDeclarationSyntax, loopedProperties);
        }
    }

    private static void ValidateUIMapControlProperty(
        SyntaxNodeAnalysisContext context,
        INamedTypeSymbol typeSymbol,
        PropertyDeclarationSyntax propertyDeclarationSyntax,
        HashSet<string> loopedProperties)
    {
        var validator = PropertyDeclarationValidator.Validate(context.SemanticModel, typeSymbol, propertyDeclarationSyntax, context.ReportDiagnostic);
        if (!validator.HasUIMapControlAttribute(out AttributeSyntax uimapControlAttributeSyntax))
            return;

        if (validator.HasExistingParentControl(uimapControlAttributeSyntax, out string parentConrol, out AttributeArgumentSyntax parentControlArgumentSyntax))
        {
            if (!loopedProperties.Contains(propertyDeclarationSyntax.Identifier.ValueText) &&
                !validator.IsNoParentLoop(parentConrol, parentControlArgumentSyntax, out var affectedProperties))
            {
                foreach (var property in affectedProperties)
                    loopedProperties.Add(property);
            }
        }
    }
}

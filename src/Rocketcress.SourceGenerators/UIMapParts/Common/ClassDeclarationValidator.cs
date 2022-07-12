using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocketcress.SourceGenerators.Extensions;
using Rocketcress.SourceGenerators.UIMapParts.Selenium;
using Rocketcress.SourceGenerators.UIMapParts.UIAutomation;

namespace Rocketcress.SourceGenerators.UIMapParts.Common;

internal readonly struct ClassDeclarationValidator
{
    private readonly ClassDeclarationSyntax _classDeclaration;
    private readonly SemanticModel _semanticModel;
    private readonly Action<Diagnostic>? _reportDiagnostic;

    private ClassDeclarationValidator(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel, Action<Diagnostic>? reportDiagnostic)
    {
        _classDeclaration = classDeclaration;
        _semanticModel = semanticModel;
        _reportDiagnostic = reportDiagnostic;
    }

    public static ClassDeclarationValidator Validate(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel)
        => new(classDeclaration, semanticModel, null);

    public static ClassDeclarationValidator Validate(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel, Action<Diagnostic> reportDiagnostic)
        => new(classDeclaration, semanticModel, reportDiagnostic);

    public bool HasGenerateUIMapPartsAttribute()
        => _classDeclaration.TryGetAttributeSyntax(_semanticModel, TypeSymbols.Names.GenerateUIMapPartsAttribute, out _);

    public bool HasDebugGeneratorAttribute()
        => _classDeclaration.TryGetAttributeSyntax(_semanticModel, TypeSymbols.Names.DebugGeneratorAttribute, out _);

    public bool IsNamedTypeSymbol(out INamedTypeSymbol typeSymbol)
        => (typeSymbol = RetrieveTypeSymbol()!) is not null;

    public bool IsPartial()
    {
        if (!_classDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword))
        {
            _reportDiagnostic?.Invoke(DiagnosticFactory.UIMapParts.ClassMustBePartial(_classDeclaration));
            return false;
        }

        var parent = _classDeclaration.Parent;
        while (parent is ClassDeclarationSyntax parentClassDeclaration)
        {
            if (!parentClassDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword))
            {
                _reportDiagnostic?.Invoke(DiagnosticFactory.UIMapParts.AllParentTypesMustBePartial(_classDeclaration, parentClassDeclaration));
                return false;
            }

            parent = parentClassDeclaration.Parent;
        }

        return true;
    }

    public bool IsDerivedFromSupportedBaseType(INamedTypeSymbol typeSymbol)
    {
        if (!typeSymbol.IsAssignableToAny(SeleniumTypeSymbols.Names.UIMapBaseClassType, UIAutomationTypeSymbols.Names.UIMapBaseClassType))
        {
            _reportDiagnostic?.Invoke(DiagnosticFactory.UIMapParts.BadBaseType(_classDeclaration));
            return false;
        }

        return true;
    }

    private INamedTypeSymbol? RetrieveTypeSymbol() => _semanticModel.GetDeclaredSymbol(_classDeclaration);
}

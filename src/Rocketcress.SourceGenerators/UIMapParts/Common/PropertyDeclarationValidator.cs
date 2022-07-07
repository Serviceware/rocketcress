using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocketcress.SourceGenerators.Extensions;

namespace Rocketcress.SourceGenerators.UIMapParts.Common;

internal readonly struct PropertyDeclarationValidator
{
    private readonly SemanticModel _semanticModel;
    private readonly INamedTypeSymbol _typeSymbol;
    private readonly PropertyDeclarationSyntax _propertyDeclarationSyntax;
    private readonly Action<Diagnostic> _reportDiagnostic;

    private PropertyDeclarationValidator(SemanticModel semanticModel, INamedTypeSymbol typeSymbol, PropertyDeclarationSyntax propertyDeclarationSyntax, Action<Diagnostic> reportDiagnostic)
    {
        _semanticModel = semanticModel;
        _typeSymbol = typeSymbol;
        _propertyDeclarationSyntax = propertyDeclarationSyntax;
        _reportDiagnostic = reportDiagnostic;
    }

    public static PropertyDeclarationValidator Validate(SemanticModel semanticModel, INamedTypeSymbol typeSymbol, PropertyDeclarationSyntax propertyDeclarationSyntax, Action<Diagnostic> reportDiagnostic)
        => new(semanticModel, typeSymbol, propertyDeclarationSyntax, reportDiagnostic);

    public bool HasUIMapControlAttribute(out AttributeSyntax uimapControlAttributeSyntax)
    {
        return _propertyDeclarationSyntax.TryGetAttributeSyntax(_semanticModel, TypeSymbols.Names.UIMapControlAttribute, out uimapControlAttributeSyntax);
    }

    public bool HasExistingParentControl(AttributeSyntax uimapControlAttributeSyntax, out string? parentControl)
    {
        parentControl = null;

        var argumentSyntax = uimapControlAttributeSyntax.GetNamedArgumentSyntax("ParentControl");
        if (argumentSyntax is null ||
            !_semanticModel.TryGetDeclaredSymbol(_propertyDeclarationSyntax, out IPropertySymbol propertySymbol) ||
            !propertySymbol.TryGetAttribute(TypeSymbols.Names.UIMapControlAttribute, out AttributeData uimapControlAttribute))
        {
            return false;
        }

        foreach (var argument in uimapControlAttribute.NamedArguments)
        {
            if (argument.Key == "ParentControl")
                parentControl = argument.Value.Value as string;
        }

        if (parentControl is null)
            return false;

        foreach (var member in _typeSymbol.GetMembers(parentControl))
        {
            if (member is IPropertySymbol)
                return true;
        }

        _reportDiagnostic(
            DiagnosticFactory.UIMapParts.MissingParentControl(
                argumentSyntax.Expression.GetLocation(),
                _typeSymbol.Name,
                _propertyDeclarationSyntax.Identifier.ValueText,
                parentControl));

        return false;
    }
}

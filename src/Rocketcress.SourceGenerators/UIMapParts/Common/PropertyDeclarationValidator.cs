using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocketcress.SourceGenerators.Extensions;
using System.Linq;

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

    public bool HasExistingParentControl(AttributeSyntax uimapControlAttributeSyntax, out string parentControl, out AttributeArgumentSyntax argumentSyntax)
    {
        if (!TryGetParentControl(uimapControlAttributeSyntax, out parentControl, out argumentSyntax))
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

    public bool IsNoParentLoop(string? parentControl, AttributeArgumentSyntax argumentSyntax, out IEnumerable<string> affectedProperties)
    {
        var visited = new List<string> { _propertyDeclarationSyntax.Identifier.ValueText };
        var current = parentControl;

        while (current is not null)
        {
            if (visited.Contains(current))
            {
                affectedProperties = visited;
                _reportDiagnostic(
                    DiagnosticFactory.UIMapParts.ParentLoop(
                        argumentSyntax.Expression.GetLocation(),
                        _typeSymbol.Name,
                        _propertyDeclarationSyntax.Identifier.ValueText,
                        visited.Append(current)));
                return false;
            }

            visited.Add(current);
            current = GetParentControl(current);
        }

        affectedProperties = Array.Empty<string>();
        return true;
    }

    private string? GetParentControl(string control)
    {
        IPropertySymbol? propertySymbol = null;
        foreach (var member in _typeSymbol.GetMembers(control))
        {
            if (member is IPropertySymbol symbol)
            {
                propertySymbol = symbol;
                break;
            }
        }

        if (propertySymbol is null ||
            !propertySymbol.TryGetAttribute(TypeSymbols.Names.UIMapControlAttribute, out AttributeData uimapControlAttribute))
        {
            return null;
        }

        string? parentControl = null;
        foreach (var argument in uimapControlAttribute.NamedArguments)
        {
            if (argument.Key == "ParentControl")
                parentControl = (argument.Value.Value as string)!;
        }

        return parentControl;
    }

    private bool TryGetParentControl(AttributeSyntax uimapControlAttributeSyntax, out string parentControl, out AttributeArgumentSyntax parentArgumentSyntax)
    {
        parentControl = null!;

        parentArgumentSyntax = uimapControlAttributeSyntax.GetNamedArgumentSyntax("ParentControl")!;
        if (parentArgumentSyntax is null ||
            !_semanticModel.TryGetDeclaredSymbol(_propertyDeclarationSyntax, out IPropertySymbol propertySymbol) ||
            !propertySymbol.TryGetAttribute(TypeSymbols.Names.UIMapControlAttribute, out AttributeData uimapControlAttribute))
        {
            return false;
        }

        foreach (var argument in uimapControlAttribute.NamedArguments)
        {
            if (argument.Key == "ParentControl")
                parentControl = (argument.Value.Value as string)!;
        }

        return parentControl is not null;
    }
}

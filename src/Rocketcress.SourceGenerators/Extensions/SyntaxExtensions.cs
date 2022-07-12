using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rocketcress.SourceGenerators.Extensions;

internal static class SyntaxExtensions
{
    public static bool TryGetAttributeSyntax(this MemberDeclarationSyntax memberDeclarationSyntax, SemanticModel semanticModel, string attributeTypeName, out AttributeSyntax attribute)
    {
        foreach (var attributeListSyntax in memberDeclarationSyntax.AttributeLists)
        {
            foreach (var attributeSyntax in attributeListSyntax.Attributes)
            {
                if (semanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                    continue;

                INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                string fullName = attributeContainingTypeSymbol.ToDisplayString();

                if (fullName == attributeTypeName)
                {
                    attribute = attributeSyntax;
                    return true;
                }
            }
        }

        attribute = null!;
        return false;
    }

    public static AttributeArgumentSyntax? GetNamedArgumentSyntax(this AttributeSyntax attributeSyntax, string namedArgumentName)
    {
        if (attributeSyntax.ArgumentList is null)
            return null;

        foreach (var argumentSyntax in attributeSyntax.ArgumentList.Arguments)
        {
            if (argumentSyntax.NameEquals?.Name.Identifier.ValueText == namedArgumentName)
                return argumentSyntax;
        }

        return null;
    }
}

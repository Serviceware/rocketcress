using Microsoft.CodeAnalysis;

namespace Rocketcress.SourceGenerators.Extensions;

internal static class SemanticModelExtensions
{
    public static bool TryGetDeclaredSymbol<T>(this SemanticModel semanticModel, SyntaxNode declaration, out T symbol)
        where T : class, ISymbol
    {
        symbol = (semanticModel.GetDeclaredSymbol(declaration) as T)!;
        return symbol is not null;
    }
}

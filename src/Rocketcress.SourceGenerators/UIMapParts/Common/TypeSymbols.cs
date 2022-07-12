using Microsoft.CodeAnalysis;

namespace Rocketcress.SourceGenerators.UIMapParts.Common;

internal class TypeSymbols
{
    private TypeSymbols(Compilation compilation)
    {
        Func1 = compilation.GetTypeByMetadataName("System.Func`1")!;
        Func2 = compilation.GetTypeByMetadataName("System.Func`2")!;
        DebugGeneratorAttribute = compilation.GetTypeByMetadataName(Names.DebugGeneratorAttribute);
        GenerateUIMapPartsAttribute = compilation.GetTypeByMetadataName(Names.GenerateUIMapPartsAttribute)!;
        UIMapControlAttribute = compilation.GetTypeByMetadataName(Names.UIMapControlAttribute)!;
    }

    public INamedTypeSymbol Func1 { get; }
    public INamedTypeSymbol Func2 { get; }

    public INamedTypeSymbol? DebugGeneratorAttribute { get; }
    public INamedTypeSymbol GenerateUIMapPartsAttribute { get; }
    public INamedTypeSymbol UIMapControlAttribute { get; }

    private bool HasAllSymbols =>
        Func1 is not null &&
        Func2 is not null &&
        GenerateUIMapPartsAttribute is not null &&
        UIMapControlAttribute is not null;

    public static bool TryCreate(Compilation compilation, out TypeSymbols typeSymbols)
    {
        typeSymbols = new TypeSymbols(compilation);
        return typeSymbols.HasAllSymbols;
    }

    internal static class Names
    {
        internal const string DebugGeneratorAttribute = "Rocketcress.Core.Attributes.DebugGeneratorAttribute";
        internal const string GenerateUIMapPartsAttribute = "Rocketcress.Core.Attributes.GenerateUIMapPartsAttribute";
        internal const string UIMapControlAttribute = "Rocketcress.Core.Attributes.UIMapControlAttribute";
    }
}

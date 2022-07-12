using Microsoft.CodeAnalysis;

namespace Rocketcress.SourceGenerators.Tests.Validators;

public static class PropertySymbolValidations
{
    public static ISymbolValidator<IPropertySymbol> IsOfType<T>(this ISymbolValidator<IPropertySymbol> validator)
        => IsOfType(validator, typeof(T));

    public static ISymbolValidator<IPropertySymbol> IsOfType(this ISymbolValidator<IPropertySymbol> validator, Type type)
        => IsOfType(validator, validator.Compilation.GetTypeSymbolFromType(type));

    public static ISymbolValidator<IPropertySymbol> IsOfType(this ISymbolValidator<IPropertySymbol> validator, ITypeSymbol typeSymbol)
    {
        Assert.Instance.AreEqual(typeSymbol, validator.Symbol.Type, $"The type of property \"{validator.Symbol.Name}\" in type \"{validator.TryGetParent<ISymbolValidator<INamedTypeSymbol>>()?.GetTypeName()}\" is not correct.");
        return validator;
    }
}

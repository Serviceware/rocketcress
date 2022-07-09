using MaSch.Test.Assertion;
using Microsoft.CodeAnalysis;

namespace Rocketcress.SourceGenerators.Tests.Validators;

public static class NamedTypeSymbolValidations
{
    public static string GetTypeName(this ISymbolValidator<INamedTypeSymbol> validator)
        => validator.Symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

    public static ISymbolValidator<INamedTypeSymbol> HasProperty(this ISymbolValidator<INamedTypeSymbol> validator, string propertyName)
        => HasProperty(validator, propertyName, null);
    public static ISymbolValidator<INamedTypeSymbol> HasProperty(this ISymbolValidator<INamedTypeSymbol> validator, string propertyName, Action<ISymbolValidator<IPropertySymbol>>? propertyValidation)
    {
        var property = validator.Symbol.GetMembers(propertyName).OfType<IPropertySymbol>().FirstOrDefault();
        Assert.Instance.IsNotNull(property, $"Property \"{propertyName}\" does not exist in type \"{GetTypeName(validator)}\".");

        if (propertyValidation is not null)
        {
            var propertyValidator = new SymbolValidator<IPropertySymbol>(property, validator);
            propertyValidation(propertyValidator);
        }

        return validator;
    }

    public static ISymbolValidator<INamedTypeSymbol> DoesNotHaveProperty(this ISymbolValidator<INamedTypeSymbol> validator, string propertyName)
        => DoesNotHaveMember<IPropertySymbol>(validator, propertyName, null, "Property");

    public static ISymbolValidator<INamedTypeSymbol> DoesNotHaveMethod(this ISymbolValidator<INamedTypeSymbol> validator, string methodName)
        => DoesNotHaveMember<IMethodSymbol>(validator, methodName, x => x.MethodKind == MethodKind.Ordinary, "Method");

    public static ISymbolValidator<INamedTypeSymbol> HasMethod(this ISymbolValidator<INamedTypeSymbol> validator, string methodName)
        => HasMethod(validator, out _, methodName, MethodKind.Ordinary, (ITypeSymbol[]?)null);

    public static ISymbolValidator<INamedTypeSymbol> HasMethod(this ISymbolValidator<INamedTypeSymbol> validator, string methodName, params Type[]? parameterTypes)
        => HasMethod(validator, out _, methodName, MethodKind.Ordinary, validator.Compilation.GetTypeSymbols(parameterTypes));

    public static ISymbolValidator<INamedTypeSymbol> HasMethod(this ISymbolValidator<INamedTypeSymbol> validator, string methodName, params object[]? parameterTypes)
        => HasMethod(validator, out _, methodName, MethodKind.Ordinary, validator.Compilation.GetTypeSymbols(parameterTypes));

    public static ISymbolValidator<INamedTypeSymbol> HasMethod(this ISymbolValidator<INamedTypeSymbol> validator, string methodName, params ITypeSymbol[]? parameterTypeSymbols)
        => HasMethod(validator, out _, methodName, MethodKind.Ordinary, parameterTypeSymbols);

    public static ISymbolValidator<INamedTypeSymbol> HasMethod(this ISymbolValidator<INamedTypeSymbol> validator, string methodName, MethodKind kind)
        => HasMethod(validator, out _, methodName, kind, (ITypeSymbol[]?)null);

    public static ISymbolValidator<INamedTypeSymbol> HasMethod(this ISymbolValidator<INamedTypeSymbol> validator, string methodName, MethodKind kind, params Type[]? parameterTypes)
        => HasMethod(validator, out _, methodName, kind, validator.Compilation.GetTypeSymbols(parameterTypes));

    public static ISymbolValidator<INamedTypeSymbol> HasMethod(this ISymbolValidator<INamedTypeSymbol> validator, string methodName, MethodKind kind, params object[]? parameterTypes)
        => HasMethod(validator, out _, methodName, kind, validator.Compilation.GetTypeSymbols(parameterTypes));

    public static ISymbolValidator<INamedTypeSymbol> HasMethod(this ISymbolValidator<INamedTypeSymbol> validator, string methodName, MethodKind kind, params ITypeSymbol[]? parameterTypeSymbols)
        => HasMethod(validator, out _, methodName, kind, parameterTypeSymbols);

    public static ISymbolValidator<INamedTypeSymbol> HasMethod(this ISymbolValidator<INamedTypeSymbol> validator, out IMethodSymbol methodSymbol, string methodName)
        => HasMethod(validator, out methodSymbol, methodName, MethodKind.Ordinary, (ITypeSymbol[]?)null);

    public static ISymbolValidator<INamedTypeSymbol> HasMethod(this ISymbolValidator<INamedTypeSymbol> validator, out IMethodSymbol methodSymbol, string methodName, params Type[]? parameterTypes)
        => HasMethod(validator, out methodSymbol, methodName, MethodKind.Ordinary, validator.Compilation.GetTypeSymbols(parameterTypes));

    public static ISymbolValidator<INamedTypeSymbol> HasMethod(this ISymbolValidator<INamedTypeSymbol> validator, out IMethodSymbol methodSymbol, string methodName, params object[]? parameterTypes)
        => HasMethod(validator, out methodSymbol, methodName, MethodKind.Ordinary, validator.Compilation.GetTypeSymbols(parameterTypes));

    public static ISymbolValidator<INamedTypeSymbol> HasMethod(this ISymbolValidator<INamedTypeSymbol> validator, out IMethodSymbol methodSymbol, string methodName, params ITypeSymbol[]? parameterTypeSymbols)
        => HasMethod(validator, out methodSymbol, methodName, MethodKind.Ordinary, parameterTypeSymbols);

    public static ISymbolValidator<INamedTypeSymbol> HasMethod(this ISymbolValidator<INamedTypeSymbol> validator, out IMethodSymbol methodSymbol, string methodName, MethodKind kind)
        => HasMethod(validator, out methodSymbol, methodName, kind, (ITypeSymbol[]?)null);

    public static ISymbolValidator<INamedTypeSymbol> HasMethod(this ISymbolValidator<INamedTypeSymbol> validator, out IMethodSymbol methodSymbol, string methodName, MethodKind kind, params Type[]? parameterTypes)
        => HasMethod(validator, out methodSymbol, methodName, kind, validator.Compilation.GetTypeSymbols(parameterTypes));

    public static ISymbolValidator<INamedTypeSymbol> HasMethod(this ISymbolValidator<INamedTypeSymbol> validator, out IMethodSymbol methodSymbol, string methodName, MethodKind kind, params object[]? parameterTypes)
        => HasMethod(validator, out methodSymbol, methodName, kind, validator.Compilation.GetTypeSymbols(parameterTypes));

    public static ISymbolValidator<INamedTypeSymbol> HasMethod(this ISymbolValidator<INamedTypeSymbol> validator, out IMethodSymbol methodSymbol, string methodName, MethodKind kind, params ITypeSymbol[]? parameterTypeSymbols)
    {
        var method = (from m in validator.Symbol.GetMembers(methodName).OfType<IMethodSymbol>()
                      where m.MethodKind == kind && (
                          parameterTypeSymbols == null ||
                          m.Parameters.Select(x => x.Type).SequenceEqual(parameterTypeSymbols, SymbolEqualityComparer.Default))
                      select m).FirstOrDefault();
        Assert.Instance.IsNotNull(method, GetErrorMessage());
        methodSymbol = method;

        return validator;

        string GetErrorMessage()
        {
            return parameterTypeSymbols is null
                ? $"Method \"{methodName}\" of kind {kind} does not exist in type \"{GetTypeName(validator)}\"."
                : $"Method \"{methodName}\" of kind {kind} with {parameterTypeSymbols.Length} parameter(s) ({string.Join(", ", parameterTypeSymbols.Select(x => x.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)))}) does not exist in type \"{GetTypeName(validator)}\".";
        }
    }

    public static ISymbolValidator<INamedTypeSymbol> HasConstructor(this ISymbolValidator<INamedTypeSymbol> validator, params Type[] parameterTypes)
        => HasConstructor(validator, out _, parameterTypes);

    public static ISymbolValidator<INamedTypeSymbol> HasConstructor(this ISymbolValidator<INamedTypeSymbol> validator, out IMethodSymbol methodSymbol, params Type[] parameterTypes)
        => HasConstructor(validator, out methodSymbol, validator.Compilation.GetTypeSymbols(parameterTypes));

    public static ISymbolValidator<INamedTypeSymbol> HasConstructor(this ISymbolValidator<INamedTypeSymbol> validator, params object[] parameterTypes)
        => HasConstructor(validator, out _, parameterTypes);

    public static ISymbolValidator<INamedTypeSymbol> HasConstructor(this ISymbolValidator<INamedTypeSymbol> validator, out IMethodSymbol methodSymbol, params object[] parameterTypes)
        => HasConstructor(validator, out methodSymbol, validator.Compilation.GetTypeSymbols(parameterTypes));

    public static ISymbolValidator<INamedTypeSymbol> HasConstructor(this ISymbolValidator<INamedTypeSymbol> validator, params ITypeSymbol[] parameterTypeSymbols)
        => HasConstructor(validator, out _, parameterTypeSymbols);

    public static ISymbolValidator<INamedTypeSymbol> HasConstructor(this ISymbolValidator<INamedTypeSymbol> validator, out IMethodSymbol methodSymbol, params ITypeSymbol[] parameterTypeSymbols)
    {
        var constructor = (from ctor in validator.Symbol.InstanceConstructors
                           where ctor.Parameters.Select(x => x.Type).SequenceEqual(parameterTypeSymbols, SymbolEqualityComparer.Default)
                           select ctor).FirstOrDefault();
        Assert.Instance.IsNotNull(constructor, $"A construcotr with {parameterTypeSymbols.Length} parameter(s) ({string.Join(", ", parameterTypeSymbols.Select(x => x.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)))}) was not found on type \"{GetTypeName(validator)}\".");
        methodSymbol = constructor;
        return validator;
    }

    public static ISymbolValidator<INamedTypeSymbol> HasMembers(this ISymbolValidator<INamedTypeSymbol> validator, SymbolKind kind, int count)
    {
        var propertyCount = validator.Symbol.GetMembers().Where(x => x.Kind == kind).Count();
        Assert.Instance.AreEqual(count, propertyCount, $"The type \"{GetTypeName(validator)}\" has the wrong number of members of kind {kind}.");
        return validator;
    }

    public static ISymbolValidator<INamedTypeSymbol> HasMembers(this ISymbolValidator<INamedTypeSymbol> validator, MethodKind kind, int count)
    {
        var propertyCount = validator.Symbol.GetMembers().OfType<IMethodSymbol>().Where(x => x.MethodKind == kind).Count();
        Assert.Instance.AreEqual(count, propertyCount, $"The type \"{GetTypeName(validator)}\" has the wrong number of methods of kind {kind}.");
        return validator;
    }

    public static ISymbolValidator<INamedTypeSymbol> HasMembers(this ISymbolValidator<INamedTypeSymbol> validator, SymbolKind kind, Accessibility accessibility, int count)
    {
        var propertyCount = validator.Symbol.GetMembers().Where(x => x.Kind == kind && x.DeclaredAccessibility == accessibility).Count();
        Assert.Instance.AreEqual(count, propertyCount, $"The type \"{GetTypeName(validator)}\" has the wrong number of {accessibility} members of kind {kind}.");
        return validator;
    }

    public static ISymbolValidator<INamedTypeSymbol> HasMembers(this ISymbolValidator<INamedTypeSymbol> validator, MethodKind kind, Accessibility accessibility, int count)
    {
        var propertyCount = validator.Symbol.GetMembers().OfType<IMethodSymbol>().Where(x => x.MethodKind == kind && x.DeclaredAccessibility == accessibility).Count();
        Assert.Instance.AreEqual(count, propertyCount, $"The type \"{GetTypeName(validator)}\" has the wrong number of {accessibility} methods of kind {kind}.");
        return validator;
    }

    public static ISymbolValidator<INamedTypeSymbol> ValidateMethod(this ISymbolValidator<INamedTypeSymbol> validator, IMethodSymbol methodSymbol, Action<ISymbolValidator<IMethodSymbol>> methodValidation)
    {
        var methodValidator = new SymbolValidator<IMethodSymbol>(methodSymbol, validator);
        methodValidation(methodValidator);
        return validator;
    }

    private static ISymbolValidator<INamedTypeSymbol> DoesNotHaveMember<T>(ISymbolValidator<INamedTypeSymbol> validator, string name, Func<T, bool>? predicate, string memberTypeDisplayName)
        where T : ISymbol
    {
        var members = validator.Symbol.GetMembers(name).OfType<T>();
        if (predicate is not null)
            members = members.Where(predicate);
        var member = members.FirstOrDefault();
        Assert.Instance.IsNull(member, $"{memberTypeDisplayName} \"{name}\" exist unexpectedly in type \"{GetTypeName(validator)}\".");
        return validator;
    }
}
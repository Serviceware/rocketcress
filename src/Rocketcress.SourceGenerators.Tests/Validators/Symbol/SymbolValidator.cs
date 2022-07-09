﻿using MaSch.Test.Assertion;
using Microsoft.CodeAnalysis;

namespace Rocketcress.SourceGenerators.Tests.Validators;

public interface ISymbolValidator : IValidator
{
    new ISymbolValidator? Parent { get; }
    ISymbol Symbol { get; }
}

public interface ISymbolValidator<T> : ISymbolValidator
    where T : ISymbol
{
    new T Symbol { get; }
}

public class SymbolValidator<T> : ISymbolValidator<T>
    where T : ISymbol
{
    private readonly IValidator? _parent;

    public SymbolValidator(T symbol, CompilationValidator compilation)
    {
        Symbol = symbol;
        Parent = null;
        _parent = null;
        Compilation = compilation;
    }

    public SymbolValidator(T symbol, IValidator? parent)
    {
        Symbol = symbol;
        if (parent is ISymbolValidator symbolValidator)
            Parent = symbolValidator;
        _parent = parent;
        Compilation = parent.Compilation;
    }

    public T Symbol { get; }
    public ISymbolValidator? Parent { get; }
    public CompilationValidator Compilation { get; }

    ISymbol ISymbolValidator.Symbol => Symbol;
    IValidator? IValidator.Parent => _parent;
}

public static partial class SymbolValidatorExtensions
{
    public static T TryGetParent<T>(this ISymbolValidator validator)
        where T : ISymbolValidator
    {
        if (validator.Parent is T parent)
            return parent;
        return default;
    }

    public static ISymbolValidator<T> HasAccessibility<T>(this ISymbolValidator<T> validator, Accessibility expectedAccessibility)
    where T : ISymbol
    {
        Assert.Instance.AreEqual(expectedAccessibility, validator.Symbol.DeclaredAccessibility, $"Wrong accessiblity for symbol \"{validator.Symbol}\".");
        return validator;
    }

    public static ISymbolValidator<T> IsStatic<T>(this ISymbolValidator<T> validator)
        where T : ISymbol
    {
        Assert.Instance.IsTrue(validator.Symbol.IsStatic);
        return validator;
    }

    public static ISymbolValidator<T> IsNotStatic<T>(this ISymbolValidator<T> validator)
        where T : ISymbol
    {
        Assert.Instance.IsFalse(validator.Symbol.IsStatic);
        return validator;
    }

    public static ISymbolValidator<T> HasName<T>(this ISymbolValidator<T> validator, string name)
        where T : ISymbol
    {
        Assert.Instance.AreEqual(name, validator.Symbol.Name);
        return validator;
    }
}

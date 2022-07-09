using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = MaSch.Test.Assertion.Assert;

namespace Rocketcress.SourceGenerators.Tests.Validators;

public static class MethodSymbolValidations
{
    public static ISymbolValidator<IMethodSymbol> IsPartial(this ISymbolValidator<IMethodSymbol> validator)
    {
        bool isPartial = (from syntaxRef in validator.Symbol.DeclaringSyntaxReferences
                          let syntax = syntaxRef.GetSyntax()
                          where syntax is MethodDeclarationSyntax declarationSyntax
                              && declarationSyntax.Modifiers.Any(SyntaxKind.PartialKeyword)
                          select syntax).Any();
        Assert.Instance.IsTrue(isPartial);
        return validator;
    }

    public static ISymbolValidator<IMethodSymbol> HasBodyExpression<T>(this ISymbolValidator<IMethodSymbol> validator)
        where T : ExpressionSyntax
        => HasBodyExpression<T>(validator, null, null);

    public static ISymbolValidator<IMethodSymbol> HasBodyExpression<T>(this ISymbolValidator<IMethodSymbol> validator, Action<ISyntaxNodeValidator<T>>? validation)
        where T : ExpressionSyntax
        => HasBodyExpression(validator, null, validation);

    public static ISymbolValidator<IMethodSymbol> HasBodyExpression<T>(this ISymbolValidator<IMethodSymbol> validator, string? logName)
        where T : ExpressionSyntax
        => HasBodyExpression<T>(validator, logName, null);

    public static ISymbolValidator<IMethodSymbol> HasBodyExpression<T>(this ISymbolValidator<IMethodSymbol> validator, string? logName, Action<ISyntaxNodeValidator<T>>? validation)
    where T : ExpressionSyntax
    {
        foreach (var syntaxRef in validator.Symbol.DeclaringSyntaxReferences)
        {
            if (syntaxRef.GetSyntax() is not MethodDeclarationSyntax declarationSyntax)
                continue;

            if (declarationSyntax.ExpressionBody is not null &&
                declarationSyntax.ExpressionBody.Expression is T expressionBodyExpression &&
                TryValidate(validator, expressionBodyExpression, validation))
            {
                return validator;
            }

            if (declarationSyntax.Body is not null)
            {
                foreach (var statement in declarationSyntax.Body.Statements)
                {
                    if (statement is ExpressionStatementSyntax expressionStatement &&
                        expressionStatement.Expression is T statementExpression &&
                        TryValidate(validator, statementExpression, validation))
                    {
                        return validator;
                    }
                }
            }
        }

        Assert.Instance.Fail("No body expression found with the given validation." + (string.IsNullOrWhiteSpace(logName) ? string.Empty : $" ({logName})"));
        return validator;
    }

    public static ISymbolValidator<IMethodSymbol> IsContainedIn(this ISymbolValidator<IMethodSymbol> validator, Type typeSymbol)
        => IsContainedIn(validator, validator.Compilation.GetTypeSymbolFromType(typeSymbol));

    public static ISymbolValidator<IMethodSymbol> IsContainedIn(this ISymbolValidator<IMethodSymbol> validator, INamedTypeSymbol typeSymbol)
    {
        Assert.Instance.AreEqual(typeSymbol, validator.Symbol.ContainingType);
        return validator;
    }

    public static ISymbolValidator<IMethodSymbol> HasParameters(this ISymbolValidator<IMethodSymbol> validator)
        => HasParameters(validator, Array.Empty<INamedTypeSymbol>());

    public static ISymbolValidator<IMethodSymbol> HasParameters(this ISymbolValidator<IMethodSymbol> validator, params object[] types)
        => HasParameters(validator, validator.Compilation.GetTypeSymbols(types));

    public static ISymbolValidator<IMethodSymbol> HasParameters(this ISymbolValidator<IMethodSymbol> validator, params Type[]? types)
        => HasParameters(validator, validator.Compilation.GetTypeSymbols(types));

    public static ISymbolValidator<IMethodSymbol> HasParameters(this ISymbolValidator<IMethodSymbol> validator, params INamedTypeSymbol[] typeSymbols)
    {
        Assert.Instance.AreCollectionsEqual(
            validator.Symbol.Parameters.Select(x => x.Type),
            typeSymbols,
            SymbolEqualityComparer.Default);
        return validator;
    }

    private static bool TryValidate<T>(ISymbolValidator<IMethodSymbol> validator, T syntaxNode, Action<ISyntaxNodeValidator<T>>? validation)
        where T : SyntaxNode
    {
        if (validation is null)
            return true;

        try
        {
            var syntaxValidator = new SyntaxNodeValidator<T>(syntaxNode, validator.Compilation);
            validation(syntaxValidator);
            return true;
        }
        catch (AssertFailedException)
        {
            return false;
        }
    }
}

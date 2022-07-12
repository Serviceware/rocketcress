using Microsoft.CodeAnalysis;
using Rocketcress.SourceGenerators.Tests.Validators;

namespace Rocketcress.SourceGenerators.Tests.Extensions;

public static class ActionExtensions
{
    public static void TryInvoke<T>(this Action<ISyntaxNodeValidator<T>>? action, CompilationValidator parentValidator, T value)
        where T : SyntaxNode
    {
        if (action is not null)
        {
            var validator = new SyntaxNodeValidator<T>(value, parentValidator);
            action(validator);
        }
    }

    public static void TryInvoke<T>(this Action<ISyntaxNodeValidator<T>>? action, IValidator parentValidator, T value)
    where T : SyntaxNode
    {
        if (action is not null)
        {
            var validator = new SyntaxNodeValidator<T>(value, parentValidator);
            action(validator);
        }
    }

    public static void TryInvoke<T>(this Action<ISymbolValidator<T>>? action, CompilationValidator parentValidator, T value)
        where T : ISymbol
    {
        if (action is not null)
        {
            var validator = new SymbolValidator<T>(value, parentValidator);
            action(validator);
        }
    }

    public static void TryInvoke<T>(this Action<ISymbolValidator<T>>? action, IValidator parentValidator, T value)
    where T : ISymbol
    {
        if (action is not null)
        {
            var validator = new SymbolValidator<T>(value, parentValidator);
            action(validator);
        }
    }
}

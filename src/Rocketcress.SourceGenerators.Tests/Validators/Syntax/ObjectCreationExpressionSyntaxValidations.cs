using MaSch.Test.Assertion;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rocketcress.SourceGenerators.Tests.Validators;

public static class ObjectCreationExpressionSyntaxValidations
{
    public static ISyntaxNodeValidator<ObjectCreationExpressionSyntax> HasArguments(this ISyntaxNodeValidator<ObjectCreationExpressionSyntax> validator, int count)
    {
        Assert.Instance.AreEqual(count, validator.SyntaxNode.ArgumentList.Arguments.Count);
        return validator;
    }

    public static ISyntaxNodeValidator<ObjectCreationExpressionSyntax> HasArgument<T>(this ISyntaxNodeValidator<ObjectCreationExpressionSyntax> validator, int index)
        where T : SyntaxNode
        => HasArgument<T>(validator, index, null);

    public static ISyntaxNodeValidator<ObjectCreationExpressionSyntax> HasArgument<T>(this ISyntaxNodeValidator<ObjectCreationExpressionSyntax> validator, int index, Action<ISyntaxNodeValidator<T>>? validation)
        where T : SyntaxNode
    {
        Assert.Instance.IsGreaterThan(index, validator.SyntaxNode.ArgumentList.Arguments.Count);
        var argument = validator.SyntaxNode.ArgumentList.Arguments[index];
        var expression = Assert.Instance.IsInstanceOfType<T>(argument.Expression);

        if (validation is not null)
        {
            var argumentValidator = new SyntaxNodeValidator<T>(expression, validator);
            validation(argumentValidator);
        }

        return validator;
    }
}

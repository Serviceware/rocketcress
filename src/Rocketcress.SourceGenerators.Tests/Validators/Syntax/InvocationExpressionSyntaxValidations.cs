using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocketcress.SourceGenerators.Tests.Extensions;

namespace Rocketcress.SourceGenerators.Tests.Validators;

public static class InvocationExpressionSyntaxValidations
{
    public static ISyntaxNodeValidator<InvocationExpressionSyntax> HasNoArguments(this ISyntaxNodeValidator<InvocationExpressionSyntax> validator)
    {
        ValidateArgumentCount(validator, 0);
        return validator;
    }

    public static ISyntaxNodeValidator<InvocationExpressionSyntax> HasArguments<T1>(this ISyntaxNodeValidator<InvocationExpressionSyntax> validator, Action<ISyntaxNodeValidator<T1>>? argument1Validation = null)
        where T1 : ExpressionSyntax
    {
        ValidateArgumentCount(validator, 1);
        ValidateArgumentSyntax(validator, 0, argument1Validation);
        return validator;
    }

    public static ISyntaxNodeValidator<InvocationExpressionSyntax> HasArguments<T1, T2>(this ISyntaxNodeValidator<InvocationExpressionSyntax> validator, Action<ISyntaxNodeValidator<T1>>? argument1Validation = null, Action<ISyntaxNodeValidator<T2>>? argument2Validation = null)
        where T1 : ExpressionSyntax
        where T2 : ExpressionSyntax
    {
        ValidateArgumentCount(validator, 2);
        ValidateArgumentSyntax(validator, 0, argument1Validation);
        ValidateArgumentSyntax(validator, 1, argument2Validation);
        return validator;
    }

    public static ISyntaxNodeValidator<InvocationExpressionSyntax> HasArguments<T1, T2, T3>(this ISyntaxNodeValidator<InvocationExpressionSyntax> validator, Action<ISyntaxNodeValidator<T1>>? argument1Validation = null, Action<ISyntaxNodeValidator<T2>>? argument2Validation = null, Action<ISyntaxNodeValidator<T3>>? argument3Validation = null)
        where T1 : ExpressionSyntax
        where T2 : ExpressionSyntax
        where T3 : ExpressionSyntax
    {
        ValidateArgumentCount(validator, 3);
        ValidateArgumentSyntax(validator, 0, argument1Validation);
        ValidateArgumentSyntax(validator, 1, argument2Validation);
        ValidateArgumentSyntax(validator, 2, argument3Validation);
        return validator;
    }

    public static ISyntaxNodeValidator<InvocationExpressionSyntax> HasArguments<T1, T2, T3, T4>(this ISyntaxNodeValidator<InvocationExpressionSyntax> validator, Action<ISyntaxNodeValidator<T1>>? argument1Validation = null, Action<ISyntaxNodeValidator<T2>>? argument2Validation = null, Action<ISyntaxNodeValidator<T3>>? argument3Validation = null, Action<ISyntaxNodeValidator<T4>>? argument4Validation = null)
        where T1 : ExpressionSyntax
        where T2 : ExpressionSyntax
        where T3 : ExpressionSyntax
        where T4 : ExpressionSyntax
    {
        ValidateArgumentCount(validator, 4);
        ValidateArgumentSyntax(validator, 0, argument1Validation);
        ValidateArgumentSyntax(validator, 1, argument2Validation);
        ValidateArgumentSyntax(validator, 2, argument3Validation);
        ValidateArgumentSyntax(validator, 3, argument4Validation);
        return validator;
    }

    private static void ValidateArgumentCount(ISyntaxNodeValidator<InvocationExpressionSyntax> validator, int expectedArgumentCount)
    {
        Assert.Instance.AreEqual(expectedArgumentCount, validator.SyntaxNode.ArgumentList.Arguments.Count, "Wrong number of argument for the InvocationExpressionSyntax.");
    }

    private static void ValidateArgumentSyntax<T>(ISyntaxNodeValidator<InvocationExpressionSyntax> validator, int argumentIndex, Action<ISyntaxNodeValidator<T>>? argumentValidation)
        where T : ExpressionSyntax
    {
        var argumentExpression = Assert.Instance.IsInstanceOfType<T>(validator.SyntaxNode.ArgumentList.Arguments[argumentIndex].Expression, $"The argument at index {argumentIndex} has the wrong expression type.");
        argumentValidation.TryInvoke(validator, argumentExpression);
    }
}

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rocketcress.SourceGenerators.Tests.Validators;

public static class AssignmentExpressionSyntaxValidations
{
    public static ISyntaxNodeValidator<AssignmentExpressionSyntax> LeftIs<T>(this ISyntaxNodeValidator<AssignmentExpressionSyntax> validator)
        where T : ExpressionSyntax
        => Is<T>(validator, validator.SyntaxNode.Left, null);
    public static ISyntaxNodeValidator<AssignmentExpressionSyntax> LeftIs<T>(this ISyntaxNodeValidator<AssignmentExpressionSyntax> validator, Action<ISyntaxNodeValidator<T>>? validation)
        where T : ExpressionSyntax
        => Is(validator, validator.SyntaxNode.Left, validation);

    public static ISyntaxNodeValidator<AssignmentExpressionSyntax> RightIs<T>(this ISyntaxNodeValidator<AssignmentExpressionSyntax> validator)
        where T : ExpressionSyntax
        => Is<T>(validator, validator.SyntaxNode.Right, null);
    public static ISyntaxNodeValidator<AssignmentExpressionSyntax> RightIs<T>(this ISyntaxNodeValidator<AssignmentExpressionSyntax> validator, Action<ISyntaxNodeValidator<T>>? validation)
        where T : ExpressionSyntax
        => Is(validator, validator.SyntaxNode.Right, validation);

    public static ISyntaxNodeValidator<AssignmentExpressionSyntax> LeftIsSymbol<T>(this ISyntaxNodeValidator<AssignmentExpressionSyntax> validator)
        where T : ISymbol
        => IsSymbol<T>(validator, validator.SyntaxNode.Left, null);
    public static ISyntaxNodeValidator<AssignmentExpressionSyntax> LeftIsSymbol<T>(this ISyntaxNodeValidator<AssignmentExpressionSyntax> validator, Action<ISymbolValidator<T>>? validation)
        where T : ISymbol
        => IsSymbol(validator, validator.SyntaxNode.Left, validation);

    public static ISyntaxNodeValidator<AssignmentExpressionSyntax> RightIsSymbol<T>(this ISyntaxNodeValidator<AssignmentExpressionSyntax> validator)
        where T : ISymbol
        => IsSymbol<T>(validator, validator.SyntaxNode.Right, null);
    public static ISyntaxNodeValidator<AssignmentExpressionSyntax> RightIsSymbol<T>(this ISyntaxNodeValidator<AssignmentExpressionSyntax> validator, Action<ISymbolValidator<T>>? validation)
        where T : ISymbol
        => IsSymbol(validator, validator.SyntaxNode.Right, validation);

    private static ISyntaxNodeValidator<AssignmentExpressionSyntax> Is<T>(ISyntaxNodeValidator<AssignmentExpressionSyntax> validator, ExpressionSyntax syntax, Action<ISyntaxNodeValidator<T>>? validation)
        where T : ExpressionSyntax
    {
        var castedSyntax = Assert.Instance.IsInstanceOfType<T>(syntax);
        if (validation is not null)
        {
            var leftValidator = new SyntaxNodeValidator<T>(castedSyntax, validator);
            validation(leftValidator);
        }

        return validator;
    }

    private static ISyntaxNodeValidator<AssignmentExpressionSyntax> IsSymbol<T>(ISyntaxNodeValidator<AssignmentExpressionSyntax> validator, ExpressionSyntax syntax, Action<ISymbolValidator<T>>? validation)
        where T : ISymbol
    {
        var symbol = validator.Compilation.Compilation.GetSemanticModel(syntax.SyntaxTree).GetSymbolInfo(syntax).Symbol;
        var castedSymbol = Assert.Instance.IsInstanceOfType<T>(symbol);
        if (validation is not null)
        {
            var leftValidator = new SymbolValidator<T>(castedSymbol, validator.Compilation);
            validation(leftValidator);
        }

        return validator;
    }
}

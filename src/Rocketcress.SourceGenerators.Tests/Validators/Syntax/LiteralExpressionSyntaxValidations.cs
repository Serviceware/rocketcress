using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rocketcress.SourceGenerators.Tests.Validators;

public static class LiteralExpressionSyntaxValidations
{
    public static ISyntaxNodeValidator<LiteralExpressionSyntax> IsEqualTo(this ISyntaxNodeValidator<LiteralExpressionSyntax> validator, string expectedValue)
    {
        Assert.Instance.AreEqual((int)SyntaxKind.StringLiteralToken, validator.SyntaxNode.Token.RawKind);
        Assert.Instance.AreEqual(expectedValue, validator.SyntaxNode.Token.ValueText);
        return validator;
    }
}

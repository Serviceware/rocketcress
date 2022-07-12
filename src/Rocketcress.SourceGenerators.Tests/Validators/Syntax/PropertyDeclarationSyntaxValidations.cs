using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocketcress.SourceGenerators.Tests.Extensions;

namespace Rocketcress.SourceGenerators.Tests.Validators;

public static class PropertyDeclarationSyntaxValidations
{
    public static ISyntaxNodeValidator<PropertyDeclarationSyntax> HasInitializer(this ISyntaxNodeValidator<PropertyDeclarationSyntax> validator)
        => HasInitializer(validator, null);

    public static ISyntaxNodeValidator<PropertyDeclarationSyntax> HasInitializer(this ISyntaxNodeValidator<PropertyDeclarationSyntax> validator, Action<ISyntaxNodeValidator<EqualsValueClauseSyntax>>? validation)
    {
        Assert.Instance.IsNotNull(validator.SyntaxNode.Initializer, "The property declaration does not have an initializer.");
        validation.TryInvoke(validator, validator.SyntaxNode.Initializer);
        return validator;
    }

    public static ISyntaxNodeValidator<PropertyDeclarationSyntax> HasExpressionBody(this ISyntaxNodeValidator<PropertyDeclarationSyntax> validator)
        => HasExpressionBody(validator, null);

    public static ISyntaxNodeValidator<PropertyDeclarationSyntax> HasExpressionBody(this ISyntaxNodeValidator<PropertyDeclarationSyntax> validator, Action<ISyntaxNodeValidator<ArrowExpressionClauseSyntax>>? validation)
    {
        Assert.Instance.IsNotNull(validator.SyntaxNode.ExpressionBody, "The property declaration does not have an expression body.");
        validation.TryInvoke(validator, validator.SyntaxNode.ExpressionBody);
        return validator;
    }
}

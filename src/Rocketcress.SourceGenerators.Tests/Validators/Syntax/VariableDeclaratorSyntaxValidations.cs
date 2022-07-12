using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocketcress.SourceGenerators.Tests.Extensions;

namespace Rocketcress.SourceGenerators.Tests.Validators;

public static class VariableDeclaratorSyntaxValidations
{
    public static ISyntaxNodeValidator<VariableDeclaratorSyntax> HasInitializer(this ISyntaxNodeValidator<VariableDeclaratorSyntax> validator)
        => HasInitializer(validator, null);

    public static ISyntaxNodeValidator<VariableDeclaratorSyntax> HasInitializer(this ISyntaxNodeValidator<VariableDeclaratorSyntax> validator, Action<ISyntaxNodeValidator<EqualsValueClauseSyntax>>? validation)
    {
        Assert.Instance.IsNotNull(validator.SyntaxNode.Initializer, "The variable declarator does not have an initializer.");
        validation.TryInvoke(validator, validator.SyntaxNode.Initializer);
        return validator;
    }
}

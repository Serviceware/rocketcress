using MaSch.Test.Assertion;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rocketcress.SourceGenerators.Tests.Validators
{
    public class MethodValidator : SymbolValidator<MethodValidator>
    {
        public MethodValidator(NamedTypeSymbolValidator parent, IMethodSymbol methodSymbol)
            : base(methodSymbol)
        {
            Parent = parent;
            MethodSymbol = methodSymbol;
        }

        public NamedTypeSymbolValidator Parent { get; }
        public IMethodSymbol MethodSymbol { get; }
        protected override MethodValidator This => this;

        public MethodValidator IsPartial()
        {
            bool isPartial = (from syntaxRef in MethodSymbol.DeclaringSyntaxReferences
                              let syntax = syntaxRef.GetSyntax()
                              where syntax is MethodDeclarationSyntax declarationSyntax
                                  && declarationSyntax.Modifiers.Any(SyntaxKind.PartialKeyword)
                              select syntax).Any();
            Assert.Instance.IsTrue(isPartial);
            return this;
        }
    }
}

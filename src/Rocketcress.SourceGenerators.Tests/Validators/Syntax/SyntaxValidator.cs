﻿using Microsoft.CodeAnalysis;
using Rocketcress.SourceGenerators.Tests.Extensions;

namespace Rocketcress.SourceGenerators.Tests.Validators;

public interface ISyntaxNodeValidator : IValidator
{
    new ISyntaxNodeValidator? Parent { get; }
    SyntaxNode SyntaxNode { get; }
}

public interface ISyntaxNodeValidator<T> : ISyntaxNodeValidator
    where T : SyntaxNode
{
    new T SyntaxNode { get; }

    ISyntaxNodeValidator<T> IsSymbol<TSymbol>(Action<ISymbolValidator<TSymbol>>? validation)
        where TSymbol : ISymbol;

    ISyntaxNodeValidator<T> Is<TSyntax>(Action<ISyntaxNodeValidator<TSyntax>>? validation)
        where TSyntax : T;
}

public class SyntaxNodeValidator<T> : ISyntaxNodeValidator<T>
    where T : SyntaxNode
{
    private readonly IValidator? _parent;

    public SyntaxNodeValidator(T syntaxNode, CompilationValidator compilation)
    {
        SyntaxNode = syntaxNode;
        Parent = null;
        _parent = null;
        Compilation = compilation;
    }

    public SyntaxNodeValidator(T syntaxNode, IValidator parent)
    {
        SyntaxNode = syntaxNode;
        if (parent is ISyntaxNodeValidator syntaxNodeValidator)
            Parent = syntaxNodeValidator;
        _parent = parent;
        Compilation = parent.Compilation;
    }

    public T SyntaxNode { get; }
    public ISyntaxNodeValidator? Parent { get; }
    public CompilationValidator Compilation { get; }

    SyntaxNode ISyntaxNodeValidator.SyntaxNode => SyntaxNode;
    IValidator? IValidator.Parent => _parent;

    public ISyntaxNodeValidator<T> IsSymbol<TSymbol>(Action<ISymbolValidator<TSymbol>>? validation)
        where TSymbol : ISymbol
    {
        var symbol = Compilation.Compilation.GetSemanticModel(SyntaxNode.SyntaxTree).GetSymbolInfo(SyntaxNode).Symbol;
        var castedSymbol = Assert.Instance.IsInstanceOfType<TSymbol>(symbol);

        validation.TryInvoke(this, castedSymbol);

        return this;
    }

    public ISyntaxNodeValidator<T> Is<TSyntax>(Action<ISyntaxNodeValidator<TSyntax>>? validation)
        where TSyntax : T
    {
        var castedSyntaxNode = Assert.Instance.IsInstanceOfType<TSyntax>(SyntaxNode);

        validation.TryInvoke(this, castedSyntaxNode);

        return this;
    }
}
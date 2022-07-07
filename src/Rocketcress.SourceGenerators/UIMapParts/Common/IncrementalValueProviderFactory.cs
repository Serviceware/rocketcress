using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Threading;

namespace Rocketcress.SourceGenerators.UIMapParts.Common;

internal readonly struct IncrementalValueProviderFactory
{
    private readonly IncrementalGeneratorInitializationContext _context;

    public IncrementalValueProviderFactory(IncrementalGeneratorInitializationContext context)
    {
        _context = context;
    }

    public static IncrementalValueProviderFactory From(IncrementalGeneratorInitializationContext context) => new(context);

    public IncrementalValueProvider<(Compilation Compilation, ImmutableArray<ClassDeclarationSyntax> ClassDeclarations)> GetClassDeclarationsWithGenerateUIMapPartsAttributeAndCorrectBaseType()
    {
        var classDeclarations = _context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: IsSyntaxTargetForGeneration,
                transform: GetSemanticTargetForGeneration)
            .Where(static c => c is not null);

        return _context.CompilationProvider.Combine(classDeclarations.Collect())!;
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode node, CancellationToken cancellationToken)
    {
        return node is ClassDeclarationSyntax c && c.AttributeLists.Count > 0;
    }

    private static ClassDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;
        var validator = ClassDeclarationValidator.Validate(classDeclarationSyntax, context.SemanticModel);

        if (!validator.HasGenerateUIMapPartsAttribute())
            return null;

        return classDeclarationSyntax;
    }
}

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rocketcress.SourceGenerators.Common;
using Rocketcress.SourceGenerators.UIMapParts.Models;

namespace Rocketcress.SourceGenerators.UIMapParts.Generation;

internal readonly struct FileGenerator
{
    private const string GeneratorName = "UIMapPartsGenerator";

    private readonly UIMapPartsGeneratorContext _context;

    private FileGenerator(UIMapPartsGeneratorContext context)
    {
        _context = context;
    }

    public static FileGenerator Generate(UIMapPartsGeneratorContext context) => new(context);

    public void UIMapParts(GeneratorData data)
    {
        var sourceBuilder = new SourceBuilder();
        AddUsings(sourceBuilder, _context.TypeSymbol);
        ClassGenerator.Generate(_context, sourceBuilder).UIMapParts(data);
        _context.AddSource(CodeGenerationHelper.CreateHintName(_context.TypeSymbol, GeneratorName), sourceBuilder.ToString());
    }

    private static void AddUsings(SourceBuilder builder, INamedTypeSymbol typeSymbol)
    {
        bool hasUsing = false;
        SyntaxNode? currentSyntax = typeSymbol.DeclaringSyntaxReferences[0].GetSyntax();
        while (currentSyntax != null)
        {
            SyntaxList<UsingDirectiveSyntax>? u = null;
            if (currentSyntax is CompilationUnitSyntax cus)
                u = cus.Usings;
            else if (currentSyntax is NamespaceDeclarationSyntax nds)
                u = nds.Usings;

            if (u.HasValue)
            {
                if (u.Value.Count > 0)
                    hasUsing = true;
                foreach (var @using in u.Value)
                    builder.AppendLine(@using.GetText().ToString().Trim());
            }

            currentSyntax = currentSyntax.Parent;
        }

        if (hasUsing)
            builder.AppendLine();
    }
}

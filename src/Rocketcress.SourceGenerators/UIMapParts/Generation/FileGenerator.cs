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
        ClassGenerator.Generate(_context, sourceBuilder).UIMapParts(data);
        _context.AddSource(CodeGenerationHelper.CreateHintName(_context.TypeSymbol, GeneratorName), sourceBuilder.ToString());
    }
}

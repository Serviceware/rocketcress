using Rocketcress.SourceGenerators.Common;
using Rocketcress.SourceGenerators.UIMapParts.Models;

namespace Rocketcress.SourceGenerators.UIMapParts.Generation
{
    internal readonly struct FileGenerator
    {
        private readonly UIMapPartsGeneratorContext _context;

        private FileGenerator(UIMapPartsGeneratorContext context)
        {
            _context = context;
        }

        public static FileGenerator Generate(UIMapPartsGeneratorContext context) => new(context);

        public void UIMapParts(ControlDefinition[] controls)
        {
            var sourceBuilder = new SourceBuilder();
            ClassGenerator.Generate(sourceBuilder).UIMapParts();
            _context.AddSource(CodeGenerationHelper.CreateHintName(_context.TypeSymbol, nameof(UIMapPartsGenerator)), sourceBuilder.ToString());
        }
    }
}

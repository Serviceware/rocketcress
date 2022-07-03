using Microsoft.CodeAnalysis;
using Rocketcress.SourceGenerators.Common;
using Rocketcress.SourceGenerators.Extensions;
using Rocketcress.SourceGenerators.UIMapParts.Models;

namespace Rocketcress.SourceGenerators.UIMapParts.Generation
{
    internal readonly struct ConstructorGenerator
    {
        private readonly UIMapPartsGeneratorContext _context;
        private readonly SourceBuilder _builder;

        private ConstructorGenerator(UIMapPartsGeneratorContext context, SourceBuilder builder)
        {
            _context = context;
            _builder = builder;
        }

        public static ConstructorGenerator Generate(UIMapPartsGeneratorContext context, SourceBuilder builder) => new(context, builder);

        public void DefaultConstructors(IMethodSymbol[] constructors)
        {
            foreach (var constructor in constructors)
            {
                var accessibility = constructor.DeclaredAccessibility == Accessibility.Public ? "public" : "protected";
                var paramDef = string.Join(", ", constructor.Parameters.Select(y => y.ToDefinitionString()));
                var paramUsg = string.Join(", ", constructor.Parameters.Select(y => y.ToUsageString()));
                _builder.AppendLine($"{accessibility} {_context.TypeSymbol.Name}({paramDef}) : base({paramUsg}) {{ }}");
            }
        }
    }
}

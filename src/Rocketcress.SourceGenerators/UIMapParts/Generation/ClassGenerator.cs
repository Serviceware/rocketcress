using Microsoft.CodeAnalysis;
using Rocketcress.SourceGenerators.Common;
using Rocketcress.SourceGenerators.Extensions;
using Rocketcress.SourceGenerators.UIMapParts.Models;

namespace Rocketcress.SourceGenerators.UIMapParts.Generation
{
    internal readonly struct ClassGenerator
    {
        private readonly UIMapPartsGeneratorContext _context;
        private readonly SourceBuilder _builder;

        private ClassGenerator(UIMapPartsGeneratorContext context, SourceBuilder builder)
        {
            _context = context;
            _builder = builder;
        }

        public static ClassGenerator Generate(UIMapPartsGeneratorContext context, SourceBuilder builder) => new(context, builder);

        public void UIMapParts(GeneratorData data)
        {
            using (_builder.AddBlock($"namespace {_context.TypeSymbol.ContainingNamespace}"))
            using (AddPartialClasses())
            {
                FieldGenerator.Generate(_context, _builder).LocationKeys(data.Controls);

                _builder.AppendLineBeforeNextAppend();
                ConstructorGenerator.Generate(_context, _builder).DefaultConstructors(data.ConstructorsToGenerate);

                _builder.AppendLineBeforeNextAppend();
                MethodGenerator.Generate(_context, _builder).InitializeOverride(data.Controls);

                _builder.AppendLineBeforeNextAppend();
                MethodGenerator.Generate(_context, _builder).PartialInitialize(data.Controls);

                _builder.AppendLineBeforeNextAppend();
                MethodGenerator.Generate(_context, _builder).InitUsing();
            }
        }

        private DisposableStack AddPartialClasses()
        {
            var typeStack = new Stack<INamedTypeSymbol>();
            var current = _context.TypeSymbol;
            while (current != null)
            {
                typeStack.Push(current);
                current = current.ContainingType;
            }

            var blockStack = new DisposableStack();
            while (typeStack.Count > 0)
            {
                blockStack.Push(_builder.AddBlock($"partial class {typeStack.Pop().ToTypeDefinitionString()}"));
            }

            return blockStack;
        }

        private class DisposableStack : Stack<IDisposable>, IDisposable
        {
            public void Dispose()
            {
                while (Count > 0)
                    Pop().Dispose();
            }
        }
    }
}

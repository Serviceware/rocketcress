using MaSch.Test.Assertion;
using Microsoft.CodeAnalysis;

namespace Rocketcress.SourceGenerators.Tests.Validators
{
    public class CompilationValidator : DiagnosticsValidator<CompilationValidator>
    {
        public CompilationValidator(Compilation compilation, Diagnostic[] diagnostics)
            : base(diagnostics)
        {
            Compilation = compilation;
        }

        public Compilation Compilation { get; }
        protected override CompilationValidator This => this;

        public CompilationValidator HasType(string fullTypeName)
            => HasType(fullTypeName, out _);

        public CompilationValidator HasType(string fullTypeName, out INamedTypeSymbol typeSymbol)
        {
            var type = Compilation.GetTypeByMetadataName(fullTypeName);
            Assert.Instance.IsNotNull(type, $"The type \"{fullTypeName}\" does not exists in the compilation.");
            typeSymbol = type;

            return this;
        }

        public CompilationValidator DoesNotHaveType(string fullTypeName)
        {
            var typeSymbol = Compilation.GetTypeByMetadataName(fullTypeName);
            Assert.Instance.IsNull(typeSymbol, $"The type \"{fullTypeName}\" exists unexpectedly in the compilation.");

            return this;
        }

        public CompilationValidator ValidateType(INamedTypeSymbol typeSymbol, Action<ISymbolValidator<INamedTypeSymbol>> typeValidation)
        {
            var typeValidator = new SymbolValidator<INamedTypeSymbol>(typeSymbol, this);
            typeValidation(typeValidator);

            return this;
        }

        public INamedTypeSymbol GetTypeSymbolFromType(Type type)
        {
            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                HasType(genericType.FullName!, out var genericTypeSymbol);
                return genericTypeSymbol.Construct(type.GetGenericArguments().Select(GetTypeSymbolFromType).ToArray());
            }
            else
            {
                HasType(type.FullName!, out var typeSymbol);
                return typeSymbol;
            }
        }

        [return: NotNullIfNotNull("types")]
        public INamedTypeSymbol[]? GetTypeSymbols(params object[]? types)
        {
            if (types is null)
                return null;

            return (from objType in types
                    select objType switch
                    {
                        INamedTypeSymbol typeSymbol => typeSymbol,
                        Type type => GetTypeSymbolFromType(type),
                        _ => throw new InvalidOperationException($"Type \"{objType.GetType().FullName}\" is not allowed."),
                    }).ToArray();
        }

        [return: NotNullIfNotNull("types")]
        public INamedTypeSymbol[]? GetTypeSymbols(params Type[]? types)
        {
            if (types is null)
                return null;

            return types.Select(x => GetTypeSymbolFromType(x)).ToArray();
        }
    }
}

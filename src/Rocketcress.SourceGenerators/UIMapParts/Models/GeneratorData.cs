using Microsoft.CodeAnalysis;
using Rocketcress.SourceGenerators.Extensions;

namespace Rocketcress.SourceGenerators.UIMapParts.Models
{
    internal record GeneratorData(
        GenerateUIMapPartsOptions ClassOptions,
        ControlDefinition[] Controls,
        IMethodSymbol[] ConstructorsToGenerate)
    {
        public static GeneratorData Create(UIMapPartsGeneratorContext context)
        {
            var classOptions = GenerateUIMapPartsOptions.Get(context);
            var controls = ControlDefinition.GetAll(context, classOptions);
            var constructors = classOptions.GenerateDefaultConstructors ? GetConstructorsToGenerate(context) : Array.Empty<IMethodSymbol>();

            return new GeneratorData(classOptions, controls, constructors);
        }

        [SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1024:Symbols should be compared for equality", Justification = "I am using SymbolEqualityComparer in ConstructorEqualityComparer.")]
        private static IMethodSymbol[] GetConstructorsToGenerate(UIMapPartsGeneratorContext context)
        {
            if (context.TypeSymbol.BaseType is null)
                return Array.Empty<IMethodSymbol>();

            var set = new HashSet<IMethodSymbol>(
                GetConstructors(context.TypeSymbols, context.TypeSymbol, false),
                ConstructorEqualityComparer.Instance);

            var result = new List<IMethodSymbol>();
            foreach (var constructor in GetConstructors(context.TypeSymbols, context.TypeSymbol.BaseType, true))
            {
                if (set.Add(constructor))
                    result.Add(constructor);
            }

            return result.ToArray();
        }

        private static IEnumerable<IMethodSymbol> GetConstructors(TypeSymbols typeSymbols, INamedTypeSymbol typeSymbol, bool includeBaseType)
        {
            foreach (var constructor in typeSymbol.InstanceConstructors)
            {
                if (!constructor.IsImplicitlyDeclared &&
                    (constructor.DeclaredAccessibility == Accessibility.Public ||
                     constructor.DeclaredAccessibility == Accessibility.Protected))
                {
                    yield return constructor;
                }
            }

            var baseType = typeSymbol.BaseType;
            if (baseType is not null && GenerateUIMapPartsOptions.GetGenerateDefaultConstructors(typeSymbols, baseType))
            {
                foreach (var constructor in GetConstructors(typeSymbols, baseType, true))
                    yield return constructor;
            }
        }

        private readonly struct ConstructorEqualityComparer : IEqualityComparer<IMethodSymbol>
        {
            public static ConstructorEqualityComparer Instance { get; }

            public bool Equals(IMethodSymbol x, IMethodSymbol y)
            {
                if (SymbolEqualityComparer.Default.Equals(x, y))
                    return true;
                if (x is null || y is null)
                    return false;
                if (x.Parameters.Length != y.Parameters.Length)
                    return false;

                for (int i = 0; i < x.Parameters.Length; i++)
                {
                    if (!SymbolEqualityComparer.Default.Equals(x.Parameters[i].Type, y.Parameters[i].Type))
                        return false;
                }

                return true;
            }

            public int GetHashCode(IMethodSymbol obj)
            {
                unchecked
                {
                    if (obj?.Parameters is null)
                        return 0;
                    int hash = 17;
                    foreach (var param in obj.Parameters)
                        hash = (hash * 31) + SymbolEqualityComparer.Default.GetHashCode(param.Type);
                    return hash;
                }
            }
        }
    }
}

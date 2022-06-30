using MaSch.Test.Assertion;
using Microsoft.CodeAnalysis;

namespace Rocketcress.SourceGenerators.Tests.Validators
{
    public class NamedTypeSymbolValidator : SymbolValidator<NamedTypeSymbolValidator>
    {
        public NamedTypeSymbolValidator(CompilationValidator compilation, INamedTypeSymbol typeSymbol)
            : base(typeSymbol)
        {
            Compilation = compilation;
            TypeSymbol = typeSymbol;
        }

        public CompilationValidator Compilation { get; }
        public INamedTypeSymbol TypeSymbol { get; }
        public string TypeName => TypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        protected override NamedTypeSymbolValidator This => this;

        public NamedTypeSymbolValidator HasProperty(string propertyName)
            => HasProperty(propertyName, null);

        public NamedTypeSymbolValidator HasProperty(string propertyName, Action<PropertyValidator>? propertyValidation)
        {
            var property = TypeSymbol.GetMembers(propertyName).OfType<IPropertySymbol>().FirstOrDefault();
            Assert.Instance.IsNotNull(property, $"Property \"{propertyName}\" does not exist in type \"{TypeName}\".");

            if (propertyValidation is not null)
            {
                var validator = new PropertyValidator(this, property);
                propertyValidation(validator);
            }

            return this;
        }

        public NamedTypeSymbolValidator DoesNotHaveProperty(string propertyName)
        {
            var property = TypeSymbol.GetMembers(propertyName).OfType<IPropertySymbol>().FirstOrDefault();
            Assert.Instance.IsNull(property, $"Property \"{propertyName}\" exist unexpectedly in type \"{TypeName}\".");
            return this;
        }

        public NamedTypeSymbolValidator HasMethod(string methodName)
            => HasMethod(out _, methodName, MethodKind.Ordinary, (ITypeSymbol[]?)null);

        public NamedTypeSymbolValidator HasMethod(string methodName, params Type[]? parameterTypes)
            => HasMethod(out _, methodName, MethodKind.Ordinary, GetTypeSymbols(parameterTypes));

        public NamedTypeSymbolValidator HasMethod(string methodName, params object[]? parameterTypes)
            => HasMethod(out _, methodName, MethodKind.Ordinary, GetTypeSymbols(parameterTypes));

        public NamedTypeSymbolValidator HasMethod(string methodName, params ITypeSymbol[]? parameterTypeSymbols)
            => HasMethod(out _, methodName, MethodKind.Ordinary, parameterTypeSymbols);

        public NamedTypeSymbolValidator HasMethod(string methodName, MethodKind kind)
            => HasMethod(out _, methodName, kind, (ITypeSymbol[]?)null);

        public NamedTypeSymbolValidator HasMethod(string methodName, MethodKind kind, params Type[]? parameterTypes)
            => HasMethod(out _, methodName, kind, GetTypeSymbols(parameterTypes));

        public NamedTypeSymbolValidator HasMethod(string methodName, MethodKind kind, params object[]? parameterTypes)
            => HasMethod(out _, methodName, kind, GetTypeSymbols(parameterTypes));

        public NamedTypeSymbolValidator HasMethod(string methodName, MethodKind kind, params ITypeSymbol[]? parameterTypeSymbols)
            => HasMethod(out _, methodName, kind, parameterTypeSymbols);

        public NamedTypeSymbolValidator HasMethod(out IMethodSymbol methodSymbol, string methodName)
            => HasMethod(out methodSymbol, methodName, MethodKind.Ordinary, (ITypeSymbol[]?)null);

        public NamedTypeSymbolValidator HasMethod(out IMethodSymbol methodSymbol, string methodName, params Type[]? parameterTypes)
            => HasMethod(out methodSymbol, methodName, MethodKind.Ordinary, GetTypeSymbols(parameterTypes));

        public NamedTypeSymbolValidator HasMethod(out IMethodSymbol methodSymbol, string methodName, params object[]? parameterTypes)
            => HasMethod(out methodSymbol, methodName, MethodKind.Ordinary, GetTypeSymbols(parameterTypes));

        public NamedTypeSymbolValidator HasMethod(out IMethodSymbol methodSymbol, string methodName, params ITypeSymbol[]? parameterTypeSymbols)
            => HasMethod(out methodSymbol, methodName, MethodKind.Ordinary, parameterTypeSymbols);

        public NamedTypeSymbolValidator HasMethod(out IMethodSymbol methodSymbol, string methodName, MethodKind kind)
            => HasMethod(out methodSymbol, methodName, kind, (ITypeSymbol[]?)null);

        public NamedTypeSymbolValidator HasMethod(out IMethodSymbol methodSymbol, string methodName, MethodKind kind, params Type[]? parameterTypes)
            => HasMethod(out methodSymbol, methodName, kind, GetTypeSymbols(parameterTypes));

        public NamedTypeSymbolValidator HasMethod(out IMethodSymbol methodSymbol, string methodName, MethodKind kind, params object[]? parameterTypes)
            => HasMethod(out methodSymbol, methodName, kind, GetTypeSymbols(parameterTypes));

        public NamedTypeSymbolValidator HasMethod(out IMethodSymbol methodSymbol, string methodName, MethodKind kind, params ITypeSymbol[]? parameterTypeSymbols)
        {
            var method = (from m in TypeSymbol.GetMembers(methodName).OfType<IMethodSymbol>()
                          where m.MethodKind == kind && (
                              parameterTypeSymbols == null ||
                              m.Parameters.Select(x => x.Type).SequenceEqual(parameterTypeSymbols, SymbolEqualityComparer.Default))
                          select m).FirstOrDefault();
            Assert.Instance.IsNotNull(method, GetErrorMessage());
            methodSymbol = method;

            return this;

            string GetErrorMessage()
            {
                return parameterTypeSymbols is null
                    ? $"Method \"{methodName}\" of kind {kind} does not exist in type \"{TypeName}\"."
                    : $"Method \"{methodName}\" of kind {kind} with {parameterTypeSymbols.Length} parameter(s) ({string.Join(", ", parameterTypeSymbols.Select(x => x.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)))}) does not exist in type \"{TypeName}\".";
            }
        }

        public NamedTypeSymbolValidator HasConstructor(params Type[] parameterTypes)
            => HasConstructor(out _, parameterTypes);

        public NamedTypeSymbolValidator HasConstructor(out IMethodSymbol methodSymbol, params Type[] parameterTypes)
            => HasConstructor(out methodSymbol, GetTypeSymbols(parameterTypes));

        public NamedTypeSymbolValidator HasConstructor(params object[] parameterTypes)
            => HasConstructor(out _, parameterTypes);

        public NamedTypeSymbolValidator HasConstructor(out IMethodSymbol methodSymbol, params object[] parameterTypes)
            => HasConstructor(out methodSymbol, GetTypeSymbols(parameterTypes));

        public NamedTypeSymbolValidator HasConstructor(params ITypeSymbol[] parameterTypeSymbols)
            => HasConstructor(out _, parameterTypeSymbols);

        public NamedTypeSymbolValidator HasConstructor(out IMethodSymbol methodSymbol, params ITypeSymbol[] parameterTypeSymbols)
        {
            var constructor = (from ctor in TypeSymbol.InstanceConstructors
                               where ctor.Parameters.Select(x => x.Type).SequenceEqual(parameterTypeSymbols, SymbolEqualityComparer.Default)
                               select ctor).FirstOrDefault();
            Assert.Instance.IsNotNull(constructor, $"A construcotr with {parameterTypeSymbols.Length} parameter(s) ({string.Join(", ", parameterTypeSymbols.Select(x => x.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)))}) was not found on type \"{TypeName}\".");
            methodSymbol = constructor;
            return this;
        }

        public NamedTypeSymbolValidator HasMembers(SymbolKind kind, int count)
        {
            var propertyCount = TypeSymbol.GetMembers().Where(x => x.Kind == kind).Count();
            Assert.Instance.AreEqual(count, propertyCount, $"The type \"{TypeName}\" has the wrong number of members of kind {kind}.");
            return this;
        }

        public NamedTypeSymbolValidator HasMembers(MethodKind kind, int count)
        {
            var propertyCount = TypeSymbol.GetMembers().OfType<IMethodSymbol>().Where(x => x.MethodKind == kind).Count();
            Assert.Instance.AreEqual(count, propertyCount, $"The type \"{TypeName}\" has the wrong number of methods of kind {kind}.");
            return this;
        }

        public NamedTypeSymbolValidator HasMembers(SymbolKind kind, Accessibility accessibility, int count)
        {
            var propertyCount = TypeSymbol.GetMembers().Where(x => x.Kind == kind && x.DeclaredAccessibility == accessibility).Count();
            Assert.Instance.AreEqual(count, propertyCount, $"The type \"{TypeName}\" has the wrong number of {accessibility} members of kind {kind}.");
            return this;
        }

        public NamedTypeSymbolValidator HasMembers(MethodKind kind, Accessibility accessibility, int count)
        {
            var propertyCount = TypeSymbol.GetMembers().OfType<IMethodSymbol>().Where(x => x.MethodKind == kind && x.DeclaredAccessibility == accessibility).Count();
            Assert.Instance.AreEqual(count, propertyCount, $"The type \"{TypeName}\" has the wrong number of {accessibility} methods of kind {kind}.");
            return this;
        }

        public NamedTypeSymbolValidator ValidateMethod(IMethodSymbol methodSymbol, Action<MethodValidator> methodValidation)
        {
            var validator = new MethodValidator(this, methodSymbol);
            methodValidation(validator);
            return this;
        }

        [return: NotNullIfNotNull("types")]
        private INamedTypeSymbol[]? GetTypeSymbols(object[]? types)
        {
            if (types is null)
                return null;

            return (from objType in types
                    select objType switch
                    {
                        INamedTypeSymbol typeSymbol => typeSymbol,
                        Type type => Compilation.GetTypeSymbolFromType(type),
                        _ => throw new InvalidOperationException($"Type \"{objType.GetType().FullName}\" is not allowed."),
                    }).ToArray();
        }

        [return: NotNullIfNotNull("types")]
        private INamedTypeSymbol[]? GetTypeSymbols(Type[]? types)
        {
            if (types is null)
                return null;

            return types.Select(x => Compilation.GetTypeSymbolFromType(x)).ToArray();
        }
    }
}

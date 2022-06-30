using MaSch.Test.Assertion;
using Microsoft.CodeAnalysis;

namespace Rocketcress.SourceGenerators.Tests.Validators
{
    public class PropertyValidator : SymbolValidator<PropertyValidator>
    {
        public PropertyValidator(NamedTypeSymbolValidator parent, IPropertySymbol propertySymbol)
            : base(propertySymbol)
        {
            Parent = parent;
            PropertySymbol = propertySymbol;
        }

        public NamedTypeSymbolValidator Parent { get; }
        public IPropertySymbol PropertySymbol { get; }
        protected override PropertyValidator This => this;

        public PropertyValidator IsOfType<T>()
            => IsOfType(typeof(T));

        public PropertyValidator IsOfType(Type type)
            => IsOfType(Parent.Compilation.GetTypeSymbolFromType(type));

        public PropertyValidator IsOfType(ITypeSymbol typeSymbol)
        {
            Assert.Instance.AreEqual(typeSymbol, PropertySymbol.Type, $"The type of property \"{PropertySymbol.Name}\" in type \"{Parent.TypeName}\" is not correct.");
            return this;
        }
    }
}

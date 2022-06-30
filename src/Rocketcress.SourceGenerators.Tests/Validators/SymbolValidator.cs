using MaSch.Test.Assertion;
using Microsoft.CodeAnalysis;

namespace Rocketcress.SourceGenerators.Tests.Validators
{
    public abstract class SymbolValidator<T>
        where T : SymbolValidator<T>
    {
        private readonly ISymbol _symbol;

        public SymbolValidator(ISymbol symbol)
        {
            _symbol = symbol;
        }

        protected abstract T This { get; }

        public T HasAccessibility(Accessibility expectedAccessibility)
        {
            Assert.Instance.AreEqual(expectedAccessibility, _symbol.DeclaredAccessibility, $"Wrong accessiblity for symbol \"{_symbol}\".");
            return This;
        }

        public T IsStatic()
        {
            Assert.Instance.IsTrue(_symbol.IsStatic);
            return This;
        }

        public T IsNotStatic()
        {
            Assert.Instance.IsFalse(_symbol.IsStatic);
            return This;
        }
    }
}

using Microsoft.CodeAnalysis;
using Rocketcress.SourceGenerators.UIMapParts.Common;

namespace Rocketcress.SourceGenerators.UIMapParts.Models
{
    internal abstract class UIMapPartsGeneratorContext
    {
        private readonly Action<Diagnostic> _reportDiagnostic;
        private readonly Action<string, string> _addSource;

        public UIMapPartsGeneratorContext(
            Compilation compilation,
            TypeSymbols typeSymbols,
            UITestTypeSymbols uitestTypeSymbols,
            INamedTypeSymbol typeSymbol,
            Action<Diagnostic> reportDiagnostic,
            Action<string, string> addSource)
        {
            Compilation = compilation;
            TypeSymbols = typeSymbols;
            UITestTypeSymbols = uitestTypeSymbols;
            TypeSymbol = typeSymbol;
            _reportDiagnostic = reportDiagnostic;
            _addSource = addSource;
        }

        public abstract string IdLocationKeyFormat { get; }
        public abstract string EmptyLocationKeyFormat { get; }
        public abstract string ControlInitFirstParamName { get; }

        public Compilation Compilation { get; }
        public TypeSymbols TypeSymbols { get; }
        public UITestTypeSymbols UITestTypeSymbols { get; }
        public INamedTypeSymbol TypeSymbol { get; }

        public void ReportDiagnostic(Diagnostic diagnostic) => _reportDiagnostic(diagnostic);
        public void AddSource(string hintName, string sourceText) => _addSource(hintName, sourceText);
    }
}

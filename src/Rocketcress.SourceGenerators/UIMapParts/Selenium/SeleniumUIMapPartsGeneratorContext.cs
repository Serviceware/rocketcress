using Microsoft.CodeAnalysis;
using Rocketcress.SourceGenerators.UIMapParts.Common;
using Rocketcress.SourceGenerators.UIMapParts.Models;

namespace Rocketcress.SourceGenerators.UIMapParts.Selenium;

internal class SeleniumUIMapPartsGeneratorContext : UIMapPartsGeneratorContext
{
    public SeleniumUIMapPartsGeneratorContext(
        Compilation compilation,
        TypeSymbols typeSymbols,
        SeleniumTypeSymbols seleniumTypeSymbols,
        INamedTypeSymbol typeSymbol,
        Action<Diagnostic> reportDiagnostic,
        Action<string, string> addSource)
        : base(compilation, typeSymbols, seleniumTypeSymbols, typeSymbol, reportDiagnostic, addSource)
    {
        UITestTypeSymbols = seleniumTypeSymbols;
    }

    public new SeleniumTypeSymbols UITestTypeSymbols { get; }

    public override string IdLocationKeyFormat { get; } = "{0}.Id(\"{1}\")";
    public override string EmptyLocationKeyFormat { get; } = "{0}.XPath(\".//*\")";
    public override string ControlInitFirstParamName { get; } = "this.Driver";
}

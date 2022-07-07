using Microsoft.CodeAnalysis;
using Rocketcress.SourceGenerators.UIMapParts.Common;
using Rocketcress.SourceGenerators.UIMapParts.Models;

namespace Rocketcress.SourceGenerators.UIMapParts.UIAutomation
{
    internal class UIAutomationUIMapPartsGeneratorContext : UIMapPartsGeneratorContext
    {
        public UIAutomationUIMapPartsGeneratorContext(
            Compilation compilation,
            TypeSymbols typeSymbols,
            UIAutomationTypeSymbols uiautomationTypeSymbols,
            INamedTypeSymbol typeSymbol,
            Action<Diagnostic> reportDiagnostic,
            Action<string, string> addSource)
            : base(compilation, typeSymbols, uiautomationTypeSymbols, typeSymbol, reportDiagnostic, addSource)
        {
            UITestTypeSymbols = uiautomationTypeSymbols;
        }

        public new UIAutomationTypeSymbols UITestTypeSymbols { get; }

        public override string IdLocationKeyFormat { get; } = "{0}.AutomationId(\"{1}\")";
        public override string EmptyLocationKeyFormat { get; } = "{0}.Empty";
        public override string ControlInitFirstParamName { get; } = "this.Application";
    }
}

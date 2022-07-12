extern alias rccore;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Rocketcress.SourceGenerators.Tests.Validators;
using System.Collections.Immutable;
using System.Text;

namespace Rocketcress.SourceGenerators.Tests.Runner
{
    public static class AnalyzerTestRunner
    {
        public static CompilationValidator CompileAndGenerate(string source, params IIncrementalGenerator[] generators)
        {
            return CompileAndGenerate(source, generators.Select(GeneratorExtensions.AsSourceGenerator).ToArray());
        }

        public static CompilationValidator CompileAndGenerate(string source, params ISourceGenerator[] generators)
        {
            // Load assemblies into AppDomain, so they are added to compilation
            _ = typeof(Rocketcress.Selenium.WebDriver).Assembly;
            _ = typeof(rccore::Rocketcress.Core.Wait).Assembly;
            _ = typeof(Rocketcress.UIAutomation.Application).Assembly;

            var compilation = CreateCompilation(source);
            var newCompilation = RunGenerators(compilation, out var diagnostics, generators);

            return new CompilationValidator(newCompilation, newCompilation.GetDiagnostics().ToArray());
        }

        public static DiagnosticsValidator CompileAndAnalyze(string source, params DiagnosticAnalyzer[] analyzers)
        {
            var diagnostics = CreateCompilation(source)
                .WithAnalyzers(ImmutableArray.Create(analyzers))
                .GetAnalyzerDiagnosticsAsync()
                .Result;
            LogDiagnostics(diagnostics);
            return new DiagnosticsValidator(diagnostics.ToArray());
        }

        private static Compilation CreateCompilation(string source)
            => CSharpCompilation.Create(
                "compilation",
                new[]
                {
                    CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Latest)),
                },
                AppDomain.CurrentDomain.GetAssemblies()
                    .Where(x => !x.IsDynamic && !string.IsNullOrWhiteSpace(x.Location) && x.GetName().Name != "Rocketcress.SourceGenerators").Select(x => MetadataReference.CreateFromFile(x.Location)).ToArray(),
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        private static GeneratorDriver CreateDriver(Compilation c, params ISourceGenerator[] generators)
            => CSharpGeneratorDriver.Create(
                generators,
                parseOptions: c.SyntaxTrees.First().Options as CSharpParseOptions,
                additionalTexts: ImmutableArray<AdditionalText>.Empty);

        private static Compilation RunGenerators(Compilation compilation, out ImmutableArray<Diagnostic> diagnostics, params ISourceGenerator[] generators)
        {
            var driver = CreateDriver(compilation, generators).RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out diagnostics);

            LogDiagnostics(driver.GetRunResult().Diagnostics);
            Trace.WriteLine(string.Empty);

            var genLog = new StringBuilder().AppendLine("Generated Code:");
            foreach (var result in driver.GetRunResult().Results.SelectMany(x => x.GeneratedSources))
            {
                genLog.AppendLine(result.HintName);
                genLog.AppendLine(result.SourceText.ToString());
            }

            Trace.WriteLine(genLog.ToString());

            return outputCompilation;
        }

        private static void LogDiagnostics(IEnumerable<Diagnostic> diagnostics)
        {
            Trace.WriteLine($"Diagnostics:\n{string.Join("\n", diagnostics.Select(x => x.ToString()))}");
        }
    }
}

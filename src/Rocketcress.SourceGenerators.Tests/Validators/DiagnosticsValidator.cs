using Microsoft.CodeAnalysis;

namespace Rocketcress.SourceGenerators.Tests.Validators
{
    public class DiagnosticsValidator : DiagnosticsValidator<DiagnosticsValidator>
    {
        public DiagnosticsValidator(Diagnostic[] diagnostics)
            : base(diagnostics)
        {
        }

        protected override DiagnosticsValidator This => this;
    }

    public abstract class DiagnosticsValidator<T>
        where T : DiagnosticsValidator<T>
    {
        public DiagnosticsValidator(Diagnostic[] diagnostics)
        {
            Diagnostics = diagnostics;
        }

        public Diagnostic[] Diagnostics { get; }
        protected abstract T This { get; }

        public T HasNoErrors()
        {
            var errors = Diagnostics.Where(x => x.Severity == DiagnosticSeverity.Error).ToArray();
            Assert.Instance.AreEqual(0, errors.Length, $"There were errors during compilation:\n{string.Join("\n", errors.Select(x => x.ToString()))}");
            return This;
        }

        public T HasNoWarnings()
        {
            var errors = Diagnostics.Where(x => x.Severity == DiagnosticSeverity.Warning).ToArray();
            Assert.Instance.AreEqual(0, errors.Length, $"There were warnings during compilation:\n{string.Join("\n", errors.Select(x => x.ToString()))}");
            return This;
        }

        public T HasNoWarningsOrErrors()
        {
            var errors = Diagnostics.Where(x => x.Severity == DiagnosticSeverity.Warning || x.Severity == DiagnosticSeverity.Error).ToArray();
            Assert.Instance.AreEqual(0, errors.Length, $"There were warnings and/or errors during compilation:\n{string.Join("\n", errors.Select(x => x.ToString()))}");
            return This;
        }

        public T HasError(string id)
            => HasError(id, -1);

        public T HasError(string id, int count)
            => HasDiagnostic(DiagnosticSeverity.Error, id, count);

        public T HasWarning(string id)
            => HasWarning(id, -1);

        public T HasWarning(string id, int count)
            => HasDiagnostic(DiagnosticSeverity.Warning, id, count);

        public T HasDiagnostic(DiagnosticSeverity severity, string id, int count)
        {
            var errors = Diagnostics.Where(x => x.Severity == severity && x.Id == id).ToArray();
            if (count >= 0)
                Assert.Instance.AreEqual(count, errors.Length, $"Wrong number of {id} {severity}s.");
            else
                Assert.Instance.IsGreaterThan(0, errors.Length, $"At least one {severity} with id {id} was expected, but got none.");

            return This;
        }
    }
}

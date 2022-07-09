namespace Rocketcress.SourceGenerators.Tests.Validators
{
    public interface IValidator
    {
        IValidator? Parent { get; }
        CompilationValidator Compilation { get; }
    }
}

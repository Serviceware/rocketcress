namespace Rocketcress.SourceGenerators.Tests.Validators;

public interface IValidator
{
    IValidator? Parent { get; }
    CompilationValidator Compilation { get; }
}

public static class ValidatorExtensions
{
    public static T Conditional<T>(this T validator, Func<T, bool> condition, Action<T> validation)
        where T : IValidator
        => Conditional(validator, condition(validator), validation);

    public static T Conditional<T>(this T validator, bool condition, Action<T> validation)
        where T : IValidator
    {
        if (condition)
            validation(validator);
        return validator;
    }
}
